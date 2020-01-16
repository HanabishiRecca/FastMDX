namespace FastMDX {
    using static InnerBlocks;

    public struct Geoset : IDataRW {
        public Vec3[] vertexPositions, vertexNormals;
        public uint[] faceTypeGroups, faceGroups, matrixGroups, matrixIndices;
        public ushort[] faces;
        public byte[] vertexGroups;
        public uint materialId, selectionGroup, selectionFlags;
        public Extent extent;
        public Extent[] sequenceExtents;
        public TextureCoordinateSet[] textureCoordinateSets;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.Skip(sizeof(uint));

            ds.CheckTag(VRTX);
            vertexPositions = ds.ReadStructArray<Vec3>();

            ds.CheckTag(NRMS);
            vertexNormals = ds.ReadStructArray<Vec3>();

            ds.CheckTag(PTYP);
            faceTypeGroups = ds.ReadStructArray<uint>();

            ds.CheckTag(PCNT);
            faceGroups = ds.ReadStructArray<uint>();

            ds.CheckTag(PVTX);
            faces = ds.ReadStructArray<ushort>();

            ds.CheckTag(GNDX);
            vertexGroups = ds.ReadStructArray<byte>();

            ds.CheckTag(MTGC);
            matrixGroups = ds.ReadStructArray<uint>();

            ds.CheckTag(MATS);
            matrixIndices = ds.ReadStructArray<uint>();

            ds.ReadStruct(ref materialId);
            ds.ReadStruct(ref selectionGroup);
            ds.ReadStruct(ref selectionFlags);
            ds.ReadStruct(ref extent);

            sequenceExtents = ds.ReadStructArray<Extent>();

            ds.CheckTag(UVAS);
            textureCoordinateSets = ds.ReadDataArray<TextureCoordinateSet>();
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteStruct(VRTX);
            ds.WriteStructArray(vertexPositions);

            ds.WriteStruct(NRMS);
            ds.WriteStructArray(vertexNormals);

            ds.WriteStruct(PTYP);
            ds.WriteStructArray(faceTypeGroups);

            ds.WriteStruct(PCNT);
            ds.WriteStructArray(faceGroups);

            ds.WriteStruct(PVTX);
            ds.WriteStructArray(faces);

            ds.WriteStruct(GNDX);
            ds.WriteStructArray(vertexGroups);

            ds.WriteStruct(MTGC);
            ds.WriteStructArray(matrixGroups);

            ds.WriteStruct(MATS);
            ds.WriteStructArray(matrixIndices);

            ds.WriteStruct(materialId);
            ds.WriteStruct(selectionGroup);
            ds.WriteStruct(selectionFlags);
            ds.WriteStruct(extent);

            ds.WriteStructArray(sequenceExtents);

            ds.WriteStruct(UVAS);
            ds.WriteDataArray(textureCoordinateSets);

            ds.SetValueAt(offset, ds.Offset - offset);
        }
    }
}
