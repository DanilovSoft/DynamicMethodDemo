``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.17763.348 (1809/October2018Update/Redstone5)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=2.2.101
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|            Method |          Mean |          Error |          StdDev |
|------------------ |--------------:|---------------:|----------------:|
|            Static |      51.77 ns |      0.8738 ns |       0.8173 ns |
| ColdDynamicMethod | 869,727.15 ns | 66,977.4531 ns | 197,484.5317 ns |
|     DynamicMethod |      93.21 ns |      1.0437 ns |       0.9763 ns |
