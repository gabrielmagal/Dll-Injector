using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Injector
{
	[Flags]
	enum AllocationProtectEnum
	{
		PAGE_EXECUTE = 0x00000010,
		PAGE_EXECUTE_READ = 0x00000020,
		PAGE_EXECUTE_READWRITE = 0x00000040,
		PAGE_EXECUTE_WRITECOPY = 0x00000080,
		PAGE_NOACCESS = 0x00000001,
		PAGE_READONLY = 0x00000002,
		PAGE_READWRITE = 0x00000004,
		PAGE_WRITECOPY = 0x00000008,
		PAGE_GUARD = 0x00000100,
		PAGE_NOCACHE = 0x00000200,
		PAGE_WRITECOMBINE = 0x00000400
	}

	[Flags]
	enum AllocationType
	{
		Commit = 0x1000,
		Reserve = 0x2000,
		Decommit = 0x4000,
		Release = 0x8000,
		Reset = 0x80000,
		Physical = 0x400000,
		TopDown = 0x100000,
		WriteWatch = 0x200000,
		LargePages = 0x20000000
	}

	[Flags]
	enum ProcessAccessFlags
	{
		All = 0x001F0FFF,
		Terminate = 0x00000001,
		CreateThread = 0x00000002,
		VirtualMemoryOperation = 0x00000008,
		VirtualMemoryRead = 0x00000010,
		VirtualMemoryWrite = 0x00000020,
		DuplicateHandle = 0x00000040,
		CreateProcess = 0x000000080,
		SetQuota = 0x00000100,
		SetInformation = 0x00000200,
		QueryInformation = 0x00000400,
		QueryLimitedInformation = 0x00001000,
		Synchronize = 0x00100000
	}

	internal static class UnsafeNativeMethods
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] 
		internal static extern IntPtr LoadLibrary(string lpFileName);
		
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll", SetLastError = true)] [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)] [SuppressUnmanagedCodeSecurity] [return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll")] 
		internal static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);
		
		[DllImport("kernel32.dll")] 
		internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr CreateRemoteThread(IntPtr hProcess,
		IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

		[DllImport("kernel32.dll", SetLastError = true)] 
		internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);

		[DllImport("kernel32.dll")]
		internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetDllDirectory(string lpPathName);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);
	}

	internal class additionalMethods
	{
		public static void freeMemory(IntPtr hProcess, IntPtr address, int size)
		{
			byte[] buff = new byte[size];
			for (int i = 0; i < size; i++) buff[i] = 0;
			IntPtr outSz;
			UnsafeNativeMethods.WriteProcessMemory(hProcess, address, buff, size, out outSz);
		}

		public static string convert_string_to_wstring(string path)
		{
			string tmp = "";
			for (int i = 0; i < (path.Length * 2); i++)
			{
				if (i % 2 == 0) tmp += path[i / 2];
				else tmp += (char)0;
			}
			return tmp;
		}
	}

	internal class PEHeader
	{
		public static void EraseEx(IntPtr hProcess, IntPtr lpStartAddress)
		{
			uint Protect;
			UnsafeNativeMethods.VirtualProtectEx(hProcess, lpStartAddress, 4096, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE, out Protect);
			additionalMethods.freeMemory(hProcess, lpStartAddress, 4096);
			UnsafeNativeMethods.VirtualProtectEx(hProcess, lpStartAddress, 4096, Protect, out Protect);
		}

		public static void RandomEx(IntPtr hProcess, IntPtr lpStartAddress)
		{
			Random rnd = new Random((int)DateTime.Now.Ticks);
			byte[] buffer = new byte[4096];
			for (int i = 0; i < 4096; i++) buffer[i] = (byte)rnd.Next(255);
			uint Protect;
			UnsafeNativeMethods.VirtualProtectEx(hProcess, lpStartAddress, 4096, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE, out Protect);
			IntPtr outSz;
			UnsafeNativeMethods.WriteProcessMemory(hProcess, lpStartAddress, buffer, 4096, out outSz);
			UnsafeNativeMethods.VirtualProtectEx(hProcess, lpStartAddress, 4096, Protect, out Protect);
		}

		public static void PEHeaderMgr(IntPtr hProcess, IntPtr lpStartAddress, int PEpeModify)
        {
			if(PEpeModify == 0) EraseEx(hProcess, lpStartAddress);
			if(PEpeModify == 1) RandomEx(hProcess, lpStartAddress);
		}
	}

	internal class MethodInjection
    {
		public static string standardA(string path, int pId, int peModify)
        {
			IntPtr hProcess = UnsafeNativeMethods.OpenProcess((uint)ProcessAccessFlags.All, false, pId);
			if (hProcess == IntPtr.Zero) return "Fail to inject, process invalid!";
			IntPtr lpParameter = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)path.Length, (uint)AllocationType.Commit | (uint)AllocationType.Reserve, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
			if (lpParameter == IntPtr.Zero) return "Problem to alloc memory in process!";
			IntPtr outSz;
			UnsafeNativeMethods.WriteProcessMemory(hProcess, lpParameter, Encoding.ASCII.GetBytes(path), path.Length, out outSz);
			if (outSz == IntPtr.Zero) return "Problem to write dll path in process!";
			IntPtr lpStartAddress = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "LoadLibraryA");
			if (lpStartAddress == IntPtr.Zero) return "Problem to get address LoadLibraryA!";
			IntPtr hThread = UnsafeNativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpStartAddress, lpParameter, 0, IntPtr.Zero);
			if (hThread == IntPtr.Zero) return "Problem to create remote thread!";
			uint address = 0;
			while (UnsafeNativeMethods.GetExitCodeThread(hThread, out address))
			{
				if (address != 0x103) break;
				Thread.Sleep(10);
			}
			PEHeader.PEHeaderMgr(hProcess, (IntPtr)address, peModify);
			additionalMethods.freeMemory(hProcess, lpParameter, path.Length);
			UnsafeNativeMethods.CloseHandle(hThread);
			UnsafeNativeMethods.CloseHandle(hProcess);
			return "successfully injected";
		}

		public static string standardW(string path, int pId, int peModify)
		{
			IntPtr hProcess = UnsafeNativeMethods.OpenProcess((uint)ProcessAccessFlags.All, false, pId);
			if (hProcess == IntPtr.Zero) return "Fail to inject, process invalid!";
			IntPtr lpParameter = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, ((uint)path.Length * 2), (uint)AllocationType.Commit | (uint)AllocationType.Reserve, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
			if (lpParameter == IntPtr.Zero) return "Problem to alloc memory in process!";
			IntPtr outSz;
			UnsafeNativeMethods.WriteProcessMemory(hProcess, lpParameter, Encoding.ASCII.GetBytes(additionalMethods.convert_string_to_wstring(path)), (path.Length * 2), out outSz);
			if (outSz == IntPtr.Zero) return "Problem to write dll path in process!";
			IntPtr lpStartAddress = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "LoadLibraryW");
			if (lpStartAddress == IntPtr.Zero) return "Problem to get address LoadLibraryW!";
			IntPtr hThread = UnsafeNativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpStartAddress, lpParameter, 0, IntPtr.Zero);
			if (hThread == IntPtr.Zero) return "Problem to create remote thread!";
			uint address = 0;
			while (UnsafeNativeMethods.GetExitCodeThread(hThread, out address))
			{
				if (address != 0x103) break;
				Thread.Sleep(10);
			}
			PEHeader.PEHeaderMgr(hProcess, (IntPtr)address, peModify);
			additionalMethods.freeMemory(hProcess, lpParameter, path.Length * 2);
			UnsafeNativeMethods.CloseHandle(hThread);
			UnsafeNativeMethods.CloseHandle(hProcess);
			return "successfully injected";
		}
	}
}
