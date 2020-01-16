using LT = FastMDX.ParticleEmitter;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct ParticleEmitter : IDataRW {
        public Node node;
        public float emissionRate, gravity, longitude, latitude, lifespan, speed;
        fixed byte name[(int)PATH_LEN];
        public Transform<float> emissionRateTransform, gravityTransform, longitudeTransform, latitudeTransform, lifespanTransform, speedTransform, visibilityTransform;

        const uint PATH_LEN = 260;

        public string Path {
            get {
                fixed(byte* n = name)
                    return BinaryString.Decode(n, PATH_LEN);
            }
            set {
                fixed(byte* n = name)
                    BinaryString.Encode(value, n, PATH_LEN);
            }
        }

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadData(ref node);

            ds.ReadStruct(ref emissionRate);
            ds.ReadStruct(ref gravity);
            ds.ReadStruct(ref longitude);
            ds.ReadStruct(ref latitude);

            fixed(byte* n = name)
                ds.ReadUnmanagedArray(n, PATH_LEN);

            ds.ReadStruct(ref lifespan);
            ds.ReadStruct(ref speed);

            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteData(ref node);

            ds.WriteStruct(emissionRate);
            ds.WriteStruct(gravity);
            ds.WriteStruct(longitude);
            ds.WriteStruct(latitude);

            fixed(byte* n = name)
                ds.WriteUnmanagedArray(n, PATH_LEN);

            ds.WriteStruct(lifespan);
            ds.WriteStruct(speed);

            ds.WriteOptionalBlocks(ref this, _knownTransforms);

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KPEE] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.emissionRateTransform),
            [KPEG] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.gravityTransform),
            [KPLN] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.longitudeTransform),
            [KPLT] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.latitudeTransform),
            [KPEL] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.lifespanTransform),
            [KPES] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.speedTransform),
            [KPEV] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.visibilityTransform),
        };
    }
}
