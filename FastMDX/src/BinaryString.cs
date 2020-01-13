using System;
using System.Text;

namespace FastMDX {
    static class BinaryString {
        internal static string Decode(byte[] bytes) {
            if(bytes is null)
                return null;

            var index = Array.IndexOf(bytes, (byte)0);
            index = (index > -1) ? Math.Min(bytes.Length, index) : bytes.Length;
            return Encoding.ASCII.GetString(bytes, 0, index);
        }

        internal static void Encode(string str, ref byte[] bytes, uint defLen) {
            if(bytes is null)
                bytes = new byte[defLen];
            else
                Array.Clear(bytes, 0, bytes.Length);

            if(str?.Length > 0)
                Encoding.ASCII.GetBytes(str, 0, Math.Min(str.Length, bytes.Length), bytes, 0);
        }
    }
}
