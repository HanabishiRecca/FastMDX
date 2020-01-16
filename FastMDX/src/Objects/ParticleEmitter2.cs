using System.Runtime.InteropServices;
using LT = FastMDX.ParticleEmitter2;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct ParticleEmitter2 : IDataRW {
        public Node node;
        public PE2Props properties;
        public Transform<float> emissionRateTransform, gravityTransform, latitudeTransform, speedTransform, visibilityTransform, variationTransform, lengthTransform, widthTransform;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadData(ref node);
            ds.ReadStruct(ref properties);

            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteData(ref node);
            ds.WriteStruct(ref properties);

            ds.WriteOptionalBlocks(ref this, _knownTransforms);

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KP2E] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.emissionRateTransform),
            [KP2G] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.gravityTransform),
            [KP2L] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.latitudeTransform),
            [KP2S] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.speedTransform),
            [KP2V] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.visibilityTransform),
            [KP2R] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.variationTransform),
            [KP2N] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.lengthTransform),
            [KP2W] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.widthTransform),
        };

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
