using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using CollectionsX.Objs;
using System.Reflection;
using BaseX;

namespace FaoLogiX.UnsafeX
{
	[NodeName("FuncCall")]
	[Category(new string[] { "LogiX/Experimental" })]
	[NodeDefaultType(typeof(FuncCall<Worker,object>))]
	public class FuncCall<T,O> : LogixNode, IChangeable, IWorldElement
	{
		public readonly Input<T> Target;

		public readonly Input<string> Method;

		public readonly Output<O> Output;

		public readonly Impulse Called;

		public readonly SyncList<Input<object>> Parameters;

		protected override void OnAttach()
		{
			base.OnAttach();
			Parameters.Add();
		}

		[SyncMethod]
		private void AddOutput(IButton button, ButtonEventData eventData)
		{
			Parameters.Add();
			RefreshLogixBox();
		}

		[SyncMethod]
		private void RemoveOutput(IButton button, ButtonEventData eventData)
		{
			if (Parameters.Count > 0)
			{
				Parameters.RemoveAt(Parameters.Count - 1);
				RefreshLogixBox();
			}
		}


		protected override void OnGenerateVisual(Slot root)
		{
			UIBuilder uIBuilder = GenerateUI(root);
			uIBuilder.Panel();
			uIBuilder.HorizontalFooter(32f, out var footer, out var _);
			UIBuilder uIBuilder2 = new UIBuilder(footer);
			uIBuilder2.HorizontalLayout(4f);
			LocaleString text = "+";
			color tint = color.White;
			uIBuilder2.Button(text, tint, AddOutput);
			text = "-";
			tint = color.White;
			uIBuilder2.Button( text, tint, RemoveOutput);
		}

		[ImpulseTarget]
		public void Call()
		{
			if (!base.World.UnsafeMode)
			{
				return;
			}
			var method = typeof(T).GetMethod(Method.EvaluateRaw(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			if(method is null)
            {

            }
			object[] array = Array.Empty<object>();
			if (Parameters.Count != 0)
			{
				array = new object[Parameters.Count];
				for (int i = 0; i < Parameters.Count; i++)
				{
					array[i] = Parameters[i].EvaluateRaw();
				}
			}
			Output.Value = (O)method.Invoke(Target.EvaluateRaw(), array);
		}
		
	}
}
