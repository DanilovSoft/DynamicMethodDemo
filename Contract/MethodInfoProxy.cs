using DynamicMethodsLib;
using System.Reflection;

namespace Contract
{
    public class MethodInfoProxy : TypeProxy
    {
        private readonly Logger _logger;

        public MethodInfoProxy(Logger logger)
        {
            _logger = logger;
        }

        public override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return targetMethod.InvokeFast(_logger, args);
        }
    }
}
