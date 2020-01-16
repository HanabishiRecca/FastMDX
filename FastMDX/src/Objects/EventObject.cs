namespace FastMDX {
    using static InnerBlocks;

    public struct EventObject : IDataRW {
        public Node node;
        public uint globalSequenceId;
        public uint[] tracks;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.ReadData(ref node);
            ds.CheckTag(KEVT);
            var tracksCount = ds.ReadStruct<uint>();
            ds.ReadStruct(ref globalSequenceId);
            tracks = ds.ReadStructArray<uint>(tracksCount);
        }

        void IDataRW.WriteTo(DataStream ds) {
            ds.WriteData(ref node);
            ds.WriteStruct(KEVT);
            ds.WriteStruct((uint)tracks.Length);
            ds.WriteStruct(globalSequenceId);
            ds.WriteStructArray(tracks, false);
        }
    }
}
