namespace FastMDX {
    public struct Helper : IDataRW {
        public Node node;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.ReadData(ref node);
        }

        void IDataRW.WriteTo(DataStream ds) {
            ds.WriteData(ref node);
        }
    }
}
