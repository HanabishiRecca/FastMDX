using System;
using System.Globalization;
using System.IO;
using FastMDX;

static class Program {

    // =======================================================
    // Pass a model file path as an argument to start the test
    // =======================================================

    static void Main(string[] args) {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        if(!(args?.Length > 0)) {
            Console.WriteLine("Pass a model file path as an argument to start the test");
            return;
        }

        var path = args[0];
        if(!File.Exists(path)) {
            Console.WriteLine($"Path {path} not found");
            return;
        }

        Console.WriteLine($"Loading from \"{path}\"");

        var mdx = new MDX(path);

        Console.WriteLine();
        Console.WriteLine($"{nameof(mdx.Info.Name)}: {mdx.Info.Name}");
        Console.WriteLine($"{nameof(mdx.Sequences)}: {mdx.Sequences?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.GlobalSequences)}: {mdx.GlobalSequences?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Materials)}: {mdx.Materials?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Textures)}: {mdx.Textures?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.TextureAnimations)}: {mdx.TextureAnimations?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Geosets)}: {mdx.Geosets?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.GeosetAnimations)}: {mdx.GeosetAnimations?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Bones)}: {mdx.Bones?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Lights)}: {mdx.Lights?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Helpers)}: {mdx.Helpers?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Attachments)}: {mdx.Attachments?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Pivots)}: {mdx.Pivots?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.ParticleEmitters)}: {mdx.ParticleEmitters?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.ParticleEmitters2)}: {mdx.ParticleEmitters2?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.RibbonEmitters)}: {mdx.RibbonEmitters?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.EventObjects)}: {mdx.EventObjects?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.Cameras)}: {mdx.Cameras?.Length ?? 0}");
        Console.WriteLine($"{nameof(mdx.CollisionShapes)}: {mdx.CollisionShapes?.Length ?? 0}");
        Console.WriteLine();

        var newPath = Path.ChangeExtension(path, "new.mdx");
        Console.WriteLine($"Saving to \"{newPath}\"");
        mdx.SaveTo(newPath);
    }
}
