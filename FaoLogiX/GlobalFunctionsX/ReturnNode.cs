using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using BaseX;

namespace FaoLogiX.GlobalFunctionsX
{
    [NodeName("Return")]
    [Category(new string[] { "LogiX/Flow" })]
    public class ReturnNode : LogixNode, IChangeable, IWorldElement
    {
        public readonly Input<GlobalFunctionNode> CallBack;

        public readonly SyncRef<GlobalFunctionNode> lastlink;

        public readonly SyncList<Input<object>> Returns;

        [ImpulseTarget]
        public void Return()
        {
            object[] _parameters;
            _parameters = new object[this.Returns.Count];
            for (int i = 0; i < this.Returns.Count; i++)
            {
                _parameters[i] = this.Returns[i].EvaluateRaw();
            }
            CallBack.Evaluate().ReturnCall(_parameters, this.Returns.Count);
        }

        protected override void OnChanges()
        {
            base.OnChanges();
            GlobalFunctionNode target = CallBack.Evaluate();
            if (target == lastlink.Target || target == null)
            {
                return;
            }
            if (lastlink.Target != null)
            {
                lastlink.Target.unlinkreturn(this);
            }
            target.linkreturn(this);
            lastlink.Target = target;
            Returns.Clear();
            for (int i = 0; i < lastlink.Target.outputcount.Value; i++)
            {
                Returns.Add();
            }
            RefreshLogixBox();
        }

        protected override void OnGenerateVisual(Slot root)
        {
            UIBuilder uIBuilder;
            uIBuilder = base.GenerateUI(root);
            uIBuilder.Panel();
            uIBuilder.HorizontalFooter(32f, out var footer, out var _);
            UIBuilder uIBuilder2;
            uIBuilder2 = new UIBuilder(footer);
            uIBuilder2.HorizontalLayout(4f);
            LocaleString text;
            text = "+";
            color tint;
            tint = color.White;
            uIBuilder2.Button(in text, in tint, AddOutput);
            text = "-";
            tint = color.White;
            uIBuilder2.Button(in text, in tint, RemoveOutput);
        }

        private void AddOutput(IButton button, ButtonEventData eventData)
        {
            if(lastlink != null)
            {
                lastlink.Target.intAddOutput();
            }
        }

        private void RemoveOutput(IButton button, ButtonEventData eventData)
        {
            if (this.Returns.Count > 0)
            {
                if (lastlink != null)
                {
                    lastlink.Target.intRemoveOutput();
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (lastlink.Target == null)
            {
                return;
            }
            lastlink.Target.unlinkreturn(this);
        }
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

    }
}
