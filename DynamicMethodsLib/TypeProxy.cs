using System.Reflection;

namespace DynamicMethodsLib
{
    public abstract class TypeProxy
    {
        public static T Create<T, TProxy>()
        {
            return ProxyBuilder<TProxy>.CreateProxy<T>();
        }

        public static T Create<T, TProxy>(T instance)
        {
            return ProxyBuilder<TProxy>.CreateProxy(instance: instance);
        }

        public abstract object Invoke(MethodInfo targetMethod, object[] args);
    }
}
