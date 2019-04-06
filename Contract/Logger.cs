using System;

namespace Contract
{
    public interface ILogger
    {
        int Sum(string str, int num, long num2, out int outArg, ref string refStr);
        //int Wrapper(int n);
        void SendMessage(string message);
        string GetMessage(int messageId);
    }

    public class Logger : ILogger
    {
        public string GetMessage(int messageId)
        {
            return "";
        }

        public void SendMessage(string message)
        {
            
        }

        public int Sum(string str, int num, long num2, out int outArg, ref string refStr)
        {
            long sum = num + num2;
            outArg = num;
            refStr = str;
            return (int)sum;
        }

        public int Wrapper(int n)
        {
            return n;
        }
    }
}
