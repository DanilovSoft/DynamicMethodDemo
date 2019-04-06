using DynamicMethodsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class MessengerProxy : TypeProxy
    {
        private readonly Messenger _messenger;

        public MessengerProxy(Messenger messenger)
        {
            _messenger = messenger;
        }

        public override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return targetMethod.InvokeFast(_messenger, args);
        }
    }
}
