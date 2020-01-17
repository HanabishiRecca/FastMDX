using System.Runtime.InteropServices;
using LT = FastMDX.GeosetAnimation;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct GeosetAnimation : IDataRW {
        public LocalProperties Properties;
        public Transform<float> GeosetAlpha;
        public Transform<Vec4> GeosetColor;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();
            ds.ReadStruct(ref Properties);
            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));
            ds.WriteStruct(ref Properties);
            ds.WriteOptionalBlocks(ref this, _knownTransforms);
            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KGAO] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.GeosetAlpha),
            [KGAC] = new OptionalBlockParser<Transform<Vec4>, LT>((ref LT p) => ref p.GeosetColor),
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LocalProperties {
            public float Alpha;
            public uint Flags;
            public Color Color;
            public int GeosetId;
        }
    }
}
