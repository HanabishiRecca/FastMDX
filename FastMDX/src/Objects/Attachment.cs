using LT = FastMDX.Attachment;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct Attachment : IDataRW {
        public Node node;
        fixed byte name[(int)PATH_LEN];
        public uint attachmentId;
        public Transform<float> visibilityTransform;

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

            fixed(byte* n = name)
                ds.ReadUnmanagedArray(n, PATH_LEN);

            ds.ReadStruct(ref attachmentId);

            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteData(ref node);

            fixed(byte* n = name)
                ds.WriteUnmanagedArray(n, PATH_LEN);

            ds.WriteStruct(attachmentId);

            ds.WriteOptionalBlocks(ref this, _knownTransforms);

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KATV] = new OptionalBlockParser<Transform<float>, LT>((ref LT p) => ref p.visibilityTransform),
        };
    }
}
