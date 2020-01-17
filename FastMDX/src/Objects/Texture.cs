using System.Runtime.InteropServices;

namespace FastMDX {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Texture {
        const uint NAME_LEN = 260;

        public int ReplaceableId;
        fixed byte name[(int)NAME_LEN];
        public uint Flags;

        public string Name {
            get {
                fixed(byte* n = name)
                    return BinaryString.Decode(n, NAME_LEN);
            }
            set {
                fixed(byte* n = name)
                    BinaryString.Encode(value, n, NAME_LEN);
            }
        }
    }
}
