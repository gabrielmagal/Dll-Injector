﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;

namespace Injector
{
    unsafe class Injection
    {
        static Messages messages = new Messages();

        public static Structures.IMAGE_SECTION_HEADER* IMAGE_FIRST_SECTION(Structures.IMAGE_NT_HEADERS32* ntheader)
        {
            return ((Structures.IMAGE_SECTION_HEADER*)((int)ntheader +
                 Marshal.OffsetOf(typeof(Structures.IMAGE_NT_HEADERS32), "OptionalHeader").ToInt32() +
                 ntheader->FileHeader.SizeOfOptionalHeader));
        }

        public static string standardA(string path, int pId, int peModify)
        {
            IntPtr hProcess = UnsafeNativeMethods.OpenProcess((uint)Structures.ProcessAccessFlags.All, false, pId);
            if (hProcess == IntPtr.Zero) return messages.MSG_FAIL_INJECT_PROCESS_INVALID;
            IntPtr lpParameter = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)path.Length,
                (uint)Structures.AllocationType.COMMIT | (uint)Structures.AllocationType.RESERVE, (uint)Structures.AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
            if (lpParameter == IntPtr.Zero) return messages.MSG_FAIL_ALLOC_MEMORY;
            IntPtr outSz;
            UnsafeNativeMethods.WriteProcessMemory(hProcess, lpParameter, Encoding.ASCII.GetBytes(path), path.Length, out outSz);
            if (outSz == IntPtr.Zero) return messages.MSG_FAIL_WRITE_DLL;
            IntPtr lpStartAddress = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "LoadLibraryA");
            if (lpStartAddress == IntPtr.Zero) return messages.MSG_FAIL_GET_LLA;
            IntPtr hThread = UnsafeNativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpStartAddress, lpParameter, 0, IntPtr.Zero);
            if (hThread == IntPtr.Zero) return messages.MSG_FAIL_CREATE_REMOTE_THREAD;
            uint address = 0;
            while (UnsafeNativeMethods.GetExitCodeThread(hThread, out address))
            {
                if (address != 0x103) break;
                Thread.Sleep(10);
            }
            PEHeader.PEHeaderMgr(hProcess, (IntPtr)address, peModify);
            AdditionalMethods.freeMemory(hProcess, lpParameter, path.Length);
            UnsafeNativeMethods.CloseHandle(hThread);
            UnsafeNativeMethods.CloseHandle(hProcess);
            return messages.MSG_SUCESS_INJECT;
        }

        public static string standardW(string path, int pId, int peModify)
        {
            IntPtr hProcess = UnsafeNativeMethods.OpenProcess((uint)Structures.ProcessAccessFlags.All, false, pId);
            if (hProcess == IntPtr.Zero) return messages.MSG_FAIL_INJECT_PROCESS_INVALID;
            IntPtr lpParameter = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, ((uint)path.Length * 2),
                (uint)Structures.AllocationType.COMMIT | (uint)Structures.AllocationType.RESERVE, (uint)Structures.AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
            if (lpParameter == IntPtr.Zero) return messages.MSG_FAIL_ALLOC_MEMORY;
            IntPtr outSz;
            UnsafeNativeMethods.WriteProcessMemory(hProcess, lpParameter, Encoding.ASCII.GetBytes(AdditionalMethods.convert_string_to_wstring(path)), (path.Length * 2), out outSz);
            if (outSz == IntPtr.Zero) return messages.MSG_FAIL_WRITE_DLL;
            IntPtr lpStartAddress = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "LoadLibraryW");
            if (lpStartAddress == IntPtr.Zero) return messages.MSG_FAIL_GET_LLW;
            IntPtr hThread = UnsafeNativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpStartAddress, lpParameter, 0, IntPtr.Zero);
            if (hThread == IntPtr.Zero) return messages.MSG_FAIL_CREATE_REMOTE_THREAD;
            uint address = 0;
            while (UnsafeNativeMethods.GetExitCodeThread(hThread, out address))
            {
                if (address != 0x103) break;
                Thread.Sleep(10);
            }
            PEHeader.PEHeaderMgr(hProcess, (IntPtr)address, peModify);
            AdditionalMethods.freeMemory(hProcess, lpParameter, path.Length * 2);
            UnsafeNativeMethods.CloseHandle(hThread);
            UnsafeNativeMethods.CloseHandle(hProcess);
            return messages.MSG_SUCESS_INJECT;
        }

        public static string manuallMap(string path, int pId, int peModify)
        {
            byte[] rawImage = File.ReadAllBytes(path);
            if (rawImage.Length < 0x1000) return messages.MSG_FAIL_INVALID_FILE;
            IntPtr addr = AdditionalMethods.readAddress(rawImage);

            Structures.IMAGE_DOS_HEADER* idh = (Structures.IMAGE_DOS_HEADER*)(addr);

            if (idh->e_magic != 0x5A4D) return messages.MSG_FAIL_INVALID_FILE;

            Structures.IMAGE_NT_HEADERS32* inth = (Structures.IMAGE_NT_HEADERS32*)(addr + idh->e_lfanew);

            if (inth->Signature != 0x00004550 || inth->FileHeader.NumberOfSections > 96 ||
                inth->FileHeader.SizeOfOptionalHeader == 0 || !(inth->FileHeader.Characteristics == 0x2102))
                return messages.MSG_FAIL_INVALID_FILE;

            IntPtr hProcess = UnsafeNativeMethods.OpenProcess((uint)Structures.ProcessAccessFlags.All, false, pId);
            if (hProcess == IntPtr.Zero) return messages.MSG_FAIL_INJECT_PROCESS_INVALID;

            IntPtr moduleBase = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero,
                inth->OptionalHeader.SizeOfImage, (uint)Structures.AllocationType.COMMIT | (uint)Structures.AllocationType.RESERVE,
                (uint)Structures.AllocationProtectEnum.PAGE_EXECUTE_READWRITE);

            if (moduleBase == IntPtr.Zero) return messages.MSG_FAIL_ALLOC_MEMORY;

            IntPtr outSz;
            UnsafeNativeMethods.WriteProcessMemory(hProcess, moduleBase, rawImage, 0x1000, out outSz);

            if (outSz == IntPtr.Zero) return messages.MSG_FAIL_WRITE_DLL;

            Structures.IMAGE_SECTION_HEADER* ish = IMAGE_FIRST_SECTION(inth);

            if (inth->FileHeader.NumberOfSections == 0) return messages.MSG_FAIL_INVALID_SESSION_NUMBER;

            for (int i = 0; i < inth->FileHeader.NumberOfSections; i++, ish++)
                if (ish->PointerToRawData > 0)
                    UnsafeNativeMethods.WriteProcessMemory(hProcess, moduleBase + (int)ish->VirtualAddress, rawImage.Skip((int)ish->PointerToRawData).ToArray(), (int)ish->SizeOfRawData, out outSz);

            Array.Clear(rawImage, 0, rawImage.Length);

            byte[] shell_x86 = new byte[] { 0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x08, 0x53, 0x8B, 0x5D, 0x08, 0x56, 0x57, 0x8B, 0x0B, 0x8B, 0x79, 0x3C, 0x03, 0xF9, 0x89, 0x7D, 0x08, 0x0F, 0x84, 0x35, 0x01, 0x00, 0x00, 0x8B, 0xC1, 0x2B, 0x47, 0x34, 0x89, 0x45, 0xF8, 0x74, 0x67, 0x83, 0xBF, 0xA4, 0x00, 0x00, 0x00, 0x00, 0x74, 0x5E, 0x8B, 0x97, 0xA0, 0x00, 0x00, 0x00, 0x03, 0xD1, 0x83, 0x3A, 0x00, 0x74, 0x51, 0x0F, 0x1F, 0x40, 0x00, 0x8B, 0x4A, 0x04, 0x8D, 0x72, 0x08, 0x33, 0xFF, 0x8D, 0x41, 0xF8, 0xA9, 0xFE, 0xFF, 0xFF, 0xFF, 0x76, 0x31, 0x0F, 0xB7, 0x06, 0x8B, 0xC8, 0x81, 0xE1, 0x00, 0xF0, 0x00, 0x00, 0x81, 0xF9, 0x00, 0x30, 0x00, 0x00, 0x75, 0x0E, 0x8B, 0x4D, 0xF8, 0x25, 0xFF, 0x0F, 0x00, 0x00, 0x03, 0x02, 0x03, 0x03, 0x01, 0x08, 0x8B, 0x4A, 0x04, 0x47, 0x83, 0xC6, 0x02, 0x8D, 0x41, 0xF8, 0xD1, 0xE8, 0x3B, 0xF8, 0x72, 0xCF, 0x03, 0xD1, 0x83, 0x3A, 0x00, 0x75, 0xB6, 0x8B, 0x7D, 0x08, 0x83, 0xBF, 0x84, 0x00, 0x00, 0x00, 0x00, 0x74, 0x77, 0x8B, 0xBF, 0x80, 0x00, 0x00, 0x00, 0x03, 0x3B, 0x89, 0x7D, 0xF8, 0x8B, 0x4F, 0x0C, 0x85, 0xC9, 0x74, 0x62, 0x8B, 0x03, 0x03, 0xC1, 0x50, 0x8B, 0x43, 0x04, 0xFF, 0xD0, 0x89, 0x45, 0xFC, 0x85, 0xC0, 0x0F, 0x84, 0x94, 0x00, 0x00, 0x00, 0x8B, 0x33, 0x8B, 0x0F, 0x85, 0xC9, 0x8B, 0x57, 0x10, 0x8D, 0x3C, 0x32, 0x0F, 0x45, 0xD1, 0x8B, 0x0C, 0x16, 0x03, 0xF2, 0x85, 0xC9, 0x74, 0x25, 0x79, 0x05, 0x0F, 0xB7, 0xC1, 0xEB, 0x07, 0x8B, 0x03, 0x83, 0xC0, 0x02, 0x03, 0xC1, 0x50, 0xFF, 0x75, 0xFC, 0x8B, 0x43, 0x08, 0xFF, 0xD0, 0x83, 0xC6, 0x04, 0x89, 0x07, 0x83, 0xC7, 0x04, 0x8B, 0x0E, 0x85, 0xC9, 0x75, 0xDB, 0x8B, 0x7D, 0xF8, 0x83, 0xC7, 0x14, 0x89, 0x7D, 0xF8, 0x8B, 0x4F, 0x0C, 0x85, 0xC9, 0x75, 0x9E, 0x8B, 0x7D, 0x08, 0x83, 0xBF, 0xC4, 0x00, 0x00, 0x00, 0x00, 0x74, 0x24, 0x8B, 0x03, 0x8B, 0x8F, 0xC0, 0x00, 0x00, 0x00, 0x8B, 0x74, 0x01, 0x0C, 0x8B, 0x06, 0x85, 0xC0, 0x74, 0x12, 0x6A, 0x00, 0x6A, 0x01, 0xFF, 0x33, 0xFF, 0xD0, 0x8B, 0x46, 0x04, 0x8D, 0x76, 0x04, 0x85, 0xC0, 0x75, 0xEE, 0x8B, 0x0B, 0x8B, 0x47, 0x28, 0x6A, 0x00, 0x6A, 0x01, 0x51, 0x03, 0xC1, 0xFF, 0xD0, 0x5F, 0x5E, 0x5B, 0x8B, 0xE5, 0x5D, 0xC2, 0x04, 0x00, 0x5F, 0x5E, 0x32, 0xC0, 0x5B, 0x8B, 0xE5, 0x5D, 0xC2, 0x04 };
            int shell_size = shell_x86.Length;

            IntPtr shellCodeAddress = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero,
               0x1000, (uint)Structures.AllocationType.COMMIT | (uint)Structures.AllocationType.RESERVE,
               (uint)Structures.AllocationProtectEnum.PAGE_EXECUTE_READWRITE);

            if (moduleBase == IntPtr.Zero) return messages.MSG_FAIL_ALLOC_MEMORY;

            Structures.ManualMappingData mmp;

            mmp.moduleBase = moduleBase;
            mmp.loadLibrary = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "LoadLibraryA");
            mmp.getProcAddress = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "GetProcAddress");

            if (mmp.moduleBase == IntPtr.Zero || mmp.loadLibrary == IntPtr.Zero || mmp.getProcAddress == IntPtr.Zero)
                return messages.MSG_FAIL_GET_LLA_OR_GPA;

            UnsafeNativeMethods.WriteProcessMemory(hProcess, shellCodeAddress, shell_x86, shell_size, out outSz);

            if (outSz == IntPtr.Zero) return messages.MSG_FAIL_WRITE_SHELLCODE_ADDRESS;

            UnsafeNativeMethods.WriteProcessMemory(hProcess, shellCodeAddress + shell_size, AdditionalMethods.StructureToByteArray(mmp), shell_size, out outSz);

            if (outSz == IntPtr.Zero) return messages.MSG_FAIL_WRITE_SHELLCODE_ADDRESS;

            IntPtr hThread = UnsafeNativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0, shellCodeAddress, shellCodeAddress + shell_size, 0, IntPtr.Zero);

            if (hThread == IntPtr.Zero) return messages.MSG_FAIL_CREATE_REMOTE_THREAD;

            uint address = 0;
            while (UnsafeNativeMethods.GetExitCodeThread(hThread, out address))
            {
                if (address != 0x103) break;
                Thread.Sleep(10);
            }
            PEHeader.PEHeaderMgr(hProcess, (IntPtr)address, peModify);
            AdditionalMethods.freeMemory(hProcess, moduleBase, sizeof(IntPtr));
            AdditionalMethods.freeMemory(hProcess, outSz, sizeof(IntPtr));
            UnsafeNativeMethods.CloseHandle(hThread);
            UnsafeNativeMethods.CloseHandle(hProcess);
            return messages.MSG_SUCESS_INJECT;
        }
    }
}
