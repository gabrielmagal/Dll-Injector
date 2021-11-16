using System;
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

    [Flags()]
    public enum AllocationType : uint
    {
        COMMIT = 0x1000,
        RESERVE = 0x2000,
        RESET = 0x80000,
        LARGE_PAGES = 0x20000000,
        PHYSICAL = 0x400000,
        TOP_DOWN = 0x100000,
        WRITE_WATCH = 0x200000,
        DECOMMIT = 0x4000,
        RELEASE = 0x8000
    }

    [Flags()]
    public enum PageProtection
    {
        NOACCESS = 0x01,
        READONLY = 0x02,
        READWRITE = 0x04,
        WRITECOPY = 0x08,
        EXECUTE = 0x10,
        EXECUTE_READ = 0x20,
        EXECUTE_READWRITE = 0x40,
        EXECUTE_WRITECOPY = 0x80,
        GUARD = 0x100,
        NOCACHE = 0x200,
        WRITECOMBINE = 0x400,
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_DOS_HEADER32
    {
        public ushort e_magic;       // Magic number
        public ushort e_cblp;    // Bytes on last page of file
        public ushort e_cp;      // Pages in file
        public ushort e_crlc;    // Relocations
        public ushort e_cparhdr;     // Size of header in paragraphs
        public ushort e_minalloc;    // Minimum extra paragraphs needed
        public ushort e_maxalloc;    // Maximum extra paragraphs needed
        public ushort e_ss;      // Initial (relative) SS value
        public ushort e_sp;      // Initial SP value
        public ushort e_csum;    // Checksum
        public ushort e_ip;      // Initial IP value
        public ushort e_cs;      // Initial (relative) CS value
        public ushort e_lfarlc;      // File address of relocation table
        public ushort e_ovno;    // Overlay number
        public fixed ushort e_res1[4];    // Reserved words
        public ushort e_oemid;       // OEM identifier (for e_oeminfo)
        public ushort e_oeminfo;     // OEM information; e_oemid specific
        public fixed ushort e_res2[10];    // Reserved words
        public int e_lfanew;      // File address of new exe header
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_NT_HEADERS32
    {
        public uint Signature;
        public IMAGE_FILE_HEADER FileHeader;
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_FILE_HEADER
    {
        public ushort Machine;
        public ushort NumberOfSections;
        public uint TimeDateStamp;
        public uint PointerToSymbolTable;
        public uint NumberOfSymbols;
        public ushort SizeOfOptionalHeader;
        public ushort Characteristics;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_OPTIONAL_HEADER32
    {
        public MagicType Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public uint SizeOfCode;
        public uint SizeOfInitializedData;
        public uint SizeOfUninitializedData;
        public uint AddressOfEntryPoint;
        public uint BaseOfCode;
        // PE32 contains this additional field
        public uint BaseOfData;
        public uint ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort MajorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubsystemVersion;
        public ushort MinorSubsystemVersion;
        public uint Win32VersionValue;
        public uint SizeOfImage;
        public uint SizeOfHeaders;
        public uint CheckSum;
        public SubSystemType Subsystem;
        public DllCharacteristicsType DllCharacteristics;
        public uint SizeOfStackReserve;
        public uint SizeOfStackCommit;
        public uint SizeOfHeapReserve;
        public uint SizeOfHeapCommit;
        public uint LoaderFlags;
        public uint NumberOfRvaAndSizes;
        public IMAGE_DATA_DIRECTORY ExportTable;
        public IMAGE_DATA_DIRECTORY ImportTable;
        public IMAGE_DATA_DIRECTORY ResourceTable;
        public IMAGE_DATA_DIRECTORY ExceptionTable;
        public IMAGE_DATA_DIRECTORY CertificateTable;
        public IMAGE_DATA_DIRECTORY BaseRelocationTable;
        public IMAGE_DATA_DIRECTORY Debug;
        public IMAGE_DATA_DIRECTORY Architecture;
        public IMAGE_DATA_DIRECTORY GlobalPtr;
        public IMAGE_DATA_DIRECTORY TLSTable;
        public IMAGE_DATA_DIRECTORY LoadConfigTable;
        public IMAGE_DATA_DIRECTORY BoundImport;
        public IMAGE_DATA_DIRECTORY IAT;
        public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
        public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
        public IMAGE_DATA_DIRECTORY Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_DATA_DIRECTORY
    {
        public uint VirtualAddress;
        public uint Size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_SECTION_HEADER
    {
        public fixed byte Name[8];
        public uint PhysicalAddress;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public uint PointerToRelocations;
        public uint PointerToLinenumbers;
        public ushort NumberOfRelocations;
        public ushort NumberOfLinenumbers;
        public uint Characteristics;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_DOS_HEADER
    {
        public UInt16 e_magic;       // Magic number
        public UInt16 e_cblp;    // Bytes on last page of file
        public UInt16 e_cp;      // Pages in file
        public UInt16 e_crlc;    // Relocations
        public UInt16 e_cparhdr;     // Size of header in paragraphs
        public UInt16 e_minalloc;    // Minimum extra paragraphs needed
        public UInt16 e_maxalloc;    // Maximum extra paragraphs needed
        public UInt16 e_ss;      // Initial (relative) SS value
        public UInt16 e_sp;      // Initial SP value
        public UInt16 e_csum;    // Checksum
        public UInt16 e_ip;      // Initial IP value
        public UInt16 e_cs;      // Initial (relative) CS value
        public UInt16 e_lfarlc;      // File address of relocation table
        public UInt16 e_ovno;    // Overlay number
        public fixed UInt16 e_res1[4];    // Reserved words
        public UInt16 e_oemid;       // OEM identifier (for e_oeminfo)
        public UInt16 e_oeminfo;     // OEM information; e_oemid specific
        public fixed UInt16 e_res2[10];    // Reserved words
        public Int32 e_lfanew;      // File address of new exe header
    }


    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_BASE_RELOCATION
    {
        public uint VirtualAdress;
        public uint SizeOfBlock;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_IMPORT_DESCRIPTOR
    {
        public uint OriginalFirstThunk;
        public uint TimeDateStamp;
        public uint ForwarderChain;
        public uint Name;
        public uint FirstThunk;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_EXPORT_DIRECTORY
    {
        public uint Characteristics;
        public uint TimeDateStamp;
        public ushort MajorVersion;
        public ushort MinorVersion;
        public uint Name;
        public uint Base;
        public uint NumberOfFunctions;
        public uint NumberOfNames;
        public uint AddressOfFunctions;     // RVA from base of image
        public uint AddressOfNames;         // RVA from base of image
        public uint AddressOfNameOrdinals;  // RVA from base of image
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IMAGE_IMPORT_BY_NAME
    {
        public ushort Hint;
        public fixed byte Name[1];
    }

    public enum MagicType : ushort
    {
        IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b,
        IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b
    }

    public enum SubSystemType : ushort
    {
        IMAGE_SUBSYSTEM_UNKNOWN = 0,
        IMAGE_SUBSYSTEM_NATIVE = 1,
        IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,
        IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,
        IMAGE_SUBSYSTEM_POSIX_CUI = 7,
        IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,
        IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,
        IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,
        IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,
        IMAGE_SUBSYSTEM_EFI_ROM = 13,
        IMAGE_SUBSYSTEM_XBOX = 14

    }

    public enum DllCharacteristicsType : ushort
    {
        RES_0 = 0x0001,
        RES_1 = 0x0002,
        RES_2 = 0x0004,
        RES_3 = 0x0008,
        IMAGE_DLL_CHARACTERISTICS_DYNAMIC_BASE = 0x0040,
        IMAGE_DLL_CHARACTERISTICS_FORCE_INTEGRITY = 0x0080,
        IMAGE_DLL_CHARACTERISTICS_NX_COMPAT = 0x0100,
        IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x0200,
        IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x0400,
        IMAGE_DLLCHARACTERISTICS_NO_BIND = 0x0800,
        RES_4 = 0x1000,
        IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000,
        IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000
    }

    enum BasedRelocationType
    {
        IMAGE_REL_BASED_ABSOLUTE = 0,
        IMAGE_REL_BASED_HIGH = 1,
        IMAGE_REL_BASED_LOW = 2,
        IMAGE_REL_BASED_HIGHLOW = 3,
        IMAGE_REL_BASED_HIGHADJ = 4,
        IMAGE_REL_BASED_MIPS_JMPADDR = 5,
        IMAGE_REL_BASED_MIPS_JMPADDR16 = 9,
        IMAGE_REL_BASED_IA64_IMM64 = 9,
        IMAGE_REL_BASED_DIR64 = 10
    }

    public enum ImageSectionFlags : uint
    {
        IMAGE_SCN_LNK_NRELOC_OVFL = 0x01000000,  // Section contains extended relocations.
        IMAGE_SCN_MEM_DISCARDABLE = 0x02000000,  // Section can be discarded.
        IMAGE_SCN_MEM_NOT_CACHED = 0x04000000,  // Section is not cachable.
        IMAGE_SCN_MEM_NOT_PAGED = 0x08000000, // Section is not pageable.
        IMAGE_SCN_MEM_SHARED = 0x10000000,  // Section is shareable.
        IMAGE_SCN_MEM_EXECUTE = 0x20000000, // Section is executable.
        IMAGE_SCN_MEM_READ = 0x40000000, // Section is readable.
        IMAGE_SCN_MEM_WRITE = 0x80000000  // Section is writeable.
    }

    public enum ImageSectionContains : uint
    {
        CODE = 0x00000020,  // Section contains code.
        INITIALIZED_DATA = 0x00000040,  // Section contains initialized data.
        UNINITIALIZED_DATA = 0x00000080  // Section contains uninitialized data.
    }

    public enum DllReason : uint
    {
        DLL_PROCESS_ATTACH = 1,
        DLL_THREAD_ATTACH = 2,
        DLL_THREAD_DETACH = 3,
        DLL_PROCESS_DETACH = 0
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ManualMappingData
    {
        public IntPtr moduleBase;
        public IntPtr loadLibrary;
        public IntPtr getProcAddress;
    }

    internal static class UnsafeNativeMethods
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll", SetLastError = true)] [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)] [SuppressUnmanagedCodeSecurity] [return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll")] 
		internal static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32")]
        public static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")] 
		internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr CreateRemoteThread(IntPtr hProcess,
		IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

		[DllImport("kernel32.dll", SetLastError = true)] 
		internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);


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

        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                typeof(T));
            handle.Free();
            return stuff;
        }

        public static IntPtr readAddress(byte[] addr)
        {
            return GCHandle.Alloc(addr, GCHandleType.Pinned).AddrOfPinnedObject();
        }

        public static byte[] StructureToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }

	unsafe class PEHeader
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

    unsafe class MethodInjection
    {
        public static IMAGE_SECTION_HEADER* IMAGE_FIRST_SECTION(IMAGE_NT_HEADERS32* ntheader)
        {
            return ((IMAGE_SECTION_HEADER*)((int)ntheader +
                 Marshal.OffsetOf(typeof(IMAGE_NT_HEADERS32), "OptionalHeader").ToInt32() +
                 ntheader->FileHeader.SizeOfOptionalHeader));
        }

        public static string standardA(string path, int pId, int peModify)
        {
            IntPtr hProcess = UnsafeNativeMethods.OpenProcess((uint)ProcessAccessFlags.All, false, pId);
            if (hProcess == IntPtr.Zero) return "Fail to inject, process invalid!";
            IntPtr lpParameter = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)path.Length, (uint)AllocationType.COMMIT | (uint)AllocationType.RESERVE, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
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
            IntPtr lpParameter = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, ((uint)path.Length * 2), (uint)AllocationType.COMMIT | (uint)AllocationType.RESERVE, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
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

        public static string manuallMap(string path, int pId, int peModify)
        {
            byte[] rawImage = File.ReadAllBytes(path);
            if (rawImage.Length < 0x1000) return "Invalid file!";
            IntPtr addr = additionalMethods.readAddress(rawImage);

            IMAGE_DOS_HEADER* idh = (IMAGE_DOS_HEADER*)(addr);

            if (idh->e_magic != 0x5A4D) return "Invalid file!";

            IMAGE_NT_HEADERS32* inth = (IMAGE_NT_HEADERS32*)(addr + idh->e_lfanew);

            if (inth->Signature != 0x00004550 || inth->FileHeader.NumberOfSections > 96 ||
                inth->FileHeader.SizeOfOptionalHeader == 0 || !(inth->FileHeader.Characteristics == 0x2102))
                return "Invalid file!";

            IntPtr hProcess = UnsafeNativeMethods.OpenProcess((uint)ProcessAccessFlags.All, false, pId);
            if (hProcess == IntPtr.Zero) return "Fail to inject, process invalid!";

            IntPtr moduleBase = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero,
                inth->OptionalHeader.SizeOfImage, (uint)AllocationType.COMMIT | (uint)AllocationType.RESERVE,
                (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);

            if (moduleBase == IntPtr.Zero) return "Problem to alloc memory in process!";

            IntPtr outSz;
            UnsafeNativeMethods.WriteProcessMemory(hProcess, moduleBase, rawImage, 0x1000, out outSz);

            if (outSz == IntPtr.Zero) return "Problem to write moduleBase in process!";

            IMAGE_SECTION_HEADER* ish = IMAGE_FIRST_SECTION(inth);

            if (inth->FileHeader.NumberOfSections == 0) return "Invalid number of sections!";

            for (int i = 0; i < inth->FileHeader.NumberOfSections; i++, ish++)
                if (ish->PointerToRawData > 0)
                    UnsafeNativeMethods.WriteProcessMemory(hProcess, moduleBase + (int)ish->VirtualAddress, rawImage.Skip((int)ish->PointerToRawData).ToArray(), (int)ish->SizeOfRawData, out outSz);
                
            Array.Clear(rawImage, 0, rawImage.Length);

            byte[] shell_x86 = new byte[] { 0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x08, 0x53, 0x8B, 0x5D, 0x08, 0x56, 0x57, 0x8B, 0x0B, 0x8B, 0x79, 0x3C, 0x03, 0xF9, 0x89, 0x7D, 0x08, 0x0F, 0x84, 0x35, 0x01, 0x00, 0x00, 0x8B, 0xC1, 0x2B, 0x47, 0x34, 0x89, 0x45, 0xF8, 0x74, 0x67, 0x83, 0xBF, 0xA4, 0x00, 0x00, 0x00, 0x00, 0x74, 0x5E, 0x8B, 0x97, 0xA0, 0x00, 0x00, 0x00, 0x03, 0xD1, 0x83, 0x3A, 0x00, 0x74, 0x51, 0x0F, 0x1F, 0x40, 0x00, 0x8B, 0x4A, 0x04, 0x8D, 0x72, 0x08, 0x33, 0xFF, 0x8D, 0x41, 0xF8, 0xA9, 0xFE, 0xFF, 0xFF, 0xFF, 0x76, 0x31, 0x0F, 0xB7, 0x06, 0x8B, 0xC8, 0x81, 0xE1, 0x00, 0xF0, 0x00, 0x00, 0x81, 0xF9, 0x00, 0x30, 0x00, 0x00, 0x75, 0x0E, 0x8B, 0x4D, 0xF8, 0x25, 0xFF, 0x0F, 0x00, 0x00, 0x03, 0x02, 0x03, 0x03, 0x01, 0x08, 0x8B, 0x4A, 0x04, 0x47, 0x83, 0xC6, 0x02, 0x8D, 0x41, 0xF8, 0xD1, 0xE8, 0x3B, 0xF8, 0x72, 0xCF, 0x03, 0xD1, 0x83, 0x3A, 0x00, 0x75, 0xB6, 0x8B, 0x7D, 0x08, 0x83, 0xBF, 0x84, 0x00, 0x00, 0x00, 0x00, 0x74, 0x77, 0x8B, 0xBF, 0x80, 0x00, 0x00, 0x00, 0x03, 0x3B, 0x89, 0x7D, 0xF8, 0x8B, 0x4F, 0x0C, 0x85, 0xC9, 0x74, 0x62, 0x8B, 0x03, 0x03, 0xC1, 0x50, 0x8B, 0x43, 0x04, 0xFF, 0xD0, 0x89, 0x45, 0xFC, 0x85, 0xC0, 0x0F, 0x84, 0x94, 0x00, 0x00, 0x00, 0x8B, 0x33, 0x8B, 0x0F, 0x85, 0xC9, 0x8B, 0x57, 0x10, 0x8D, 0x3C, 0x32, 0x0F, 0x45, 0xD1, 0x8B, 0x0C, 0x16, 0x03, 0xF2, 0x85, 0xC9, 0x74, 0x25, 0x79, 0x05, 0x0F, 0xB7, 0xC1, 0xEB, 0x07, 0x8B, 0x03, 0x83, 0xC0, 0x02, 0x03, 0xC1, 0x50, 0xFF, 0x75, 0xFC, 0x8B, 0x43, 0x08, 0xFF, 0xD0, 0x83, 0xC6, 0x04, 0x89, 0x07, 0x83, 0xC7, 0x04, 0x8B, 0x0E, 0x85, 0xC9, 0x75, 0xDB, 0x8B, 0x7D, 0xF8, 0x83, 0xC7, 0x14, 0x89, 0x7D, 0xF8, 0x8B, 0x4F, 0x0C, 0x85, 0xC9, 0x75, 0x9E, 0x8B, 0x7D, 0x08, 0x83, 0xBF, 0xC4, 0x00, 0x00, 0x00, 0x00, 0x74, 0x24, 0x8B, 0x03, 0x8B, 0x8F, 0xC0, 0x00, 0x00, 0x00, 0x8B, 0x74, 0x01, 0x0C, 0x8B, 0x06, 0x85, 0xC0, 0x74, 0x12, 0x6A, 0x00, 0x6A, 0x01, 0xFF, 0x33, 0xFF, 0xD0, 0x8B, 0x46, 0x04, 0x8D, 0x76, 0x04, 0x85, 0xC0, 0x75, 0xEE, 0x8B, 0x0B, 0x8B, 0x47, 0x28, 0x6A, 0x00, 0x6A, 0x01, 0x51, 0x03, 0xC1, 0xFF, 0xD0, 0x5F, 0x5E, 0x5B, 0x8B, 0xE5, 0x5D, 0xC2, 0x04, 0x00, 0x5F, 0x5E, 0x32, 0xC0, 0x5B, 0x8B, 0xE5, 0x5D, 0xC2, 0x04 };
            int shell_size = shell_x86.Length;

            IntPtr shellCodeAddress = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero,
               0x1000, (uint)AllocationType.COMMIT | (uint)AllocationType.RESERVE,
               (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);

            if (moduleBase == IntPtr.Zero) return "Problem to alloc memory in process!";

            ManualMappingData mmp;

            mmp.moduleBase = moduleBase;
            mmp.loadLibrary = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "LoadLibraryA");
            mmp.getProcAddress = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "GetProcAddress");

            if(mmp.moduleBase == IntPtr.Zero || mmp.loadLibrary == IntPtr.Zero || mmp.getProcAddress == IntPtr.Zero)
                return "Problem to get address!";

            UnsafeNativeMethods.WriteProcessMemory(hProcess, shellCodeAddress, shell_x86, shell_size, out outSz);

            if (outSz == IntPtr.Zero) return "Problem to write shellCodeAddress!";

            UnsafeNativeMethods.WriteProcessMemory(hProcess, shellCodeAddress + shell_size, additionalMethods.StructureToByteArray(mmp), shell_size, out outSz);

            if (outSz == IntPtr.Zero) return "Problem to write shellCodeAddress!";

            IntPtr hThread = UnsafeNativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0, shellCodeAddress, shellCodeAddress + shell_size, 0, IntPtr.Zero);

            if (hThread == IntPtr.Zero) return "Problem to create remote thread!";

            uint address = 0;
            while (UnsafeNativeMethods.GetExitCodeThread(hThread, out address))
            {
                if (address != 0x103) break;
                Thread.Sleep(10);
            }
            PEHeader.PEHeaderMgr(hProcess, (IntPtr)address, peModify);
            additionalMethods.freeMemory(hProcess, moduleBase, sizeof(IntPtr));
            additionalMethods.freeMemory(hProcess, outSz, sizeof(IntPtr));
            UnsafeNativeMethods.CloseHandle(hThread);
            UnsafeNativeMethods.CloseHandle(hProcess);
            return "successfully injected";
        }
    }
}
