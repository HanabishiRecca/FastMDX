namespace FastMDX {
    class ParticleEmitters2Parser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.ParticleEmitters2 = ds.ReadDataArrayUnknownCount<ParticleEmitter2>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.ParticleEmitters2, false);
        }

        public bool HasData(MDX mdx) => mdx?.ParticleEmitters2?.Length > 0;
    }
}
