using Contract;
using DynamicMethodsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            return targetMethod.Invoke(_logger, args);
        }
    }
}
