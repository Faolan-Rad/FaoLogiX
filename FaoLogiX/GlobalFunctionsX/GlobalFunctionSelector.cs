using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.UIX;
using FrooxEngine.Undo;
using BaseX;

namespace FaoLogiX.GlobalFunctionsX
{
	public class GlobalFunctionSelector : Component
	{
		protected readonly FieldDrive<string> _textDrive;

		protected readonly SyncRef<Button> _button;

        protected readonly SyncRef<SyncRef<GlobalFunctionNode>> _targetRef;
        public virtual void Setup(SyncRef<GlobalFunctionNode> target)
		{
            this._targetRef.Target = target;
            BuildUI();
		}
        protected void BuildUI()
        {
            UIBuilder ui;
            ui = new UIBuilder(base.Slot);
            ui.HorizontalLayout(4f);
            ui.Style.FlexibleWidth = -1f;
            ui.Style.MinWidth = 24f;
            LocaleString text;
            text = "<<";
            ui.Button(in text, Decrement);
            ui.Style.FlexibleWidth = 100f;
            ui.Style.MinWidth = -1f;
            Button button;
            button = ui.Button();
            this._button.Target = button;
            this._textDrive.Target = button.Slot.GetComponentInChildren<Text>().Content;
            ui.Style.FlexibleWidth = -1f;
            ui.Style.MinWidth = 24f;
            text = ">>";
            ui.Button(in text, Increment);
            ui.NestOut();

        }

        protected override void OnCommonUpdate()
        {
            if (_targetRef.Target != null)
            {
                updateText();
            }
        }

        private void updateText()
        {
            if (_targetRef.Target.Target == null)
            {
                this._textDrive.Target.Value = "<i>null</i>";
            }
            else
            {
                this._textDrive.Target.Value = GetText();
            }
        }

        public string GetText()
        {
            return "Function: " + _targetRef.Target.Target.Name + " On:" + $"{_targetRef.Target.Target.Slot.Name} ({_targetRef.Target.Target.ReferenceID})";
        }

        private void Decrement(IButton button, ButtonEventData eventData)
		{
            _targetRef.Target.Target = GlobalFunctionManager.GetLast(_targetRef.Target.Target, World);

        }

		private void Increment(IButton button, ButtonEventData eventData)
		{
            _targetRef.Target.Target = GlobalFunctionManager.GetNext(_targetRef.Target.Target, World);
        }




    }

}
