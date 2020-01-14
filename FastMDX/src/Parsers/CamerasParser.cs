namespace FastMDX {
    class CamerasParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Cameras = ds.ReadDataArrayUnknownCount<Camera>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.Cameras, false);
        }

        public bool HasData(MDX mdx) => mdx?.Cameras?.Length > 0;
    }
}
