namespace FastMDX {
    class PivotsParser : IBlockParser {
        public unsafe void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Pivots = ds.ReadStructArray<Vec3>(blockSize / (uint)sizeof(Vec3));
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteStructArray(mdx.Pivots, false);
        }

        public bool HasData(MDX mdx) => mdx?.Pivots?.Length > 0;
    }
}
