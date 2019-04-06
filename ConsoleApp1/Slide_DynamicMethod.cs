using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Slide_DynamicMethod
{

    class Program
    {

        public int Sqr(int value)
        {
            return value * value;
        }

        public void Main()
        {
            var dynamicMethod = new DynamicMethod(
                name: "SqrWrapper",
                returnType: typeof(object),
                parameterTypes: new[] { typeof(object), typeof(object[]) }, // instance и args.
                owner: this.GetType());

            #region Тело метода

            MethodInfo methodInfo = GetType().GetMethod("Sqr");
            ILGenerator il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);                       // Загрузить первый аргумент в стек (instance).
            il.Emit(OpCodes.Castclass, typeof(Program));    // Аргумент в стеке приводится к нужному типу.
            il.Emit(OpCodes.Ldarg_1);                       // Загрузить второй аргумент в стек (args).
            il.Emit(OpCodes.Ldc_I4_0);                      // Загрузить в стек значение 0.
            il.Emit(OpCodes.Ldelem_Ref);                    // Загрузить элемент из args по указанному индексу 0.
            il.Emit(OpCodes.Unbox_Any, typeof(int));        // Привести элемент object к типу int.
            il.Emit(OpCodes.Callvirt, methodInfo);          // Вызвать целевой метод. (загруженные в стек аргументы передаются методу).
            il.Emit(OpCodes.Box, typeof(int));              // Упаковать возвращённый результат int что-бы вернуть как object.
            il.Emit(OpCodes.Ret);                           // Конец процедуры.

            #endregion

            // Создаём делегат. object(object, object[]).
            var sqrFunc = (MyDelegate)dynamicMethod.CreateDelegate(typeof(MyDelegate));

            // Вызываем делегат.
            object sqrResult = sqrFunc(this, new object[] { 4 });
        }

        private delegate object MyDelegate(object instance, object[] args);

        //private static object SqrWrapper(object instance, object[] args)
        //{
        //    return ((Program)instance).Sqr((int)args[0]);
        //}


    }

}
