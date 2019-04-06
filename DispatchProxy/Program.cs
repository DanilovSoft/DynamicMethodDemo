using System;
using System.Reflection;

namespace DispatchProxyApp
{
    public class Program : IProgram
    {
        private readonly Program _instance;

        static void Main()
        {
            Console.WriteLine("Hello World!");
            var proxy = Proxy<IProgram>.CreateProxy(new Program());
            proxy.Test();
        }

        void IProgram.Test()
        {
            throw new NotImplementedException();
        }
    }

    public class Proxy<T> : DispatchProxy
    {
        private T _instance;

        public static T CreateProxy(T instance)
        {
            object proxy = Create<T, Proxy<T>>();
            var dispatchLoop = (Proxy<T>)proxy;
            dispatchLoop._instance = instance;
            return (T)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return targetMethod.Invoke(_instance, args);
        }
    }


    public interface IProgram
    {
        void Test();
    }
}
