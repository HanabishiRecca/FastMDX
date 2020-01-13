using System;
using System.Text;

namespace FastMDX {
    static class BinaryString {
        internal static string Decode(byte[] value) {
            if(value is null)
                return null;

            var index = Array.IndexOf(value, (byte)0);
            index = (index > -1) ? Math.Min(value.Length, index) : value.Length;
            return Encoding.ASCII.GetString(value, 0, index);
        }

        internal static byte[] Encode(string value, uint len) =>
            (value?.Length > 0) ?
            Encoding.ASCII.GetBytes(value, 0, Math.Min(value.Length, (int)len)) :
            new byte[len];
    }
}
