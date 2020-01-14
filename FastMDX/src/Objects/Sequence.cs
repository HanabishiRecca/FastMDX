using System.Runtime.InteropServices;

namespace FastMDX {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Sequence {
        fixed byte name[(int)NAME_LEN];
        public uint intervalStart, intervalEnd;
        public float moveSpeed;
        public uint flags;
        public float rarity;
        public uint syncPoint;
        public Extent extent;

        const uint
            NAME_LEN = 80;

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
