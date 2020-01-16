using LT = FastMDX.Light;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct Light : IDataRW {
        public Node node;
        public uint type;
        public float attenuationStart, attenuationEnd, intensity, ambientIntensity;
        public Color color, ambientColor;
        public Transform<uint> attenuationStartTransform, attenuationEndTransform;
        public Transform<Color> colorTransform, ambientColorTransform;
        public Transform<float> intensityTransform, ambientIntensityTransform, visibilityTransform;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadData(ref node);

            ds.ReadStruct(ref type);
            ds.ReadStruct(ref attenuationStart);
            ds.ReadStruct(ref attenuationEnd);
            ds.ReadStruct(ref color);
            ds.ReadStruct(ref intensity);
            ds.ReadStruct(ref ambientColor);

            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteData(ref node);

            ds.WriteStruct(type);
            ds.WriteStruct(attenuationStart);
            ds.WriteStruct(attenuationEnd);
            ds.WriteStruct(color);
            ds.WriteStruct(intensity);
            ds.WriteStruct(ambientColor);

            ds.WriteOptionalBlocks(ref this, _knownTransforms);

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KLAS] = new OptionalBlockParser<Transform<uint>, LT>((ref LT p) => ref p.attenuationStartTransform),
            [KLAE] = new OptionalBlockParser<Transform<uint>, LT>((ref LT p) => ref p.attenuationEndTransform),
            [KLAC] = new OptionalBlockParser<Transform<Color>, LT>((ref LT p) => ref p.colorTransform),
            [KLAI] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.intensityTransform),
            [KLBI] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.ambientIntensityTransform),
            [KLBC] = new OptionalBlockParser<Transform<Color>, LT>((ref LT p) => ref p.ambientColorTransform),
            [KLAV] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.visibilityTransform),
        };
    }
}
