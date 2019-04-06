using DynamicMethodsLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UnitTests
{
    public class TestClass : ITest
    {
        public string Dummy(string retArg)
        {
            return retArg;
        }

        public int DummyValueArg(int retArg)
        {
            return retArg;
        }

        public long DummyValueArg(long retArg)
        {
            return retArg;
        }

        public void Empty()
        {
            
        }

        public object RetObject(string retArg)
        {
            return retArg;
        }

        public int Sum(string str, int num, long num2, out int outArg, ref string refStr)
        {
            int sum = (int)(num + num2);
            outArg = sum;
            refStr = str;
            return sum;
        }
    }

    public interface ITest
    {
        void Empty();
        int DummyValueArg(int retArg);
        long DummyValueArg(long retArg);
        string Dummy(string retArg);
        object RetObject(string retArg);
        int Sum(string str, int num, long num2, out int outArg, ref string refStr);
    }
}
