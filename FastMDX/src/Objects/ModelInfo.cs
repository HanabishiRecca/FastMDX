using System.Runtime.InteropServices;

namespace FastMDX {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ModelInfo {
        fixed byte name[(int)NAME_LEN], animationFile[(int)ANIM_LEN];
        public Extent extent;
        public uint blendTime;

        const uint
            NAME_LEN = 80,
            ANIM_LEN = 260;

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

        public string AnimationFileName {
            get {
                fixed(byte* n = animationFile)
                    return BinaryString.Decode(n, ANIM_LEN);
            }
            set {
                fixed(byte* n = animationFile)
                    BinaryString.Encode(value, n, ANIM_LEN);
            }
        }
    }
}
