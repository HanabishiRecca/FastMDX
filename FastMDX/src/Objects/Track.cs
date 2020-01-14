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

    enum TransformTags : uint {
        KGTR = 0x5254474Bu,
        KGRT = 0x5452474Bu,
        KGSC = 0x4353474Bu,
        KMTF = 0x46544D4Bu,
        KMTA = 0x41544D4Bu,
        KTAT = 0x5441544Bu,
        KTAR = 0x5241544Bu,
        KTAS = 0x5341544Bu,
        KGAO = 0x4F41474Bu,
        KGAC = 0x4341474Bu,
    }
}
