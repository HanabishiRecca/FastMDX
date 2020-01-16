using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace FastMDX {
    using static MainBlocks;

    public partial class MDX {
        const uint VERSION = 800u;

        public BinaryBlock[] UnknownBlocks;

        public MDX() { }

        public unsafe MDX(string filePath) {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1, false);
            var fileHandle = stream.SafeFileHandle;

            var mdxHeader = new MDXHeader();
            if((FileApi.ReadFile(fileHandle, &mdxHeader, (uint)sizeof(MDXHeader)) < sizeof(MDXHeader)) || !mdxHeader.Check())
                throw new Exception("Not a MDX file!");

            if(mdxHeader.version != VERSION)
                throw new Exception("Not supported file version!");

            var len = (uint)(stream.Length - sizeof(MDXHeader));
            using var ds = new DataStream(len);
            FileApi.ReadFile(fileHandle, ds.Pointer, len);

            var unknownBlocks = new List<BinaryBlock>(10);

            while(ds.Offset < ds.Size) {
                var blockHeader = ds.ReadStruct<BlockHeader>();

                _knownParsers.TryGetValue(blockHeader.tag, out var parser);

                if(parser is null)
                    unknownBlocks.Add(new BinaryBlock { Tag = blockHeader.tag, Data = ds.ReadStructArray<byte>(blockHeader.size) });
                else
                    parser.ReadFrom(this, ds, blockHeader.size);
            }

            UnknownBlocks = unknownBlocks.ToArray();
        }

        public unsafe void SaveToFile(string filePath) {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write, 1, false);
            var fileHandle = stream.SafeFileHandle;

            using var ds = new DataStream();

            var mdxHeader = new MDXHeader();
            mdxHeader.Default();
            mdxHeader.version = VERSION;

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

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct MDXHeader {
            internal MainBlocks header, versionHeader;
            internal uint versionSize, version;

            internal bool Check() => (header == MDLX) && (versionHeader == VERS) && (versionSize == sizeof(uint));

            internal void Default() {
                header = MDLX;
                versionHeader = VERS;
                versionSize = sizeof(uint);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BlockHeader {
            internal MainBlocks tag;
            internal uint size;
        }
    }

    public class BinaryBlock {
        public MainBlocks Tag;
        public byte[] Data;
    }
}
