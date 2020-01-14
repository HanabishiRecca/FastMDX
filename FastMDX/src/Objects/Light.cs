namespace FastMDX {
    public unsafe struct Light : IDataRW {
        public Node node;
        public uint type;
        public float attenuationStart, attenuationEnd, intensity, ambientIntensity;
        public Color color, ambientColor;
        public Transform<uint> attenuationStartTransform, attenuationEndTransform;
        public Transform<Color> colorTransform, ambientColorTransform;
        public Transform<float> intensityTransform, ambientIntensityTransform, visibilityTransform;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadData(ref node);

            ds.ReadStruct(ref type);
            ds.ReadStruct(ref attenuationStart);
            ds.ReadStruct(ref attenuationEnd);
            ds.ReadStruct(ref color);
            ds.ReadStruct(ref intensity);
            ds.ReadStruct(ref ambientColor);

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)Tags.KLAS) {
                    ds.ReadData(ref attenuationStartTransform);
                } else if(tag == (uint)Tags.KLAE) {
                    ds.ReadData(ref attenuationEndTransform);
                } else if(tag == (uint)Tags.KLAC) {
                    ds.ReadData(ref colorTransform);
                } else if(tag == (uint)Tags.KLAI) {
                    ds.ReadData(ref intensityTransform);
                } else if(tag == (uint)Tags.KLBI) {
                    ds.ReadData(ref ambientIntensityTransform);
                } else if(tag == (uint)Tags.KLBC) {
                    ds.ReadData(ref ambientColorTransform);
                } else if(tag == (uint)Tags.KLAV) {
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

            ds.WriteStruct(type);
            ds.WriteStruct(attenuationStart);
            ds.WriteStruct(attenuationEnd);
            ds.WriteStruct(color);
            ds.WriteStruct(intensity);
            ds.WriteStruct(ambientColor);

            if(attenuationStartTransform.HasData) {
                ds.WriteStruct(Tags.KLAS);
                ds.WriteData(ref attenuationStartTransform);
            }

            if(attenuationEndTransform.HasData) {
                ds.WriteStruct(Tags.KLAE);
                ds.WriteData(ref attenuationEndTransform);
            }

            if(colorTransform.HasData) {
                ds.WriteStruct(Tags.KLAC);
                ds.WriteData(ref colorTransform);
            }

            if(intensityTransform.HasData) {
                ds.WriteStruct(Tags.KLAI);
                ds.WriteData(ref intensityTransform);
            }

            if(ambientIntensityTransform.HasData) {
                ds.WriteStruct(Tags.KLBI);
                ds.WriteData(ref ambientIntensityTransform);
            }

            if(ambientColorTransform.HasData) {
                ds.WriteStruct(Tags.KLBC);
                ds.WriteData(ref ambientColorTransform);
            }

            if(visibilityTransform.HasData) {
                ds.WriteStruct(Tags.KLAV);
                ds.WriteData(ref visibilityTransform);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        enum Tags : uint {
            KLAS = 0x53414C4Bu,
            KLAE = 0x45414C4Bu,
            KLAC = 0x43414C4Bu,
            KLAI = 0x49414C4Bu,
            KLBI = 0x49424C4Bu,
            KLBC = 0x43424C4Bu,
            KLAV = 0x56414C4Bu,
        }
    }
}
