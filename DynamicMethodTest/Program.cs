using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Contract;
using DynamicMethodsLib;

namespace CoreConsoleApp
{
    public class Program
    {
        private const int ForCount = 10_000_000;

        public static void Main()
        {
            //BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchmarkJob>();
            //return;

            Logger logger = new Logger();
            ILogger staticProxy = TypeProxy.Create<ILogger, StaticProxy>(logger);
            ILogger dynamicMethodProxy = TypeProxy.Create<ILogger, DynamicMethodProxy<Logger>>(logger);
            ILogger dynamicExpressionProxy = TypeProxy.Create<ILogger, DynamicExpressionProxy>(logger);
            ILogger methodInfoProxy = TypeProxy.Create<ILogger, MethodInfoProxy>(logger);

            Console.WriteLine();

            // Прогрев Dynamic Method.
            {
                var sw = Stopwatch.StartNew();
                string refStr = "";
                int result = dynamicMethodProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                sw.Stop();
                Console.WriteLine($"Холодный 'Dynamic Method': {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} msec");
            }

            // Прогрев Dynamic Expression.
            {
                var sw = Stopwatch.StartNew();
                string refStr = "";
                int result = dynamicExpressionProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                sw.Stop();
                Console.WriteLine($"Холодный 'Dynamic Expression': {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} msec");
            }

            // Прогрев MethodInfo.
            {
                var sw = Stopwatch.StartNew();
                string refStr = "";
                int result = methodInfoProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                sw.Stop();
                Console.WriteLine($"Холодный 'MethodInfo': {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} msec");
            }

            // Прогрев Static.
            {
                var sw = Stopwatch.StartNew();
                string refStr = "";
                int result = staticProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                sw.Stop();
                Trace.WriteLine(result);
                Console.WriteLine($"Холодный 'Static': {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} msec");
            }

            Console.WriteLine();
            DisplayTimerProperties();
            long methodInfoTime;

            // MethodInfo.
            {
                long result = 0;
                var date = DateTime.Now;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < ForCount; i++)
                {
                    string refStr = "";
                    result = methodInfoProxy.Sum("CopyMe", 1, result, out int outArg, ref refStr);
                }
                sw.Stop();
                Debug.Assert(result == ForCount);
                Console.WriteLine($"'MethodInfo': {sw.ElapsedMilliseconds} msec, " +
                    $"{sw.ElapsedTicks / (double)ForCount:0.00} ticks");

                methodInfoTime = sw.ElapsedTicks;
            }

            // Static.
            {
                int result = 0;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < ForCount; i++)
                {
                    string refStr = "";
                    result = staticProxy.Sum("CopyMe", 1, result, out int outArg, ref refStr);
                }
                sw.Stop();
                Debug.Assert(result == ForCount);
                Console.WriteLine($"'Static': {sw.ElapsedMilliseconds} msec, " +
                    $"{sw.ElapsedTicks / (double)ForCount:0.00} ticks, diff = {(float)methodInfoTime / sw.ElapsedTicks}");
            }

            // Dynamic Method.
            {
                int result = 0;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < ForCount; i++)
                {
                    string refStr = "";
                    result = dynamicMethodProxy.Sum("CopyMe", 1, result, out int outArg, ref refStr);
                }
                sw.Stop();
                Debug.Assert(result == ForCount);
                Console.WriteLine($"'Dynamic Method': {sw.ElapsedMilliseconds} msec, " +
                    $"{sw.ElapsedTicks / (double)ForCount:0.00} ticks, diff = {(float)methodInfoTime / sw.ElapsedTicks}");
            }

            // Dynamic Expression.
            {
                int result = 0;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < ForCount; i++)
                {
                    string refStr = "";
                    result = dynamicExpressionProxy.Sum("CopyMe", 1, result, out int outArg, ref refStr);
                }
                sw.Stop();
                Debug.Assert(result == ForCount);
                Console.WriteLine($"'Dynamic Expression': {sw.ElapsedMilliseconds} msec, " +
                    $"{sw.ElapsedTicks / (double)ForCount:0.00} ticks, diff = {(float)methodInfoTime / sw.ElapsedTicks}");
            }

            Console.WriteLine();
            Console.Write("Press Any Key...");
            Console.ReadKey();
        }

        public static void DisplayTimerProperties()
        {
            long nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            Console.WriteLine($"1 тик процессора составляет {nanosecPerTick} наносекунд");
            Console.WriteLine();
        }
    }

    public class BenchmarkJob
    {
        private readonly Logger _logger;
        private readonly ILogger _staticProxy;
        private readonly ILogger _dynamicMethodProxy;

        public BenchmarkJob()
        {
            _logger = new Logger();
            _staticProxy = TypeProxy.Create<ILogger, StaticProxy>(_logger);
            _dynamicMethodProxy = TypeProxy.Create<ILogger, DynamicMethodProxy<Logger>>(_logger);
        }

        [Benchmark]
        public void Static()
        {
            string refStr = "";
            int result = _staticProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
        }

        [Benchmark]
        public void ColdDynamicMethod()
        {
            var _logger = new Logger();
            ILogger dynamicMethodProxy = TypeProxy.Create<ILogger, DynamicMethodProxy<Logger>>(_logger);

            string refStr = "";
            int result = dynamicMethodProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
        }

        [Benchmark]
        public void DynamicMethod()
        {
            string refStr = "";
            int result = _dynamicMethodProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
        }
    }
}
