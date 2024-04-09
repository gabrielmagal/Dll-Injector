using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector
{
    internal class PEHeader
    {
        public static void EraseEx(IntPtr hProcess, IntPtr lpStartAddress)
        {
            uint Protect;
            UnsafeNativeMethods.VirtualProtectEx(hProcess, lpStartAddress, 4096, (uint)Structures.AllocationProtectEnum.PAGE_EXECUTE_READWRITE, out Protect);
            AdditionalMethods.freeMemory(hProcess, lpStartAddress, 4096);
            UnsafeNativeMethods.VirtualProtectEx(hProcess, lpStartAddress, 4096, Protect, out _);
        }

        public static void RandomEx(IntPtr hProcess, IntPtr lpStartAddress)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            byte[] buffer = new byte[4096];
            for (int i = 0; i < 4096; i++)
                buffer[i] = (byte)rnd.Next(255);
            uint Protect;
            UnsafeNativeMethods.VirtualProtectEx(hProcess, lpStartAddress, 4096, (uint)Structures.AllocationProtectEnum.PAGE_EXECUTE_READWRITE, out Protect);
            IntPtr outSz;
            UnsafeNativeMethods.WriteProcessMemory(hProcess, lpStartAddress, buffer, 4096, out outSz);
            UnsafeNativeMethods.VirtualProtectEx(hProcess, lpStartAddress, 4096, Protect, out Protect);
        }

        public static void PEHeaderMgr(IntPtr hProcess, IntPtr lpStartAddress, int PEpeModify)
        {
            if (PEpeModify == 0)
                EraseEx(hProcess, lpStartAddress);
            if (PEpeModify == 1)
                RandomEx(hProcess, lpStartAddress);
        }
    }
}
