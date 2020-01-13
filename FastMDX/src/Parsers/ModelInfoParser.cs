namespace FastMDX {
    class ModelInfoParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            ds.ReadStruct(ref mdx.Info);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteStruct(mdx.Info);
        }

        public bool HasData(MDX mdx) => true;
    }
}
