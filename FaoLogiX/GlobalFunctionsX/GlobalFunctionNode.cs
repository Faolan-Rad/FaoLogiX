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
    [NodeName("Global Function")]
    [Category(new string[] { "LogiX/Flow" })]
    public class GlobalFunctionNode : LogixNode, IChangeable, IWorldElement
    {
        public readonly Sync<string> Name;

        public readonly Output<GlobalFunctionNode> CallBack;

        public readonly Impulse Called;

        [HideInInspectorAttribute]
        public readonly SyncList<Output<object>> Input;

        public readonly SyncRef<GlobalFunctionCallNode> lastcall;

        [HideInInspectorAttribute]
        public readonly Sync<int> inputcount;

        [HideInInspectorAttribute]
        public readonly Sync<int> outputcount;

        [HideInInspectorAttribute]
        public readonly SyncRefList<GlobalFunctionCallNode> linked;

        [HideInInspectorAttribute]
        public readonly SyncRefList<ReturnNode> linkedretun;

        public void linkreturn(ReturnNode callback)
        {
            linkedretun.Add(callback);
        }

        public void unlinkreturn(ReturnNode callback)
        {
            linkedretun.Remove(callback);
        }


        public void intAddOutput()
        {
            outputcount.Value = outputcount.Value + 1;
            for (int i = 0; i < linked.Count; i++)
            {
                if(linked[i] != null)
                {
                    linked[i].intAddOutput();
                }
            }
            for (int i = 0; i < linkedretun.Count; i++)
            {
                if (linkedretun[i] != null)
                {
                    linkedretun[i].intAddOutput();
                }
            }

        }

        public void intRemoveOutput()
        {
            if (outputcount.Value == 0)
            {
                return;
            }
            outputcount.Value = outputcount.Value - 1;
            for (int i = 0; i < linked.Count; i++)
            {
                    if (linked[i] != null)
                    {
                        linked[i].intRemoveOutput();
                    }
            }
            for (int i = 0; i < linkedretun.Count; i++)
            {
                if (linkedretun[i] != null)
                {
                    linkedretun[i].intRemoveOutput();
                }
            }

        }

        public void intAddinput()
        {
            inputcount.Value = inputcount.Value + 1;
            for (int i = 0; i < linked.Count; i++)
            {
                if (linked[i] != null)
                {
                    linked[i].intAddinput();
                }
            }

            Input.Add();
            base.RefreshLogixBox();
        }

        public void intRemoveinput()
        {
            if(inputcount.Value == 0)
            {
                return;
            }
            inputcount.Value = inputcount.Value - 1;
            for (int i = 0; i < linked.Count; i++)
            {
                if (linked[i] != null)
                {
                    linked[i].intRemoveinput();
                }
            }

            Input.RemoveAt(Input.Count - 1);
            base.RefreshLogixBox();
        }


        protected override void OnGenerateVisual(Slot root)
        {
            UIBuilder uIBuilder;
            uIBuilder = base.GenerateUI(root, 384f, 160f);
            VerticalLayout verticalLayout;
            verticalLayout = uIBuilder.VerticalLayout(4f);
            verticalLayout.PaddingLeft.Value = 8f;
            verticalLayout.PaddingRight.Value = 16f;
            uIBuilder.Style.MinHeight = 20f;
            uIBuilder.TextField("", undo: false, null, parseRTF: false).Text.Content.DriveFrom(this.Name, writeBack: true);
            uIBuilder.Panel();
            uIBuilder.HorizontalFooter(20f, out var footer, out var _);
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
            intAddinput();
        }

        private void RemoveOutput(IButton button, ButtonEventData eventData)
        {
            if (this.Input.Count > 0)
            {
                intRemoveinput();
            }
        }
        protected override void OnAwake()
        {
            base.OnAwake();
            GlobalFunctionManager.addFunction(this);
            CallBack.Value = this;
        }
        protected override void OnDispose()
        {
            GlobalFunctionManager.removeFunction(this);
            base.OnDispose();

        }

        public void ReturnCall(object[] ReturnData,int count)
        {
            lastcall.Target.CallBackInt(ReturnData, count);
        }

        public void Call(GlobalFunctionCallNode callBack, object[] inputs,int count)
        {
            for (int i = 0; i < this.Input.Count; i++)
            {
                if (i < count)
                {
                    Input[i].Value = inputs[i];
                }
                else
                {
                    Input[i].Value = null;
                }
            }
            lastcall.Target = callBack;
            Called.Trigger();
        }
        public void link(GlobalFunctionCallNode callback)
        {
            linked.Add(callback);
        }

        public void unlink(GlobalFunctionCallNode callback)
        {
            linked.Remove(callback);
        }

    }
}
