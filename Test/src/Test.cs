using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using FastMDX;

static class Program {

    // =======================================================
    // Pass a model file path as an argument to start the test
    // =======================================================

    static void Main(string[] args) {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        if(!(args?.Length > 0))
            return;

        var path = args[0];
        if(!File.Exists(path))
            return;

        Run(path);
    }

    static void Run(string path) {
        // Parsing test + JIT warm up
        var mdx = new MDX(path);
        mdx.SaveToFile(Path.ChangeExtension(path, "new.mdx"));
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

        // Performance test

        Console.WriteLine($"CPU HPET: {(Stopwatch.IsHighResolution ? "yes" : "no (benchmark results may be inaccurate)")}");

        var N = 100;
        var St = 0L;
        var maxTime = 0L;
        var minTime = long.MaxValue;

        for(var i = 0; i < N; i++) {
            Console.Write($"\rRUNNING TEST [{i + 1}/{N}]");
            var time = Test(path);
            if(time > maxTime)
                maxTime = time;
            if(time < minTime)
                minTime = time;
            St += time;
        }

        Console.WriteLine();
        Console.WriteLine("Parsing time (not including disk access delay)");
        Console.WriteLine($"MIN: {SWTime(minTime)} ms");
        Console.WriteLine($"AVG: {SWTime(St) / N} ms");
        Console.WriteLine($"MAX: {SWTime(maxTime)} ms");
    }

    static long Test(string file) {
        var mdx = new MDX(file);

        var time = mdx.ParsingTime;

        // Force GC to avoid impact during the measurement interval
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

        return time;
    }

    static double SWTime(long ticks) => ticks * 1000.0 / Stopwatch.Frequency;
}
