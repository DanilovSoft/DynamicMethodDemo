using System;
using System.Reflection;

//namespace Slide_Static
//{

    class Program
    {
        public int Sqr(int value)
        {
            return value * value;
        }

        public void Main()
        {
            object sqrResult = SqrWrapper(this, new object[] { 4 });
        }

        public static object SqrWrapper(object instance, object[] args)
        {
            return ((Program)instance).Sqr((int)args[0]);
        }
    }

//}
