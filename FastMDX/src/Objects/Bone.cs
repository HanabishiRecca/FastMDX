namespace FastMDX {
    public struct Bone : IDataRW {
        public Node node;
        public uint geosetId, geosetAnimationId;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.ReadData(ref node);
            ds.ReadStruct(ref geosetId);
            ds.ReadStruct(ref geosetAnimationId);
        }

        void IDataRW.WriteTo(DataStream ds) {
            ds.WriteData(ref node);
            ds.WriteStruct(geosetId);
            ds.WriteStruct(geosetAnimationId);
        }
    }
}
