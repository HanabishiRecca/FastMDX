namespace FastMDX {
    class SequencesParser : IBlockParser {
        public unsafe void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Sequences = ds.ReadStructArray<Sequence>(blockSize / (uint)sizeof(Sequence));
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteStructArray(mdx.Sequences, false);
        }

        public bool HasData(MDX mdx) => mdx?.Sequences?.Length > 0;
    }
}
