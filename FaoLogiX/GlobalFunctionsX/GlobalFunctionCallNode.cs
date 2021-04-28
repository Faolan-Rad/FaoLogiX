using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine.LogiX;
using FrooxEngine;
namespace FaoLogiX.GlobalFunctionsX
{
    [NodeName("Call Global Function")]
    [Category(new string[] { "LogiX" })]
    public class GlobalFunctionCallNode: LogixNode, IChangeable, IWorldElement, IGlobalFunctionsCallBack
    {
		public void CallBack(object[] ReturnData)
        {

        }
    }
}
