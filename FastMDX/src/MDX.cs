using System;
using System.Collections.Generic;
using System.IO;

namespace FastMDX {
    public partial class MDX {
        public MDX() { }

        public MDX(string filePath) => LoadFromFile(filePath);

        BinaryBlock[] UnknownBlocks;

        public long ParsingTime { get; private set; }

        const uint MDX_VERSION = 800u;

        unsafe void LoadFromFile(string filePath) {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1, false);
            var fileHandle = stream.SafeFileHandle;

            var mdxHeader = new MDXHeader();
            if((FileApi.ReadFile(fileHandle, &mdxHeader, (uint)sizeof(MDXHeader)) < sizeof(MDXHeader)) || !mdxHeader.Check())
                throw new Exception("Not a MDX file!");

            if(mdxHeader.version != MDX_VERSION)
                throw new Exception("Not supported file version!");

            var len = (uint)(stream.Length - sizeof(MDXHeader));
            using var ds = new DataStream(len);
            FileApi.ReadFile(fileHandle, ds.Pointer, len);

            var unknownBlocks = new List<BinaryBlock>(10);

            var t = new System.Diagnostics.Stopwatch();
            t.Start();

            while(ds.Offset < ds.Size) {
                var blockHeader = ds.ReadStruct<BlockHeader>();

                _knownParsers.TryGetValue(blockHeader.tag, out var parser);

                if(parser is null)
                    unknownBlocks.Add(new BinaryBlock { Tag = blockHeader.tag, Data = ds.ReadStructArray<byte>(blockHeader.size) });
                else
                    parser.ReadFrom(this, ds, blockHeader.size);
            }

            t.Stop();
            ParsingTime = t.ElapsedTicks;

            UnknownBlocks = unknownBlocks.ToArray();
        }

        public unsafe void SaveToFile(string filePath) {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write, 1, false);
            var fileHandle = stream.SafeFileHandle;

            using var ds = new DataStream();

            var mdxHeader = new MDXHeader();
            mdxHeader.Default();
            mdxHeader.version = MDX_VERSION;

            ds.WriteStruct(ref mdxHeader);

            foreach(var parser in _knownParsers) {
                if(parser.Value.HasData(this)) {
                    ds.WriteStruct(parser.Key);

                    var offset = ds.Offset;
                    ds.Skip(sizeof(uint));

                    parser.Value.WriteTo(this, ds);

                    ds.SetValueAt(offset, ds.Offset - (offset + sizeof(uint)));
                }
            }

            if(UnknownBlocks?.Length > 0)
                foreach(var block in UnknownBlocks)
                    if(block?.Data?.Length > 0) {
                        ds.WriteStruct(block.Tag);
                        ds.WriteStructArray(block.Data);
                    }

            FileApi.WriteFile(fileHandle, ds.Pointer, ds.Offset);
            stream.Flush(true);
        }
    }
}
