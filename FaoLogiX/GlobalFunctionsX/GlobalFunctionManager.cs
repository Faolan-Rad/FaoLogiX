using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;

namespace FaoLogiX.GlobalFunctionsX
{
    public static class GlobalFunctionManager
    {
        static public List<GlobalFunction> Functions = new List<GlobalFunction>();
        static public bool FunctionExsets(string name, World world)
        {
            return (GetFunction(name, world) != null);
        }
        static public GlobalFunction GetFunction(string name, World world)
        {
            foreach (GlobalFunction func in Functions)
            {
                if (func.Name == name && func.World == world)
                {
                    return func;
                }

            }
            return null;
        }

        static void AddFunction(GlobalFunction func)
        {
            Functions.Add(func);
        }
    }
}
