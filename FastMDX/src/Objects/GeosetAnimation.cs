using LT = FastMDX.GeosetAnimation;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct GeosetAnimation : IDataRW {
        public float alpha;
        public uint flags, geosetId;
        public Color color;
        public Transform<float> geosetAlpha;
        public Transform<Vec4> geosetColor;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadStruct(ref alpha);
            ds.ReadStruct(ref flags);
            ds.ReadStruct(ref color);
            ds.ReadStruct(ref geosetId);

            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteStruct(alpha);
            ds.WriteStruct(flags);
            ds.WriteStruct(color);
            ds.WriteStruct(geosetId);

            ds.WriteOptionalBlocks(ref this, _knownTransforms);

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KGAO] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.geosetAlpha),
            [KGAC] = new OptionalBlockParser<Transform<Vec4>, LT>((ref LT p) => ref p.geosetColor),
        };
    }
}
