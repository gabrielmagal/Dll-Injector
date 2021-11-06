using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
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
		internal static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
		
		[DllImport("kernel32.dll")] 
		internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr CreateRemoteThread(IntPtr hProcess,
		IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

		[DllImport("kernel32.dll", SetLastError = true)] 
		internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetDllDirectory(string lpPathName);
	}

	internal class PEHeader
	{
	}

	internal class MethodInjection
    {
		public static void standardA(string path, int pId, int peModify, int closeOnInject)
        {
			IntPtr hProcess = UnsafeNativeMethods.OpenProcess((uint)ProcessAccessFlags.All, false, pId);
			IntPtr lpParameter = UnsafeNativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)path.Length+1, (uint)AllocationType.Commit | (uint)AllocationType.Reserve, (uint)AllocationProtectEnum.PAGE_EXECUTE_READWRITE);
			IntPtr outSz;
			UnsafeNativeMethods.WriteProcessMemory(hProcess, lpParameter, Encoding.ASCII.GetBytes(path), path.Length + 1, out outSz);
			IntPtr lpStartAddress = UnsafeNativeMethods.GetProcAddress(UnsafeNativeMethods.GetModuleHandle("Kernel32.dll"), "LoadLibraryA");
			IntPtr hThread = UnsafeNativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpStartAddress, lpParameter, 0, IntPtr.Zero);
			UnsafeNativeMethods.CloseHandle(hThread);
			UnsafeNativeMethods.CloseHandle(hProcess);
		}
	}
}
