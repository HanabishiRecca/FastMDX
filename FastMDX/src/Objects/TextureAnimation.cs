namespace FastMDX {
    public unsafe struct TextureAnimation : IDataRW {
        public Transform<Vec3> translation, scaling;
        public Transform<Vec4> rotation;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();
            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)TransformTags.KTAT) {
                    ds.ReadData(ref translation);
                } else if(tag == (uint)TransformTags.KTAR) {
                    ds.ReadData(ref rotation);
                } else if(tag == (uint)TransformTags.KTAS) {
                    ds.ReadData(ref scaling);
                } else {
                    throw new ParsingException();
                }
            }
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            if(translation.HasData) {
                ds.WriteStruct(TransformTags.KTAT);
                ds.WriteData(ref translation);
            }

            if(rotation.HasData) {
                ds.WriteStruct(TransformTags.KTAR);
                ds.WriteData(ref rotation);
            }

            if(scaling.HasData) {
                ds.WriteStruct(TransformTags.KTAS);
                ds.WriteData(ref scaling);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }
    }
}
