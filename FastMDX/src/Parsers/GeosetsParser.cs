namespace MDXLib {
    class GeosetsParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Geosets = ds.ReadDataArrayUnknownCount<Geoset>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds, uint tag) {
            if(mdx.Geosets?.Length < 1)
                return;

            ds.WriteStruct(tag);

            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteDataArray(mdx.Geosets, false);
            ds.SetValueAt(offset, ds.Offset - (offset + sizeof(uint)));
        }
    }
}
