namespace FastMDX {
    public struct Transform<T> : IDataRW where T : unmanaged {
        public uint interpolationType;
        public int globalSequenceId;
        public Track<T>[] tracks;
        public TrackInter<T>[] tracksInter;

        void IDataRW.ReadFrom(DataStream ds) {
            var tracksCount = ds.ReadStruct<uint>();

            ds.ReadStruct(ref interpolationType);
            ds.ReadStruct(ref globalSequenceId);

            if(interpolationType > 1)
                tracksInter = ds.ReadStructArray<TrackInter<T>>(tracksCount);
            else
                tracks = ds.ReadStructArray<Track<T>>(tracksCount);
        }

        void IDataRW.WriteTo(DataStream ds) {
            if(interpolationType > 1)
                ds.WriteStruct((uint)tracksInter.Length);
            else
                ds.WriteStruct((uint)tracks.Length);

            ds.WriteStruct(interpolationType);
            ds.WriteStruct(globalSequenceId);

            if(interpolationType > 1)
                ds.WriteStructArray(tracksInter, false);
            else
                ds.WriteStructArray(tracks, false);
        }

        internal bool HasData => (interpolationType > 1) ? (tracksInter?.Length > 0) : (tracks?.Length > 0);
    }
}
