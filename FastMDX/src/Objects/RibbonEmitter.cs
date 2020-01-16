using System.Runtime.InteropServices;
using LT = FastMDX.RibbonEmitter;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct RibbonEmitter : IDataRW {
        public Node node;
        public REProps properties;
        public Transform<float> visibilityTransform, heightAboveTransform, heightBelowTransform, alphaTransform, colorTransform, textureSlotTransform;

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
            [KRVS] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.visibilityTransform),
            [KRHA] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.heightAboveTransform),
            [KRHB] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.heightBelowTransform),
            [KRAL] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.alphaTransform),
            [KRCO] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.colorTransform),
            [KRTX] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.textureSlotTransform),
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct REProps {
            public float heightAbove, heightBelow, alpha;
            public Color color;
            public float lifespan;
            public uint textureSlot, emissionRate, rows, columns, materialId;
            public float gravity;
        }
    }
}
