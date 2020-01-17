namespace FastMDX {
    public struct CollisionShape : IDataRW {
        public Node Node;
        public uint Type;
        public Vec3 Vertices1, Vertices2;
        public float Radius;

        void IDataRW.ReadFrom(DataStream ds) {
            ds.ReadData(ref Node);
            ds.ReadStruct(ref Type);

            if(Type > 3)
                throw new ParsingException();

            ds.ReadStruct(ref Vertices1);

            if(Type != 2)
                ds.ReadStruct(ref Vertices2);

            if(Type > 1)
                ds.ReadStruct(ref Radius);
        }

        void IDataRW.WriteTo(DataStream ds) {
            if(Type > 3)
                throw new ParsingException();

            ds.WriteData(ref Node);
            ds.WriteStruct(Type);
            ds.WriteStruct(ref Vertices1);

            if(Type != 2)
                ds.WriteStruct(ref Vertices2);

            if(Type > 1)
                ds.WriteStruct(Radius);
        }
    }
}
