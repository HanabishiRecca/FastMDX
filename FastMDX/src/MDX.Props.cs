namespace FastMDX {
    using Parsers = System.Collections.Generic.Dictionary<uint, IBlockParser>;

    public partial class MDX {
        public ModelInfo Info;
        public Sequence[] Sequences;
        public GlobalSequence[] GlobalSequences;
        public Material[] Materials;
        public Texture[] Textures;
        public TextureAnimation[] TextureAnimations;
        public Geoset[] Geosets;
        public GeosetAnimation[] GeosetAnimations;
        public Bone[] Bones;
        public Light[] Lights;
        public Helper[] Helpers;
        public Attachment[] Attachments;
        public Vec3[] Pivots;
        public ParticleEmitter[] ParticleEmitters;
        public ParticleEmitter2[] ParticleEmitters2;
        public RibbonEmitter[] RibbonEmitters;
        public EventObject[] EventObjects;
        public Camera[] Cameras;
        public CollisionShape[] CollisionShapes;

        static readonly Parsers _knownParsers = new Parsers {
            [(uint)KnownBlocks.MODL] = new StructParser<ModelInfo>(mdx => ref mdx.Info),
            [(uint)KnownBlocks.SEQS] = new StructArrayParser<Sequence>(mdx => ref mdx.Sequences),
            [(uint)KnownBlocks.GLBS] = new StructArrayParser<GlobalSequence>(mdx => ref mdx.GlobalSequences),
            [(uint)KnownBlocks.MTLS] = new DataArrayParser<Material>(mdx => ref mdx.Materials),
            [(uint)KnownBlocks.TEXS] = new StructArrayParser<Texture>(mdx => ref mdx.Textures),
            [(uint)KnownBlocks.TXAN] = new DataArrayParser<TextureAnimation>(mdx => ref mdx.TextureAnimations),
            [(uint)KnownBlocks.GEOS] = new DataArrayParser<Geoset>(mdx => ref mdx.Geosets),
            [(uint)KnownBlocks.GEOA] = new DataArrayParser<GeosetAnimation>(mdx => ref mdx.GeosetAnimations),
            [(uint)KnownBlocks.BONE] = new DataArrayParser<Bone>(mdx => ref mdx.Bones),
            [(uint)KnownBlocks.LITE] = new DataArrayParser<Light>(mdx => ref mdx.Lights),
            [(uint)KnownBlocks.HELP] = new DataArrayParser<Helper>(mdx => ref mdx.Helpers),
            [(uint)KnownBlocks.ATCH] = new DataArrayParser<Attachment>(mdx => ref mdx.Attachments),
            [(uint)KnownBlocks.PIVT] = new StructArrayParser<Vec3>(mdx => ref mdx.Pivots),
            [(uint)KnownBlocks.PREM] = new DataArrayParser<ParticleEmitter>(mdx => ref mdx.ParticleEmitters),
            [(uint)KnownBlocks.PRE2] = new DataArrayParser<ParticleEmitter2>(mdx => ref mdx.ParticleEmitters2),
            [(uint)KnownBlocks.RIBB] = new DataArrayParser<RibbonEmitter>(mdx => ref mdx.RibbonEmitters),
            [(uint)KnownBlocks.EVTS] = new DataArrayParser<EventObject>(mdx => ref mdx.EventObjects),
            [(uint)KnownBlocks.CAMS] = new DataArrayParser<Camera>(mdx => ref mdx.Cameras),
            [(uint)KnownBlocks.CLID] = new DataArrayParser<CollisionShape>(mdx => ref mdx.CollisionShapes),
        };
    }

    interface IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize);
        public void WriteTo(MDX mdx, DataStream ds);
        public bool HasData(MDX mdx);
    }

    enum KnownBlocks : uint {
        MODL = 0x4C444F4Du,
        SEQS = 0x53514553u,
        GLBS = 0x53424C47u,
        TEXS = 0x53584554u,
        SNDS = 0x53444E53u,
        MTLS = 0x534C544Du,
        TXAN = 0x4E415854u,
        GEOS = 0x534F4547u,
        GEOA = 0x414F4547u,
        BONE = 0x454E4F42u,
        LITE = 0x4554494Cu,
        HELP = 0x504C4548u,
        ATCH = 0x48435441u,
        PIVT = 0x54564950u,
        PREM = 0x4D455250u,
        PRE2 = 0x32455250u,
        RIBB = 0x42424952u,
        EVTS = 0x53545645u,
        CAMS = 0x534D4143u,
        CLID = 0x44494C43u,
        BPOS = 0x534F5042u,
        FAFX = 0x58464146u,
        CORN = 0x4E524F43u,
    }
}
