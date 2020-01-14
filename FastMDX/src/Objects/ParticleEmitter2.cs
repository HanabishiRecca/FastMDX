using System.Runtime.InteropServices;

namespace FastMDX {
    public unsafe struct ParticleEmitter2 : IDataRW {
        public Node node;
        public PE2Props properties;
        public Transform<float> emissionRateTransform, gravityTransform, latitudeTransform, speedTransform, visibilityTransform, variationTransform, lengthTransform, widthTransform;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadData(ref node);
            ds.ReadStruct(ref properties);

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)Tags.KP2E) {
                    ds.ReadData(ref emissionRateTransform);
                } else if(tag == (uint)Tags.KP2G) {
                    ds.ReadData(ref gravityTransform);
                } else if(tag == (uint)Tags.KP2L) {
                    ds.ReadData(ref latitudeTransform);
                } else if(tag == (uint)Tags.KP2S) {
                    ds.ReadData(ref speedTransform);
                } else if(tag == (uint)Tags.KP2V) {
                    ds.ReadData(ref visibilityTransform);
                } else if(tag == (uint)Tags.KP2R) {
                    ds.ReadData(ref variationTransform);
                } else if(tag == (uint)Tags.KP2N) {
                    ds.ReadData(ref lengthTransform);
                } else if(tag == (uint)Tags.KP2W) {
                    ds.ReadData(ref widthTransform);
                } else {
                    throw new ParsingException();
                }
            }
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteData(ref node);
            ds.WriteStruct(ref properties);

            if(emissionRateTransform.HasData) {
                ds.WriteStruct(Tags.KP2E);
                ds.WriteData(ref emissionRateTransform);
            }

            if(gravityTransform.HasData) {
                ds.WriteStruct(Tags.KP2G);
                ds.WriteData(ref gravityTransform);
            }

            if(latitudeTransform.HasData) {
                ds.WriteStruct(Tags.KP2L);
                ds.WriteData(ref latitudeTransform);
            }

            if(speedTransform.HasData) {
                ds.WriteStruct(Tags.KP2S);
                ds.WriteData(ref speedTransform);
            }

            if(visibilityTransform.HasData) {
                ds.WriteStruct(Tags.KP2V);
                ds.WriteData(ref visibilityTransform);
            }

            if(variationTransform.HasData) {
                ds.WriteStruct(Tags.KP2R);
                ds.WriteData(ref variationTransform);
            }

            if(lengthTransform.HasData) {
                ds.WriteStruct(Tags.KP2N);
                ds.WriteData(ref lengthTransform);
            }

            if(widthTransform.HasData) {
                ds.WriteStruct(Tags.KP2W);
                ds.WriteData(ref widthTransform);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        enum Tags : uint {
            KP2E = 0x4532504Bu,
            KP2G = 0x4732504Bu,
            KP2L = 0x4C32504Bu,
            KP2S = 0x5332504Bu,
            KP2V = 0x5632504Bu,
            KP2R = 0x5232504Bu,
            KP2N = 0x4E32504Bu,
            KP2W = 0x5732504Bu,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PE2Props {
            public float speed, variation, latitude, gravity, lifespan, emissionRate, length, width;
            public uint filterMode, rows, columns, flag;
            public float tailLength, time;
            public SegmentColor segmentColor;
            public SegmentAlpha segmentAlpha;
            public Vec3 segmentScaling;
            public Interval headInterval, headDecayInterval, tailInterval, tailDecayInterval;
            public uint textureId, squirt, priorityPlane, replaceableId;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SegmentColor {
            public Color color1, color2, color3;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SegmentAlpha {
            public byte alpha1, alpha2, alpha3;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Interval {
            public uint start, end, repeat;
        }
    }
}
