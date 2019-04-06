using Contract;
using DynamicMethodsLib;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Contract
{
    public class StaticProxy : TypeProxy
    {
        private readonly Logger _logger;

        public StaticProxy(Logger instance)
        {
            _logger = instance;
        }

        public override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return Sum(_logger, args);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static object Sum(object P_0, object[] P_1)
        {
            string refStr = (string)P_1[4];
            int result = ((ILogger)P_0).Sum((string)P_1[0], (int)P_1[1], (long)P_1[2], out int outArg, ref refStr);

            P_1[3] = outArg;
            P_1[4] = refStr;

            return result;
        }

        private static object Sum_Convertible(object P_0, object[] P_1)
        {
            int arg1;
            if (P_1[1] is int)
            {
                arg1 = (int)P_1[1];
            }
            else if (P_1[1] is IConvertible convertible)
            {
                arg1 = convertible.ToInt32(null);
            }
            else
                arg1 = (int)P_1[1];

            long arg2;
            if (P_1[2] is long)
            {
                arg2 = (long)P_1[2];
            }
            else if (P_1[2] is IConvertible convertible)
            {
                arg2 = convertible.ToInt64(null);
            }
            else
                arg2 = (long)P_1[2];

            string refStr = (string)P_1[4];

            object result = ((Logger)P_0).Sum((string)P_1[0], arg1, arg2, out int outArg, ref refStr);

            P_1[3] = outArg;
            P_1[4] = refStr;

            return result;
        }
    }
}
