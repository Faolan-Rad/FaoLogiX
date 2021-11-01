using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using CollectionsX.Objs;
using BaseX;

namespace FaoLogiX.BetterAccessX
{
	[NodeName("Button Clicker")]
	[Category(new string[] { "LogiX/Buttons"})]
	public class ButtonClicker : LogixNode, IChangeable, IWorldElement, IButton
	{
        public readonly Sync<string> LableText;
        public readonly SyncDelegate<ButtonEventHandler> pressed;

        public SyncDelegate<ButtonEventHandler> Pressed => released;

        public readonly SyncDelegate<ButtonEventHandler> pressing;

        public SyncDelegate<ButtonEventHandler> Pressing => released;

        public readonly SyncDelegate<ButtonEventHandler> released;

        public SyncDelegate<ButtonEventHandler> Released => released;

        public string LabelText { get => LableText.Value; set => LableText.Value = value; }

        public IField<string> LabelTextField => LableText;

        public event ButtonEventHandler LocalPressed;
        public event ButtonEventHandler LocalPressing;
        public event ButtonEventHandler LocalReleased;
        public event ButtonEventHandler LocalHoverEnter;
        public event ButtonEventHandler LocalHoverStay;
        public event ButtonEventHandler LocalHoverLeave;

        public readonly Input<float3> globalPressPoint;
        public readonly Input<float2> localPressPoint;
        public readonly Input<float2> normalPressPoint;

        public readonly Impulse pressedDone;
        public readonly Impulse PressingDone;
        public readonly Impulse ReleasedDone;

        [ImpulseTarget]
		public void PressedCall()
		{
            try
            {
                this.Slot.ForeachComponent<IButtonPressReceiver>((Action<IButtonPressReceiver>)(r => r.Pressed((IButton)this, new ButtonEventData(this, globalPressPoint.EvaluateRaw(), localPressPoint.EvaluateRaw(), normalPressPoint.EvaluateRaw()))), true, true);
                pressed.Target?.Invoke(this, new ButtonEventData(this, globalPressPoint.EvaluateRaw(), localPressPoint.EvaluateRaw(), normalPressPoint.EvaluateRaw()));
                pressedDone.Trigger();
            }
            catch { }
		}
        [ImpulseTarget]
        public void PressingCall()
        {
            try
            {
                this.Slot.ForeachComponent<IButtonPressReceiver>((Action<IButtonPressReceiver>)(r => r.Pressing((IButton)this, new ButtonEventData(this, globalPressPoint.EvaluateRaw(), localPressPoint.EvaluateRaw(), normalPressPoint.EvaluateRaw()))), true, true);
                pressing.Target?.Invoke(this, new ButtonEventData(this, globalPressPoint.EvaluateRaw(), localPressPoint.EvaluateRaw(), normalPressPoint.EvaluateRaw()));
                PressingDone.Trigger();
            }
            catch { }
        }
        [ImpulseTarget]
        public void ReleasedCall()
        {
            try
            {
                this.Slot.ForeachComponent<IButtonPressReceiver>((Action<IButtonPressReceiver>)(r => r.Released((IButton)this, new ButtonEventData(this, globalPressPoint.EvaluateRaw(), localPressPoint.EvaluateRaw(), normalPressPoint.EvaluateRaw()))), true, true);
                released.Target?.Invoke(this, new ButtonEventData(this, globalPressPoint.EvaluateRaw(), localPressPoint.EvaluateRaw(), normalPressPoint.EvaluateRaw()));
                ReleasedDone.Trigger();
            }
            catch { }
        }
    }
}
