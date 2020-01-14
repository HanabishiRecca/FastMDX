using System.Runtime.InteropServices;

namespace FastMDX {
    public unsafe struct RibbonEmitter : IDataRW {
        public Node node;
        public REProps properties;
        public Transform<float> visibilityTransform, heightAboveTransform, heightBelowTransform, alphaTransform, colorTransform, textureSlotTransform;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadData(ref node);
            ds.ReadStruct(ref properties);

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)Tags.KRVS) {
                    ds.ReadData(ref visibilityTransform);
                } else if(tag == (uint)Tags.KRHA) {
                    ds.ReadData(ref heightAboveTransform);
                } else if(tag == (uint)Tags.KRHB) {
                    ds.ReadData(ref heightBelowTransform);
                } else if(tag == (uint)Tags.KRAL) {
                    ds.ReadData(ref alphaTransform);
                } else if(tag == (uint)Tags.KRCO) {
                    ds.ReadData(ref colorTransform);
                } else if(tag == (uint)Tags.KRTX) {
                    ds.ReadData(ref textureSlotTransform);
                } else {
                    throw new ParsingException();
                }
            }
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteData(ref node);
            ds.WriteStruct(ref properties);

            if(visibilityTransform.HasData) {
                ds.WriteStruct(Tags.KRVS);
                ds.WriteData(ref visibilityTransform);
            }

            if(heightAboveTransform.HasData) {
                ds.WriteStruct(Tags.KRHA);
                ds.WriteData(ref heightAboveTransform);
            }

            if(heightBelowTransform.HasData) {
                ds.WriteStruct(Tags.KRHB);
                ds.WriteData(ref heightBelowTransform);
            }

            if(alphaTransform.HasData) {
                ds.WriteStruct(Tags.KRAL);
                ds.WriteData(ref alphaTransform);
            }

            if(colorTransform.HasData) {
                ds.WriteStruct(Tags.KRCO);
                ds.WriteData(ref colorTransform);
            }

            if(textureSlotTransform.HasData) {
                ds.WriteStruct(Tags.KRTX);
                ds.WriteData(ref textureSlotTransform);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }

        enum Tags : uint {
            KRVS = 0x5356524Bu,
            KRHA = 0x4148524Bu,
            KRHB = 0x4248524Bu,
            KRAL = 0x4C41524Bu,
            KRCO = 0x4F43524Bu,
            KRTX = 0x5854524Bu,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct REProps {
            public float heightAbove, heightBelow, alpha;
            public Color color;
            public float lifespan;
            public uint textureSlot, emissionRate, rows, columns, materialId;
            public float gravity;
        }
    }
}
