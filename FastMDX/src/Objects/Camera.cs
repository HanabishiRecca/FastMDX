using LT = FastMDX.Camera;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct Camera : IDataRW {
        fixed byte name[(int)NAME_LEN];
        public Vec3 position, targetPosition;
        public uint filedOfView, farClippingPlane, nearClippingPlane;
        public Transform<Vec3> translation, targetTranslation;
        public Transform<uint> rotation;

        const uint NAME_LEN = 80;

        public string Name {
            get {
                fixed(byte* n = name)
                    return BinaryString.Decode(n, NAME_LEN);
            }
            set {
                fixed(byte* n = name)
                    BinaryString.Encode(value, n, NAME_LEN);
            }
        }

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            fixed(byte* n = name)
                ds.ReadUnmanagedArray(n, NAME_LEN);

            ds.ReadStruct(ref position);
            ds.ReadStruct(ref filedOfView);
            ds.ReadStruct(ref farClippingPlane);
            ds.ReadStruct(ref nearClippingPlane);
            ds.ReadStruct(ref targetPosition);

            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            fixed(byte* n = name)
                ds.WriteUnmanagedArray(n, NAME_LEN);

            ds.WriteStruct(position);
            ds.WriteStruct(filedOfView);
            ds.WriteStruct(farClippingPlane);
            ds.WriteStruct(nearClippingPlane);
            ds.WriteStruct(targetPosition);

            ds.WriteOptionalBlocks(ref this, _knownTransforms);

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KCTR] = new OptionalBlockParser<Transform<Vec3>, LT>((ref LT p) => ref p.translation),
            [KCRL] = new OptionalBlockParser<Transform<uint>, LT>((ref LT p) => ref p.rotation),
            [KTTR] = new OptionalBlockParser<Transform<Vec3>, LT>((ref LT p) => ref p.targetTranslation),
        };
    }
}
