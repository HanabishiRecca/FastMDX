namespace FastMDX {
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

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)Tags.KGTR) {
                    ds.ReadData(ref translation);
                } else if(tag == (uint)Tags.KGRT) {
                    ds.ReadData(ref rotation);
                } else if(tag == (uint)Tags.KGSC) {
                    ds.ReadData(ref scaling);
                } else {
                    throw new ParsingException();
                }
            }
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            fixed(byte* n = name)
                ds.WriteUnmanagedArray(n, NAME_LEN);

            ds.WriteStruct(objectId);
            ds.WriteStruct(parentId);
            ds.WriteStruct(flags);

            if(translation.HasData) {
                ds.WriteStruct(Tags.KGTR);
                ds.WriteData(ref translation);
            }

            if(rotation.HasData) {
                ds.WriteStruct(Tags.KGRT);
                ds.WriteData(ref rotation);
            }

            if(scaling.HasData) {
                ds.WriteStruct(Tags.KGSC);
                ds.WriteData(ref scaling);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        enum Tags : uint {
            KGTR = 0x5254474Bu,
            KGRT = 0x5452474Bu,
            KGSC = 0x4353474Bu,
        }
    }
}
