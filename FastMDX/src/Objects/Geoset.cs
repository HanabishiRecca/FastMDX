namespace FastMDX {
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

            ds.CheckTag((uint)GeosetInnerTags.VRTX);
            vertexPositions = ds.ReadStructArray<Vec3>();

            ds.CheckTag((uint)GeosetInnerTags.NRMS);
            vertexNormals = ds.ReadStructArray<Vec3>();

            ds.CheckTag((uint)GeosetInnerTags.PTYP);
            faceTypeGroups = ds.ReadStructArray<uint>();

            ds.CheckTag((uint)GeosetInnerTags.PCNT);
            faceGroups = ds.ReadStructArray<uint>();

            ds.CheckTag((uint)GeosetInnerTags.PVTX);
            faces = ds.ReadStructArray<ushort>();

            ds.CheckTag((uint)GeosetInnerTags.GNDX);
            vertexGroups = ds.ReadStructArray<byte>();

            ds.CheckTag((uint)GeosetInnerTags.MTGC);
            matrixGroups = ds.ReadStructArray<uint>();

            ds.CheckTag((uint)GeosetInnerTags.MATS);
            matrixIndices = ds.ReadStructArray<uint>();

            ds.ReadStruct(ref materialId);
            ds.ReadStruct(ref selectionGroup);
            ds.ReadStruct(ref selectionFlags);
            ds.ReadStruct(ref extent);

            sequenceExtents = ds.ReadStructArray<Extent>();

            ds.CheckTag((uint)GeosetInnerTags.UVAS);
            textureCoordinateSets = ds.ReadDataArray<TextureCoordinateSet>();
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));

            ds.WriteStruct(GeosetInnerTags.VRTX);
            ds.WriteStructArray(vertexPositions);

            ds.WriteStruct(GeosetInnerTags.NRMS);
            ds.WriteStructArray(vertexNormals);

            ds.WriteStruct(GeosetInnerTags.PTYP);
            ds.WriteStructArray(faceTypeGroups);

            ds.WriteStruct(GeosetInnerTags.PCNT);
            ds.WriteStructArray(faceGroups);

            ds.WriteStruct(GeosetInnerTags.PVTX);
            ds.WriteStructArray(faces);

            ds.WriteStruct(GeosetInnerTags.GNDX);
            ds.WriteStructArray(vertexGroups);

            ds.WriteStruct(GeosetInnerTags.MTGC);
            ds.WriteStructArray(matrixGroups);

            ds.WriteStruct(GeosetInnerTags.MATS);
            ds.WriteStructArray(matrixIndices);

            ds.WriteStruct(materialId);
            ds.WriteStruct(selectionGroup);
            ds.WriteStruct(selectionFlags);
            ds.WriteStruct(extent);

            ds.WriteStructArray(sequenceExtents);

            ds.WriteStruct(GeosetInnerTags.UVAS);
            ds.WriteDataArray(textureCoordinateSets);

            ds.SetValueAt(offset, ds.Offset - offset);
        }
    }

    enum GeosetInnerTags : uint {
        VRTX = 0x58545256u,
        NRMS = 0x534D524Eu,
        PTYP = 0x50595450u,
        PCNT = 0x544E4350u,
        PVTX = 0x58545650u,
        GNDX = 0x58444E47u,
        MTGC = 0x4347544Du,
        MATS = 0x5354414Du,
        UVAS = 0x53415655u,
    }
}
