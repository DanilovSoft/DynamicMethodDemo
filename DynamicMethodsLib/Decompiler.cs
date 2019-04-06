using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMethodsLib
{
    public static class Decompiler
    {
#if NET471
        public static async Task DecompileToConsoleAsync(MethodInfo method, bool skipConvertion)
        {
            // Создать сборку содержащую метод и сохранить на диск.
            string assemblyName = SaveAssembly(method, skipConvertion, out string className);

            // Декомпилировать сборку.
            string sourceCode = DecompileType(assemblyName + ".dll", className);

            // Анализировать код.
            IEnumerable<Range> ranges = await AnalysisAsync(sourceCode);

            // Вывести в консоль.
            ConsoleSourceCodePrint(ranges);

            File.Delete(assemblyName + ".dll");
        }

        public static async Task DecompileToConsoleAsync(string assemblyName, string className)
        {
            // Декомпилировать сборку.
            string sourceCode = DecompileType(assemblyName, className);

            // Анализировать код.
            IEnumerable<Range> ranges = await AnalysisAsync(sourceCode);

            // Вывести в консоль.
            ConsoleSourceCodePrint(ranges);
        }

        private static string SaveAssembly(MethodInfo method, bool skipConvertion, out string className)
        {
            var assemblyName = new AssemblyName(Guid.NewGuid().ToString());
            string dllName = assemblyName.Name + ".dll";

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, fileName: dllName);

            className = "DummyProgram";
            TypeBuilder classBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public);

            MethodBuilder methodBuilder = classBuilder.DefineMethod(
                name: method.Name,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                returnType: typeof(object),
                parameterTypes: new Type[] { typeof(object), typeof(object[]) });

            ILGenerator il = methodBuilder.GetILGenerator();
            DynamicMethodFactory.GenerateIL(method, il, 1, skipConvertion);

            classBuilder.CreateType();
            assemblyBuilder.Save(dllName);
            return assemblyName.Name;
        }
#endif

        private static string DecompileType(string fileName, string className)
        {
            var decompiler = new CSharpDecompiler(
                fileName: fileName,
                settings: new DecompilerSettings(LanguageVersion.Latest)
                {
                    ThrowOnAssemblyResolveErrors = false,
                    UsingStatement = false,
                    UsingDeclarations = true,
                    ShowXmlDocumentation = true,
                });

            var name = new FullTypeName(className);

            string sourceCode = decompiler.DecompileTypeAsString(name);
            return sourceCode;

            //ITypeDefinition typeInfo = decompiler.TypeSystem.MainModule.Compilation.FindType(name).GetDefinition();

            //var sb = new StringBuilder();
            //foreach (var method in typeInfo.Methods)
            //{
            //    var tokenOfFirstMethod = method.MetadataToken;
            //    string sourceCode = decompiler.DecompileAsString(tokenOfFirstMethod);
            //    sb.AppendLine(sourceCode);
            //    sb.AppendLine();
            //}
            //return sb.ToString();
        }

        private static void ConsoleSourceCodePrint(IEnumerable<Range> ranges)
        {
            foreach (Range range in ranges)
            {
                switch (range.ClassificationType)
                {
                    case ClassificationTypeNames.Keyword:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        break;
                    case ClassificationTypeNames.ClassName:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case ClassificationTypeNames.StringLiteral:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case ClassificationTypeNames.NumericLiteral:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }

                Console.Write(range.Text);
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        private static async Task<IEnumerable<Range>> AnalysisAsync(string sourceCode)
        {
            var workspace = new AdhocWorkspace();
            Solution solution = workspace.CurrentSolution;
            Project project = solution.AddProject("projectName", "assemblyName", LanguageNames.CSharp);
            Document document = project.AddDocument("name.cs", sourceCode);
            document = await Formatter.FormatAsync(document);
            SourceText text = await document.GetTextAsync();

            IEnumerable<ClassifiedSpan> classifiedSpans = await Classifier.GetClassifiedSpansAsync(document, TextSpan.FromBounds(0, text.Length));
            Console.BackgroundColor = ConsoleColor.Black;

            IEnumerable<Range> ranges = classifiedSpans.Select(classifiedSpan =>
                new Range(classifiedSpan, text.GetSubText(classifiedSpan.TextSpan).ToString()));

            ranges = FillGaps(text, ranges);
            return ranges;
        }

        private static IEnumerable<Range> FillGaps(SourceText text, IEnumerable<Range> ranges)
        {
            const string WhitespaceClassification = null;
            int current = 0;
            Range previous = null;

            foreach (Range range in ranges)
            {
                int start = range.TextSpan.Start;
                if (start > current)
                    yield return new Range(WhitespaceClassification, TextSpan.FromBounds(current, start), text);

                if (previous == null || range.TextSpan != previous.TextSpan)
                    yield return range;

                previous = range;
                current = range.TextSpan.End;
            }

            if (current < text.Length)
                yield return new Range(WhitespaceClassification, TextSpan.FromBounds(current, text.Length), text);
        }
    }
}
