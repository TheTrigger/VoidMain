``` ini

BenchmarkDotNet=v0.10.13, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.309)
Intel Core2 Duo CPU E8400 3.00GHz, 1 CPU, 2 logical cores and 2 physical cores
Frequency=2929352 Hz, Resolution=341.3724 ns, Timer=TSC
.NET Core SDK=2.1.4
  [Host]     : .NET Core 2.0.5 (CoreCLR 4.6.26020.03, CoreFX 4.6.26018.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.5 (CoreCLR 4.6.26020.03, CoreFX 4.6.26018.01), 64bit RyuJIT


```
|          Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|---------------- |----------:|----------:|----------:|-------:|----------:|
| CommandNameOnly |  4.788 us | 0.0169 us | 0.0141 us | 0.9842 |   3.03 KB |
|     OptionsOnly | 13.020 us | 0.0627 us | 0.0586 us | 2.6245 |   8.09 KB |
|    OperandsOnly |  9.121 us | 0.0760 us | 0.0674 us | 1.6327 |   5.05 KB |
|    ShortCommand | 10.093 us | 0.0578 us | 0.0513 us | 1.9379 |   5.96 KB |
|     LongCommand | 23.899 us | 0.1318 us | 0.1101 us | 4.7302 |  14.59 KB |
