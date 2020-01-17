using System.Runtime.InteropServices;

namespace FastMDX {
    public struct Transform<T> : IDataRW, IOptionalBlock where T : unmanaged {
        public LocalProperties Properties;
        public Track<T>[] Tracks;
        public TrackInter<T>[] TracksInter;

        void IDataRW.ReadFrom(DataStream ds) {
            var tracksCount = ds.ReadStruct<uint>();
            ds.ReadStruct(ref Properties);

            if(Properties.InterpolationType > 1)
                TracksInter = ds.ReadStructArray<TrackInter<T>>(tracksCount);
            else
                Tracks = ds.ReadStructArray<Track<T>>(tracksCount);
        }

        void IDataRW.WriteTo(DataStream ds) {
            if(Properties.InterpolationType > 1)
                ds.WriteStruct((uint)TracksInter.Length);
            else
                ds.WriteStruct((uint)Tracks.Length);

            ds.WriteStruct(ref Properties);

            if(Properties.InterpolationType > 1)
                ds.WriteStructArray(TracksInter, false);
            else
                ds.WriteStructArray(Tracks, false);
        }

        bool IOptionalBlock.HasData => (Properties.InterpolationType > 1) ? (TracksInter?.Length > 0) : (Tracks?.Length > 0);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LocalProperties {
            public uint InterpolationType;
            public int GlobalSequenceId;
        }
    }
}
