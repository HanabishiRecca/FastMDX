namespace FastMDX {
    class HelpersParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Helpers = ds.ReadDataArrayUnknownCount<Helper>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.Helpers, false);
        }

        public bool HasData(MDX mdx) => mdx?.Helpers?.Length > 0;
    }
}
