namespace FastMDX {
    class AttachmentsParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.Attachments = ds.ReadDataArrayUnknownCount<Attachment>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.Attachments, false);
        }

        public bool HasData(MDX mdx) => mdx?.Attachments?.Length > 0;
    }
}
