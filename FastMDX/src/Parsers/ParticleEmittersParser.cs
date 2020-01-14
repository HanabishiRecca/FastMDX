namespace FastMDX {
    class ParticleEmittersParser : IBlockParser {
        public void ReadFrom(MDX mdx, DataStream ds, uint blockSize) {
            mdx.ParticleEmitters = ds.ReadDataArrayUnknownCount<ParticleEmitter>(blockSize);
        }

        public void WriteTo(MDX mdx, DataStream ds) {
            ds.WriteDataArray(mdx.ParticleEmitters, false);
        }

        public bool HasData(MDX mdx) => mdx?.ParticleEmitters?.Length > 0;
    }
}
