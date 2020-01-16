using LT = FastMDX.Layer;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public struct Layer : IDataRW {
        public uint filterMode, shadingFlags, textureId, textureAnimationId, coordId;
        public float alpha;
        public Transform<uint> materialTextureId;
        public Transform<float> materialAlpha;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadStruct(ref filterMode);
            ds.ReadStruct(ref shadingFlags);
            ds.ReadStruct(ref textureId);
            ds.ReadStruct(ref textureAnimationId);
            ds.ReadStruct(ref coordId);
            ds.ReadStruct(ref alpha);

            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteStruct(filterMode);
            ds.WriteStruct(shadingFlags);
            ds.WriteStruct(textureId);
            ds.WriteStruct(textureAnimationId);
            ds.WriteStruct(coordId);
            ds.WriteStruct(alpha);

            ds.WriteOptionalBlocks(ref this, _knownTransforms);

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KMTF] = new OptionalBlockParser<Transform<uint>, LT>((ref LT p) => ref p.materialTextureId),
            [KMTA] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.materialAlpha),
        };
    }
}
