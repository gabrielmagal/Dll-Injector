using System;
using System.Runtime.InteropServices;

namespace Injector
{
    internal class Structures
    {
        [Flags]
        public enum AllocationProtectEnum
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
        public enum ProcessAccessFlags
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

        public enum BasedRelocationType
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
    }
}
