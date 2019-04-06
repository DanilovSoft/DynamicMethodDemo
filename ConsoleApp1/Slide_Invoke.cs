using System.Reflection;

namespace Slide_Invoke
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

            object sqrResult = methodInfo.Invoke(this, new object[] { 4 });
        }
    }

}
