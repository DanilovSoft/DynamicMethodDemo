``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.17763.348 (1809/October2018Update/Redstone5)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=2.2.101
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Job-DZCRZM : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

LaunchCount=50  RunStrategy=ColdStart  

```
|    Method |     Mean |    Error |   StdDev |   Median |       Min |        Max |
|---------- |---------:|---------:|---------:|---------:|----------:|-----------:|
| ColdStart | 12.77 us | 6.627 us | 142.3 us | 1.000 us | 0.8000 us | 6,699.3 us |
