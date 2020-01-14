namespace FastMDX {
    class MaterialsParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Materials = ds.ReadDataArrayUnknownCount<Material>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.Materials, false);
        }

        public bool HasData(MDX mdx) => mdx?.Materials?.Length > 0;
    }
}
