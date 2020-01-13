namespace FastMDX {
    public struct Node : IDataRW {
        byte[] name;
        public uint objectId, parentId, flags;
        public Transform<Vec3> translation, scaling;
        public Transform<Vec4> rotation;

        const uint NAME_LEN = 80;

        public string Name {
            get => BinaryString.Decode(name);
            set => BinaryString.Encode(value, ref name, NAME_LEN);
        }

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            name = ds.ReadStructArray<byte>(NAME_LEN);

            ds.ReadStruct(ref objectId);
            ds.ReadStruct(ref parentId);
            ds.ReadStruct(ref flags);

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)TransformTags.KGTR) {
                    ds.ReadData(ref translation);
                } else if(tag == (uint)TransformTags.KGRT) {
                    ds.ReadData(ref rotation);
                } else if(tag == (uint)TransformTags.KGSC) {
                    ds.ReadData(ref scaling);
                } else {
                    throw new ParsingException();
                }
            }
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            if(name is null)
                throw new NameCantBeEmptyException();

            ds.WriteStructArray(name, false);

            ds.WriteStruct(objectId);
            ds.WriteStruct(parentId);
            ds.WriteStruct(flags);

            if(translation.HasData) {
                ds.WriteStruct(TransformTags.KGTR);
                ds.WriteData(ref translation);
            }

            if(rotation.HasData) {
                ds.WriteStruct(TransformTags.KGRT);
                ds.WriteData(ref rotation);
            }

            if(scaling.HasData) {
                ds.WriteStruct(TransformTags.KGSC);
                ds.WriteData(ref scaling);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }
    }
}
