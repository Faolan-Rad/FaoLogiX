using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine.LogiX;
using FrooxEngine;
using FrooxEngine.UIX;

namespace FaoLogiX.GlobalFunctionsX
{
    [NodeName("Call Global Function")]
    [Category(new string[] { "LogiX/Flow" })]
    public class GlobalFunctionCallNode: LogixNode, IChangeable, IWorldElement
    {
        public readonly SyncRef<GlobalFunctionNode> Target;

        public readonly Impulse CallBack;

        public readonly SyncRef<GlobalFunctionNode> lastlink;

        [HideInInspectorAttribute]
        public readonly SyncList<Output<object>> Returns;

        public readonly SyncList<Input<object>> Input;

        public void intAddOutput()
        {
            this.Returns.Add();
            base.RefreshLogixBox();
        }

        public void intRemoveOutput()
        {
            this.Returns.RemoveAt(this.Returns.Count - 1);
            base.RefreshLogixBox();
        }

        public void intAddinput()
        {
            this.Input.Add();
            base.RefreshLogixBox();
        }

        public void intRemoveinput()
        {
            this.Input.RemoveAt(this.Input.Count - 1);
            base.RefreshLogixBox();
        }


        [ImpulseTarget]
        public void Call()
        {
            object[] _parameters;
            _parameters = new object[this.Input.Count];
            for (int i = 0; i < this.Input.Count; i++)
            {
                _parameters[i] = this.Input[i].EvaluateRaw();
            }
            Target.Target.Call(this, _parameters, this.Input.Count);
        }

        protected override void OnChanges()
        {
            base.OnChanges();
            if (Target.Target == lastlink.Target)
            {
                return;
            }
            if (lastlink.Target != null)
            {
                lastlink.Target.unlink(this);
            }
            Target.Target.link(this);
            lastlink.Target = Target.Target;
            Returns.Clear();
            Input.Clear();

            for (int i = 0; i < this.lastlink.Target.inputcount.Value; i++)
            {
                Input.Add();
            }
            for (int i = 0; i < this.lastlink.Target.outputcount.Value; i++)
            {
                Returns.Add();
            }
            base.RefreshLogixBox();
        }

        public void CallBackInt(object[] ReturnData, int amount)
        {
            for (int i = 0; i < this.Returns.Count; i++)
            {
                if (i < amount)
                {
                    Returns[i].Value = ReturnData[i];
                }
                else
                {
                    Returns[i].Value = null;
                }
            }
            CallBack.Trigger();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (lastlink.Target == null)
            {
                return;
            }
            lastlink.Target.unlink(this);
        }

        protected override void OnGenerateVisual(Slot root)
        {
            UIBuilder uIBuilder;
            uIBuilder = base.GenerateUI(root, 384f, 186f);
            VerticalLayout verticalLayout;
            verticalLayout = uIBuilder.VerticalLayout(4f);
            verticalLayout.PaddingLeft.Value = 8f;
            verticalLayout.PaddingRight.Value = 16f;
            uIBuilder.Style.MinHeight = 52f;
            GlobalFunctionSelector thing = uIBuilder.Root.AttachComponent<GlobalFunctionSelector>();
            thing.Setup(Target);
        }
    }
}
