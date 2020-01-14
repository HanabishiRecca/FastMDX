namespace FastMDX {
    public struct EventObject : IDataRW {
        public Node node;
        public uint globalSequenceId;
        public uint[] tracks;

        const uint KEVT_HEADER = 0x5456454Bu;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.ReadData(ref node);
            ds.CheckTag(KEVT_HEADER);
            var tracksCount = ds.ReadStruct<uint>();
            ds.ReadStruct(ref globalSequenceId);
            tracks = ds.ReadStructArray<uint>(tracksCount);
        }

        void IDataRW.WriteTo(DataStream ds) {
            ds.WriteData(ref node);
            ds.WriteStruct(KEVT_HEADER);
            ds.WriteStruct((uint)tracks.Length);
            ds.WriteStruct(globalSequenceId);
            ds.WriteStructArray(tracks, false);
        }
    }
}
