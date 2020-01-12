namespace MDXLib {
    class BonesParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Bones = ds.ReadDataArrayUnknownCount<Bone>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds, uint tag) {
            if(mdx.Bones?.Length < 1)
                return;

            ds.WriteStruct(tag);

            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteDataArray(mdx.Bones, false);
            ds.SetValueAt(offset, ds.Offset - (offset + sizeof(uint)));
        }
    }
}
