namespace FastMDX {
    public struct Material : IDataRW {
        public uint priorityPlane, flags;
        public Layer[] layers;

        const uint LAYS_HEADER = 0x5359414Cu;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.Skip(sizeof(uint));

            ds.ReadStruct(ref priorityPlane);
            ds.ReadStruct(ref flags);

            ds.CheckTag(LAYS_HEADER);
            layers = ds.ReadDataArray<Layer>();
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteStruct(priorityPlane);
            ds.WriteStruct(flags);

            ds.WriteStruct(LAYS_HEADER);
            ds.WriteDataArray(layers);

            ds.SetValueAt(offset, ds.Offset - offset);
        }
    }
}
