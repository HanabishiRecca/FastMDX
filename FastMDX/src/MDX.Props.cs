using System.Collections.Generic;

namespace FastMDX {
    public partial class MDX {
        public ModelInfo Info;
        public Sequence[] Sequences;
        public GlobalSequence[] GlobalSequences;
        public Texture[] Textures;
        public Vec3[] Pivots;
        public Material[] Materials;
        public TextureAnimation[] TextureAnimations;
        public Geoset[] Geosets;
        public GeosetAnimation[] GeosetAnimations;
        public Bone[] Bones;
        public Light[] Lights;
        public Helper[] Helpers;
        public Attachment[] Attachments;
        public ParticleEmitter[] ParticleEmitters;
        public ParticleEmitter2[] ParticleEmitters2;
        public RibbonEmitter[] RibbonEmitters;
        public EventObject[] EventObjects;
        public Camera[] Cameras;
        public CollisionShape[] CollisionShapes;

        static Dictionary<uint, IBlockParser> _knownParsers = new Dictionary<uint, IBlockParser> {
            [(uint)KnownBlocks.MODL] = new ModelInfoParser(),
            [(uint)KnownBlocks.SEQS] = new SequencesParser(),
            [(uint)KnownBlocks.GLBS] = new GlobalSequencesParser(),
            [(uint)KnownBlocks.TEXS] = new TexturesParser(),
            [(uint)KnownBlocks.PIVT] = new PivotsParser(),
            [(uint)KnownBlocks.MTLS] = new MaterialsParser(),
            [(uint)KnownBlocks.GEOS] = new GeosetsParser(),
            [(uint)KnownBlocks.GEOA] = new GeosetAnimationsParser(),
            [(uint)KnownBlocks.BONE] = new BonesParser(),
            [(uint)KnownBlocks.LITE] = new LightsParser(),
            [(uint)KnownBlocks.HELP] = new HelpersParser(),
            [(uint)KnownBlocks.ATCH] = new AttachmentsParser(),
            [(uint)KnownBlocks.PREM] = new ParticleEmittersParser(),
            [(uint)KnownBlocks.PRE2] = new ParticleEmitters2Parser(),
            [(uint)KnownBlocks.RIBB] = new RibbonEmittersParser(),
            [(uint)KnownBlocks.EVTS] = new EventObjectsParser(),
            [(uint)KnownBlocks.CAMS] = new CamerasParser(),
            [(uint)KnownBlocks.CLID] = new CollisionShapesParser(),
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
