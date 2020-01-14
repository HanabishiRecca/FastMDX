namespace FastMDX {
    public struct CollisionShape : IDataRW {
        public Node node;
        public uint type;
        public Vec3 vertices1, vertices2;
        public float radius;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.ReadData(ref node);
            ds.ReadStruct(ref type);

            if(type > 3)
                throw new ParsingException();

            ds.ReadStruct(ref vertices1);

            if(type != 2)
                ds.ReadStruct(ref vertices2);

            if(type > 1)
                ds.ReadStruct(ref radius);
        }

        void IDataRW.WriteTo(DataStream ds) {
            if(type > 3)
                throw new ParsingException();

            ds.WriteData(ref node);
            ds.WriteStruct(type);
            ds.WriteStruct(ref vertices1);

            if(type != 2)
                ds.WriteStruct(ref vertices2);

            if(type > 1)
                ds.WriteStruct(radius);
        }
    }
}
