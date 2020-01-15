namespace FastMDX {
    public unsafe struct ParticleEmitter : IDataRW {
        public Node node;
        public float emissionRate, gravity, longitude, latitude, lifespan, speed;
        fixed byte name[(int)PATH_LEN];
        public Transform<float> emissionRateTransform, gravityTransform, longitudeTransform, latitudeTransform, lifespanTransform, speedTransform, visibilityTransform;

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

            ds.ReadStruct(ref emissionRate);
            ds.ReadStruct(ref gravity);
            ds.ReadStruct(ref longitude);
            ds.ReadStruct(ref latitude);

            fixed(byte* n = name)
                ds.ReadUnmanagedArray(n, PATH_LEN);

            ds.ReadStruct(ref lifespan);
            ds.ReadStruct(ref speed);

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)Tags.KPEE) {
                    ds.ReadData(ref emissionRateTransform);
                } else if(tag == (uint)Tags.KPEG) {
                    ds.ReadData(ref gravityTransform);
                } else if(tag == (uint)Tags.KPLN) {
                    ds.ReadData(ref longitudeTransform);
                } else if(tag == (uint)Tags.KPLT) {
                    ds.ReadData(ref latitudeTransform);
                } else if(tag == (uint)Tags.KPEL) {
                    ds.ReadData(ref lifespanTransform);
                } else if(tag == (uint)Tags.KPES) {
                    ds.ReadData(ref speedTransform);
                } else if(tag == (uint)Tags.KPEV) {
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

            ds.WriteStruct(emissionRate);
            ds.WriteStruct(gravity);
            ds.WriteStruct(longitude);
            ds.WriteStruct(latitude);

            fixed(byte* n = name)
                ds.WriteUnmanagedArray(n, PATH_LEN);

            ds.WriteStruct(lifespan);
            ds.WriteStruct(speed);

            if(emissionRateTransform.HasData) {
                ds.WriteStruct(Tags.KPEE);
                ds.WriteData(ref emissionRateTransform);
            }

            if(gravityTransform.HasData) {
                ds.WriteStruct(Tags.KPEG);
                ds.WriteData(ref gravityTransform);
            }

            if(longitudeTransform.HasData) {
                ds.WriteStruct(Tags.KPLN);
                ds.WriteData(ref longitudeTransform);
            }

            if(latitudeTransform.HasData) {
                ds.WriteStruct(Tags.KPLT);
                ds.WriteData(ref latitudeTransform);
            }

            if(lifespanTransform.HasData) {
                ds.WriteStruct(Tags.KPEL);
                ds.WriteData(ref lifespanTransform);
            }

            if(speedTransform.HasData) {
                ds.WriteStruct(Tags.KPES);
                ds.WriteData(ref speedTransform);
            }

            if(visibilityTransform.HasData) {
                ds.WriteStruct(Tags.KPEV);
                ds.WriteData(ref visibilityTransform);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        enum Tags : uint {
            KPEE = 0x4545504Bu,
            KPEG = 0x4745504Bu,
            KPLN = 0x4E4C504Bu,
            KPLT = 0x544C504Bu,
            KPEL = 0x4C45504Bu,
            KPES = 0x5345504Bu,
            KPEV = 0x5645504Bu,
        }
    }
}
