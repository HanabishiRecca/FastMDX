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
    public struct Pivot {
        public Vec3 position;
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
}
