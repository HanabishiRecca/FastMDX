namespace FastMDX {
    class GeosetAnimationsParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.GeosetAnimations = ds.ReadDataArrayUnknownCount<GeosetAnimation>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.GeosetAnimations, false);
        }

        public bool HasData(MDX mdx) => mdx?.GeosetAnimations?.Length > 0;
    }
}
