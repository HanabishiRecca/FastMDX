namespace MDXLib {
    class BonesParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Bones = ds.ReadDataArrayUnknownCount<Bone>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.Bones, false);
        }

        public bool HasData(MDX mdx) => mdx?.Bones?.Length > 0;
    }
}
