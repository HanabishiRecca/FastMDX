namespace FastMDX {
    public struct Layer : IDataRW {
        public uint filterMode, shadingFlags, textureId, textureAnimationId, coordId;
        public float alpha;
        public Transform<uint> materialTextureId;
        public Transform<float> materialAlpha;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();

            ds.ReadStruct(ref filterMode);
            ds.ReadStruct(ref shadingFlags);
            ds.ReadStruct(ref textureId);
            ds.ReadStruct(ref textureAnimationId);
            ds.ReadStruct(ref coordId);
            ds.ReadStruct(ref alpha);

            while(ds.Offset < end) {
                var tag = ds.ReadStruct<uint>();
                if(tag == (uint)TransformTags.KMTF) {
                    ds.ReadData(ref materialTextureId);
                } else if(tag == (uint)TransformTags.KMTA) {
                    ds.ReadData(ref materialAlpha);
                } else {
                    throw new ParsingException();
                }
            }
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteStruct(filterMode);
            ds.WriteStruct(shadingFlags);
            ds.WriteStruct(textureId);
            ds.WriteStruct(textureAnimationId);
            ds.WriteStruct(coordId);
            ds.WriteStruct(alpha);

            if(materialTextureId.HasData) {
                ds.WriteStruct(TransformTags.KMTF);
                ds.WriteData(ref materialTextureId);
            }

            if(materialAlpha.HasData) {
                ds.WriteStruct(TransformTags.KMTA);
                ds.WriteData(ref materialAlpha);
            }

            ds.SetValueAt(offset, ds.Offset - offset);
        }
    }
}
