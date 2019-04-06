using DynamicMethodsLib;
using System.Collections.Concurrent;

namespace System.Reflection
{
    public static class MethodInfoExtensions
    {
        private static readonly ConcurrentDictionary<MethodInfo, Func<object, object[], object>> _dynamicMethods = 
            new ConcurrentDictionary<MethodInfo, Func<object, object[], object>>();

        private static readonly ConcurrentDictionary<MethodInfo, Func<object, object[], object>> _dynamicExpressions = 
            new ConcurrentDictionary<MethodInfo, Func<object, object[], object>>();

        /// <summary>
        /// Dynamic Method.
        /// </summary>
        public static object InvokeFast(this MethodInfo methodInfo, object instance, object[] args, bool skipConvertion = true)
        {
            Func<object, object[], object> func = _dynamicMethods.GetOrAdd(methodInfo, tm => DynamicMethodFactory.CreateMethodCall(methodInfo, skipConvertion));
            object result = func.Invoke(instance, args);
            return result;
        }

//#pragma warning disable IDE0060


//        public static object InvokeFast(this MethodInfo methodInfo, object instance, object[] args)
//        {
//            // ...?
//        }

//#pragma warning restore IDE0060 // Удалите неиспользуемый параметр

        /// <summary>
        /// Dynamic Expression.
        /// </summary>
        public static object InvokeExpressionFast(this MethodInfo methodInfo, object instance, params object[] args)
        {
            Func<object, object[], object> func = _dynamicExpressions.GetOrAdd(methodInfo, tm => DynamicExpressionFactory.CreateMethodDelegate(methodInfo));
            object result = func.Invoke(instance, args);
            return result;
        }
    }
}
