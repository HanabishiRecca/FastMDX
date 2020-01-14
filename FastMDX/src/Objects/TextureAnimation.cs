﻿namespace FastMDX {
    public unsafe struct TextureAnimation : IDataRW {
        public Transform<Vec3> translation, scaling;
        public Transform<Vec4> rotation;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();
            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)Tags.KTAT) {
                    ds.ReadData(ref translation);
                } else if(tag == (uint)Tags.KTAR) {
                    ds.ReadData(ref rotation);
                } else if(tag == (uint)Tags.KTAS) {
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
                ds.WriteStruct(Tags.KTAT);
                ds.WriteData(ref translation);
            }

            if(rotation.HasData) {
                ds.WriteStruct(Tags.KTAR);
                ds.WriteData(ref rotation);
            }

            if(scaling.HasData) {
                ds.WriteStruct(Tags.KTAS);
                ds.WriteData(ref scaling);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        enum Tags : uint {
            KTAT = 0x5441544Bu,
            KTAR = 0x5241544Bu,
            KTAS = 0x5341544Bu,
        }
    }
}