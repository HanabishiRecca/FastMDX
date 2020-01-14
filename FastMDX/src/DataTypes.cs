using System.Runtime.InteropServices;

namespace FastMDX {

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec2 {
        public float x, y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec3 {
        public float x, y, z;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec4 {
        public float x, y, z, w;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Color {
        public float b, g, r;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Extent {
        public float boundsRadius;
        public Vec3 minimum, maximum;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GlobalSequence {
        public uint duration;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Track<T> where T : unmanaged {
        public int frame;
        public T tr;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TrackInter<T> where T : unmanaged {
        public int frame;
        public T tr, inTr, outTr;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MDXHeader {
        internal uint header, versionHeader, versionSize, version;

        const uint
            MDLX_HEADER = 0x584C444Du,
            VERS_HEADER = 0x53524556u;

        internal bool Check() => (header == MDLX_HEADER) && (versionHeader == VERS_HEADER) && (versionSize == sizeof(uint));

        internal void Default() {
            header = MDLX_HEADER;
            versionHeader = VERS_HEADER;
            versionSize = sizeof(uint);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BlockHeader {
        internal uint tag, size;
    }

    public class BinaryBlock {
        public uint Tag;
        public byte[] Data;
    }
}
