using System.Runtime.InteropServices;
using LT = FastMDX.Layer;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public struct Layer : IDataRW {
        public LocalProperties Properties;
        public Transform<int> MaterialTextureIdTransform;
        public Transform<float> MaterialAlphaTransform;

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
            [KMTF] = new OptionalBlockParser<Transform<int>, LT>((ref LT p) => ref p.MaterialTextureIdTransform),
            [KMTA] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.MaterialAlphaTransform),
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LocalProperties {
            public uint FilterMode, ShadingFlags;
            public int TextureId, TextureAnimationId, CoordId;
            public float Alpha;
        }
    }
}
