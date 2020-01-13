namespace MDXLib {
    class GeosetsParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Geosets = ds.ReadDataArrayUnknownCount<Geoset>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.Geosets, false);
        }

        public bool HasData(MDX mdx) => mdx?.Geosets?.Length > 0;
    }
}
