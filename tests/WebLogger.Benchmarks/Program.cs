// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using WebLogger.Benchmarks.WebLogger.Render;

Console.WriteLine("Hello, World!");

var summary = BenchmarkRunner.Run<ElementBenchmark>();