using DynamicMethodsLib;
using System.Reflection;

namespace Contract
{
    public class DynamicMethodProxy<T> : TypeProxy
    {
        private readonly T _decorated;

        public DynamicMethodProxy(T instance)
        {
            _decorated = instance;
        }

        public override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return targetMethod.InvokeFast(_decorated, args);
        }
    }
}
