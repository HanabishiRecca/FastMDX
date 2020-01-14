namespace FastMDX {
    class GlobalSequencesParser : IBlockParser {
        public unsafe void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.GlobalSequences = ds.ReadStructArray<GlobalSequence>(blockSize / (uint)sizeof(GlobalSequence));
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteStructArray(mdx.GlobalSequences, false);
        }

        public bool HasData(MDX mdx) => mdx?.GlobalSequences?.Length > 0;
    }
}
