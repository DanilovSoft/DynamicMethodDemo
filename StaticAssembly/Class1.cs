using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticAssembly
{
    public class StubClass
    {
        private object Dynamic_CallMe(object[] args)
        {
            return CallMe((int)args[0]);
        }

        public int CallMe(int n)
        {
            return n + 1;
        }
    }
}
