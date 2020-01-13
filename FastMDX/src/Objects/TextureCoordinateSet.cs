namespace FastMDX {
    public struct TextureCoordinateSet : IDataRW {
        public Vec2[] textureCoordinates;

        const uint UVBS_HEADER = 0x53425655u;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.CheckTag(UVBS_HEADER);
            textureCoordinates = ds.ReadStructArray<Vec2>();
        }

        void IDataRW.WriteTo(DataStream ds) {
            ds.WriteStruct(UVBS_HEADER);
            ds.WriteStructArray(textureCoordinates);
        }
    }
}
