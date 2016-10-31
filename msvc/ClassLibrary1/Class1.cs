using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        static extern string GetString();

        public static void DoStuff()
        {
            Console.WriteLine("HELLO WORLD! String is " + GetString());
        }
    }
}
