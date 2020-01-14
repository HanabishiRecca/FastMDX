namespace FastMDX {
    public unsafe struct Attachment : IDataRW {
        public Node node;
        fixed byte name[(int)PATH_LEN];
        public uint attachmentId;
        public Transform<float> visibilityTransform;

        const uint PATH_LEN = 260;

        public string Name {
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

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)Tags.KATV) {
                    ds.ReadData(ref visibilityTransform);
                } else {
                    throw new ParsingException();
                }
            }
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteData(ref node);

            fixed(byte* n = name)
                ds.WriteUnmanagedArray(n, PATH_LEN);

            ds.WriteStruct(attachmentId);

            if(visibilityTransform.HasData) {
                ds.WriteStruct(Tags.KATV);
                ds.WriteData(ref visibilityTransform);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        enum Tags : uint {
            KATV = 0x5654414Bu,
        }
    }
}
