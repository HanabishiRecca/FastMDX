namespace FastMDX {
    class LightsParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Lights = ds.ReadDataArrayUnknownCount<Light>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.Lights, false);
        }

        public bool HasData(MDX mdx) => mdx?.Lights?.Length > 0;
    }
}
