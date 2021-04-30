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
        static private List<GlobalFunctionNode> Functions = new List<GlobalFunctionNode>();


        static public void removeFunction(GlobalFunctionNode node)
        {
            Functions.Remove(node);
        }
        static public void addFunction(GlobalFunctionNode node)
        {
            Functions.Add(node);
        }
        static public IEnumerator<GlobalFunctionNode> GetFunctions(World world)
        {
            for (int i = 0; i < Functions.Count; i++)
            {
                if (Functions[i].World == world)
                {
                    yield return Functions[i];
                }
            }
        }

        static public GlobalFunctionNode GetNext(GlobalFunctionNode node, World world)
        {
            int current;
            if(node != null)
            {
               current = Functions.IndexOf(node);
            }
            else
            {
               current = -1;
            }
            for (int i = current + 1; i < Functions.Count; i++)
            {
                if (i < Functions.Count && i >= 0)
                {
                    if (Functions[i].World == world)
                    {
                        return Functions[i];
                    }
                }
            }
            for (int i = 0; i < Functions.Count; i++)
            {
                    if (i < Functions.Count && i >= 0)
                    {
                        if (Functions[i].World == world)
                        {
                            return Functions[i];
                        }
                    }
            }
            return node;
        }

        static public GlobalFunctionNode GetLast(GlobalFunctionNode node, World world)
        {
            int current;
            if (node != null)
            {
                current = Functions.IndexOf(node) - 1;
            }
            else
            {
                current = Functions.Count - 1;
            }
            for (int i = current; i >= -1; i--)
            {
                if (i < Functions.Count && i >= 0)
                {
                    if (Functions[i].World == world)
                    {
                        return Functions[i];
                    }
                }
            }
            for (int i = Functions.Count - 1; i >= -1; i--)
            {
                if (i < Functions.Count && i >= 0)
                {
                    if (Functions[i].World == world)
                    {
                        return Functions[i];
                    }
                }
            }
            return node;
        }
    }
}
