namespace FastMDX {
    class RibbonEmittersParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.RibbonEmitters = ds.ReadDataArrayUnknownCount<RibbonEmitter>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.RibbonEmitters, false);
        }

        public bool HasData(MDX mdx) => mdx?.RibbonEmitters?.Length > 0;
    }
}
