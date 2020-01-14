namespace FastMDX {
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

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)Tags.KCTR) {
                    ds.ReadData(ref translation);
                } else if(tag == (uint)Tags.KCRL) {
                    ds.ReadData(ref rotation);
                } else if(tag == (uint)Tags.KTTR) {
                    ds.ReadData(ref targetTranslation);
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

            ds.WriteStruct(position);
            ds.WriteStruct(filedOfView);
            ds.WriteStruct(farClippingPlane);
            ds.WriteStruct(nearClippingPlane);
            ds.WriteStruct(targetPosition);

            if(translation.HasData) {
                ds.WriteStruct(Tags.KCTR);
                ds.WriteData(ref translation);
            }

            if(rotation.HasData) {
                ds.WriteStruct(Tags.KCRL);
                ds.WriteData(ref rotation);
            }

            if(targetTranslation.HasData) {
                ds.WriteStruct(Tags.KTTR);
                ds.WriteData(ref targetTranslation);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        enum Tags : uint {
            KCTR = 0x5254434Bu,
            KCRL = 0x4C52434Bu,
            KTTR = 0x5254544Bu,
        }
    }
}
