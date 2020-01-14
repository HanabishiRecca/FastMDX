namespace FastMDX {
    class CollisionShapesParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.CollisionShapes = ds.ReadDataArrayUnknownCount<CollisionShape>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.CollisionShapes, false);
        }

        public bool HasData(MDX mdx) => mdx?.CollisionShapes?.Length > 0;
    }
}
