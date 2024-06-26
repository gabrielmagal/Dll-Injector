﻿using System;
using System.Runtime.InteropServices;

namespace Injector
{
    internal class AdditionalMethods
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
}
