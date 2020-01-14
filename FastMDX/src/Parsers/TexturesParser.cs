namespace FastMDX {
    class TexturesParser : IBlockParser {
        public unsafe void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Textures = ds.ReadStructArray<Texture>(blockSize / (uint)sizeof(Texture));
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteStructArray(mdx.Textures, false);
        }

        public bool HasData(MDX mdx) => mdx?.Textures?.Length > 0;
    }
}
