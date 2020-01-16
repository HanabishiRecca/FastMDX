namespace FastMDX {
    using static InnerBlocks;

    public struct TextureCoordinateSet : IDataRW {
        public Vec2[] textureCoordinates;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.CheckTag(UVBS);
            textureCoordinates = ds.ReadStructArray<Vec2>();
        }

        void IDataRW.WriteTo(DataStream ds) {
            ds.WriteStruct(UVBS);
            ds.WriteStructArray(textureCoordinates);
        }
    }
}
