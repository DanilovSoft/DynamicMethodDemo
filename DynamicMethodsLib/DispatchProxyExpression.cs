using System.Reflection;

namespace DynamicMethodsLib
{
    public class DispatchProxyExpression<T> : DispatchProxy
    {
        private T _instance;

        public static T CreateProxy(T instance)
        {
            object proxy = Create<T, DispatchProxyExpression<T>>();
            var dispatchLoop = (DispatchProxyExpression<T>)proxy;
            dispatchLoop._instance = instance;
            return (T)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            //return targetMethod.Invoke(_instance, args);
            return targetMethod.InvokeExpressionFast(_instance, args);
        }
    }

    public class DispatchProxyDynamicMethod<T> : DispatchProxy
    {
        private T _instance;

        public static T CreateProxy(T instance)
        {
            object proxy = Create<T, DispatchProxyDynamicMethod<T>>();
            var dispatchLoop = (DispatchProxyDynamicMethod<T>)proxy;
            dispatchLoop._instance = instance;
            return (T)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            //return targetMethod.Invoke(_instance, args);
            return targetMethod.InvokeFast(_instance, args);
        }
    }

    public class DispatchProxyMethodInfo<T> : DispatchProxy
    {
        private T _instance;

        public static T CreateProxy(T instance)
        {
            object proxy = Create<T, DispatchProxyMethodInfo<T>>();
            var dispatchLoop = (DispatchProxyMethodInfo<T>)proxy;
            dispatchLoop._instance = instance;
            return (T)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return targetMethod.Invoke(_instance, args);
        }
    }
}
