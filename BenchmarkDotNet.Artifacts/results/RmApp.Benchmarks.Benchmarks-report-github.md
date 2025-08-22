```

BenchmarkDotNet v0.15.2, macOS Sequoia 15.6 (24G84) [Darwin 24.6.0]
Apple M1 Pro, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.410
  [Host]     : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD


```
| Method            | ShapeCount | Mean         | Error       | StdDev      | Median       | Allocated |
|------------------ |----------- |-------------:|------------:|------------:|-------------:|----------:|
| **CornerAreaVTBL4**   | **1000**       |     **511.0 ns** |     **0.90 ns** |     **0.84 ns** |     **510.7 ns** |         **-** |
| CornerAreaSwitch4 | 1000       |   4,146.5 ns |    29.36 ns |    26.03 ns |   4,135.1 ns |         - |
| CornerAreaUnion4  | 1000       |     601.8 ns |    10.77 ns |    16.11 ns |     593.5 ns |         - |
| **CornerAreaVTBL4**   | **10000**      |   **6,123.2 ns** |    **18.82 ns** |    **14.70 ns** |   **6,117.3 ns** |         **-** |
| CornerAreaSwitch4 | 10000      |  41,456.9 ns |    64.79 ns |    57.43 ns |  41,442.7 ns |         - |
| CornerAreaUnion4  | 10000      |   5,884.7 ns |     4.87 ns |     4.06 ns |   5,883.7 ns |         - |
| **CornerAreaVTBL4**   | **100000**     |  **61,939.5 ns** |   **620.01 ns** |   **517.74 ns** |  **61,703.6 ns** |         **-** |
| CornerAreaSwitch4 | 100000     | 416,743.3 ns | 1,222.80 ns | 1,021.09 ns | 416,471.7 ns |         - |
| CornerAreaUnion4  | 100000     |  58,414.4 ns |   116.33 ns |   103.13 ns |  58,390.2 ns |         - |
