namespace FastMDX {
    using static InnerBlocks;

    public struct Material : IDataRW {
        public uint priorityPlane, flags;
        public Layer[] layers;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.Skip(sizeof(uint));

            ds.ReadStruct(ref priorityPlane);
            ds.ReadStruct(ref flags);

            ds.CheckTag(LAYS);
            layers = ds.ReadDataArray<Layer>();
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteStruct(priorityPlane);
            ds.WriteStruct(flags);

            ds.WriteStruct(LAYS);
            ds.WriteDataArray(layers);

            ds.SetValueAt(offset, ds.Offset - offset);
        }
    }
}
