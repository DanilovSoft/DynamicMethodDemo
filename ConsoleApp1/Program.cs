using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using Contract;
using DynamicMethodsLib;

namespace ConsoleApp1
{
    public class Program
    {
        private const int ForCount = 10_000_000;

        static void Main()
        {
            Console.Title = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            DynamicMethodFactory.CreateMethodCall(typeof(global::Program).GetMethod("Sqr"), true);
            Console.ReadLine();

            //Item item = new Item();


            //// ...
            //typeof(Item).GetProperty("ItemId").SetValue(item, 123);
            // ...



            //new global::Program().Demo();
            Logger logger = new Logger();
            ILogger staticProxy = TypeProxy.Create<ILogger, StaticProxy>(logger);
            ILogger dynamicMethodProxy = TypeProxy.Create<ILogger, DynamicMethodProxy<Logger>>(logger);
            ILogger dynamicExpressionProxy = TypeProxy.Create<ILogger, DynamicExpressionProxy>(logger);
            ILogger methodInfoProxy = TypeProxy.Create<ILogger, MethodInfoProxy>(logger);
            ILogger codeDomProxy = TypeProxy.Create<ILogger, CodeDomProxy>(logger);

            Console.WriteLine();

            // Прогрев CodeDOM.
            //for (int i = 0; i < 1; i++)
            {
                var sw = Stopwatch.StartNew();
                string refStr = "";
                int result = codeDomProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                sw.Stop();
                Console.WriteLine($"Холодный 'CodeDOM': {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} msec");
            }

            // Прогрев Dynamic Method.
            //for (int i = 0; i < 1; i++)
            {
                var sw = Stopwatch.StartNew();
                string refStr = "";
                int result = dynamicMethodProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                sw.Stop();
                Console.WriteLine($"Холодный 'Dynamic Method': {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} msec");
            }

            // Прогрев Dynamic Expression.
            //for (int i = 0; i < 1; i++)
            {
                var sw = Stopwatch.StartNew();
                string refStr = "";
                int result = dynamicExpressionProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                sw.Stop();
                Console.WriteLine($"Холодный 'Dynamic Expression': {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} msec");
            }

            // Прогрев MethodInfo.
            //for (int i = 0; i < 1; i++)
            {
                var sw = Stopwatch.StartNew();
                string refStr = "";
                int result = methodInfoProxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                sw.Stop();
                Console.WriteLine($"Холодный 'MethodInfo': {sw.ElapsedTicks} ticks, {sw.ElapsedMilliseconds} msec");
            }

            // Прогрев Static.
            //for (int i = 0; i < 1; i++)
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

            // CodeDOM.
            {
                int result = 0;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < ForCount; i++)
                {
                    string refStr = "";
                    result = codeDomProxy.Sum("CopyMe", 1, result, out int outArg, ref refStr);
                }
                sw.Stop();
                Debug.Assert(result == ForCount);
                Console.WriteLine($"'CodeDOM': {sw.ElapsedMilliseconds} msec, " +
                    $"{sw.ElapsedTicks / (double)ForCount:0.00} ticks, diff = {(float)methodInfoTime / sw.ElapsedTicks}");
            }

            // Dynamic Method.
            {
                int result = 0;
                var date = DateTime.Now;
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

        private static void DisplayTimerProperties()
        {
            //if (Stopwatch.IsHighResolution)
            //{
            //    Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
            //}
            //else
            //{
            //    Console.WriteLine("Operations timed using the DateTime class.");
            //}

            long nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            Console.WriteLine($"1 тик процессора составляет {nanosecPerTick} наносекунд");
            Console.WriteLine();
        }
    }
}
