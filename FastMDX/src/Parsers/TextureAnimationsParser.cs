namespace FastMDX {
    class TextureAnimationsParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.TextureAnimations = ds.ReadDataArrayUnknownCount<TextureAnimation>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.TextureAnimations, false);
        }

        public bool HasData(MDX mdx) => mdx?.TextureAnimations?.Length > 0;
    }
}
