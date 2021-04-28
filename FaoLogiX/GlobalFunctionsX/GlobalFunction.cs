using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.UIX;

namespace FaoLogiX.GlobalFunctionsX
{
    public class GlobalFunction: SyncObject,ICustomInspector
    {
        public string Name;
        public void BuildInspectorUI(UIBuilder ui)
        {
            ui.PushStyle();
            ui.PopStyle();
        }
        protected virtual void OnAwake()
        {

        }

    }
}
