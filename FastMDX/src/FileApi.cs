using System;
using System.Runtime.InteropServices;

namespace FastMDX {

    // This class required for cross-platform unmanaged access to the filesystem

    static class FileApi {
        static readonly bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        internal static unsafe uint ReadFile(SafeHandle fileHandle, void* buffer, uint count) {
            if(isWindows) {
                var state = W_API.ReadFile(fileHandle, buffer, count, out var len, IntPtr.Zero);
                if(state == 0)
                    throw new Exception();
                return len;
            } else {
                return N_API.Read(fileHandle, buffer, count);
            }
        }

        internal static unsafe uint WriteFile(SafeHandle fileHandle, void* buffer, uint count) {
            if(isWindows) {
                var state = W_API.WriteFile(fileHandle, buffer, count, out var len, IntPtr.Zero);
                if(state == 0)
                    throw new Exception();
                return len;
            } else {
                return N_API.Write(fileHandle, buffer, count);
            }
        }

        static class W_API {
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern unsafe int ReadFile(SafeHandle handle, void* buffer, uint numBytesToRead, out uint numBytesRead, IntPtr mustBeZero);

            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern unsafe int WriteFile(SafeHandle handle, void* buffer, uint numBytesToWrite, out uint numBytesWritten, IntPtr mustBeZero);
        }

        static class N_API {
            [DllImport("System.Native", EntryPoint = "SystemNative_Read", SetLastError = true)]
            internal static extern unsafe uint Read(SafeHandle fd, void* buffer, uint count);

            [DllImport("System.Native", EntryPoint = "SystemNative_Write", SetLastError = true)]
            internal static extern unsafe uint Write(SafeHandle fd, void* buffer, uint count);
        }
    }
}
