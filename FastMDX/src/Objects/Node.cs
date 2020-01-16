using LT = FastMDX.Node;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct Node : IDataRW {
        fixed byte name[(int)NAME_LEN];
        public uint objectId, parentId, flags;
        public Transform<Vec3> translation, scaling;
        public Transform<Vec4> rotation;

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

            ds.ReadStruct(ref objectId);
            ds.ReadStruct(ref parentId);
            ds.ReadStruct(ref flags);

            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            fixed(byte* n = name)
                ds.WriteUnmanagedArray(n, NAME_LEN);

            ds.WriteStruct(objectId);
            ds.WriteStruct(parentId);
            ds.WriteStruct(flags);

            ds.WriteOptionalBlocks(ref this, _knownTransforms);

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KGTR] = new OptionalBlockParser<Transform<Vec3>, LT>((ref LT p) => ref p.translation),
            [KGRT] = new OptionalBlockParser<Transform<Vec4>, LT>((ref LT p) => ref p.rotation),
            [KGSC] = new OptionalBlockParser<Transform<Vec3>, LT>((ref LT p) => ref p.scaling),
        };
    }
}
