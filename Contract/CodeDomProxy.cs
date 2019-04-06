using DynamicMethodsLib;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
#if NET471

    public class CodeDomProxy : TypeProxy
    {
        private readonly Logger _logger;
        private readonly Func<object, object[], object> _func;

        public CodeDomProxy(Logger instance)
        {
            _logger = instance;
            _func = Compile();
        }

        public override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _func(_logger, args);
        }

        private static Func<object, object[], object> Compile()
        {
            string code = @"
    using System;
    using Contract;
    using System.Security;

    //[assembly: AllowPartiallyTrustedCallers]
    //[assembly: SecurityTransparent]
    //[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]

    public static class Program
    {
        public static object Sum(object P_0, object[] P_1)
        {
            string refStr = (string)P_1[4];
            int outArg;
            int result = ((ILogger)P_0).Sum((string)P_1[0], (int)P_1[1], (long)P_1[2], out outArg, ref refStr);

            P_1[3] = outArg;
            P_1[4] = refStr;

            return result;
        }
    }
";
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            string dllPath = typeof(ILogger).Assembly.Location;


            // Reference to System.Drawing library
            parameters.ReferencedAssemblies.Add(dllPath);
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = false;

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }

            Assembly assembly = results.CompiledAssembly;
            Type program = assembly.GetType("Program");
            MethodInfo main = program.GetMethod("Sum");

            var func = (Func<object, object[], object>)main.CreateDelegate(typeof(Func<object, object[], object>));
            return func;
        }
    }
#endif
}
