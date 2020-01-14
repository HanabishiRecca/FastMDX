namespace FastMDX {
    class EventObjectsParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.EventObjects = ds.ReadDataArrayUnknownCount<EventObject>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.EventObjects, false);
        }

        public bool HasData(MDX mdx) => mdx?.EventObjects?.Length > 0;
    }
}
