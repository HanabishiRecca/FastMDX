using System.Runtime.InteropServices;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<Node>>;

    public unsafe struct Node : IDataRW {
        public LocalProperties Properties;
        public Transform<Vec3> Translation, Scaling;
        public Transform<Vec4> Rotation;

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
            [KGTR] = new OptionalBlockParser<Transform<Vec3>, Node>((ref Node p) => ref p.Translation),
            [KGRT] = new OptionalBlockParser<Transform<Vec4>, Node>((ref Node p) => ref p.Rotation),
            [KGSC] = new OptionalBlockParser<Transform<Vec3>, Node>((ref Node p) => ref p.Scaling),
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LocalProperties {
            const uint NAME_LEN = 80;

            fixed byte name[(int)NAME_LEN];
            public int ObjectId, ParentId;
            public uint Flags;

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
        }
    }
}
