using System.Reflection;

namespace Slide_InvokeFast
{

    class Program
    {
        public int Sqr(int value)
        {
            return value * value;
        }

        public void Main()
        {
            MethodInfo methodInfo = this.GetType().GetMethod("Sqr");

            object sqrResult = methodInfo.InvokeFast(this, new object[] { 4 });
        }
    }

}
