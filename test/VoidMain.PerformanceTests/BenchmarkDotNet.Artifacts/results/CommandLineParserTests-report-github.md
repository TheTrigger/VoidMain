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
| CommandNameOnly |  4.988 us | 0.1166 us | 0.1034 us | 0.8926 |   2.76 KB |
|     OptionsOnly | 12.602 us | 0.0704 us | 0.0659 us | 2.1362 |   6.61 KB |
|    OperandsOnly |  9.218 us | 0.0869 us | 0.0770 us | 1.4801 |   4.56 KB |
|    ShortCommand | 10.102 us | 0.0277 us | 0.0216 us | 1.6479 |    5.1 KB |
|     LongCommand | 23.097 us | 0.1474 us | 0.1307 us | 4.0894 |  12.58 KB |
