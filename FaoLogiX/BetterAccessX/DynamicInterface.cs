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

namespace FaoLogiX.BetterAccessX
{
	public interface CrazyPart
    {
		void SetRef(IWorldElement element);
    }
	public class Holder<T> : Output<T>, CrazyPart where T: class,IWorldElement
	{
        public void SetRef(IWorldElement element)
        {
			try
			{
				Value = (T)element;
            }
            catch { }
        }
    }

	[NodeName("Dynamic Interface")]
	[Category(new string[] { "LogiX/", "AbcFastGrab" })]
	[NodeDefaultType(typeof(DynamicInterface<Spinner>))]
	public class DynamicInterface<T> : LogixNode, IChangeable, IWorldElement where T: Worker
	{
		public readonly Input<T> data;

		[HideInInspector]
		public readonly SyncList<SyncVar> Outputs;
		//Faolan Thing
		//      protected override void OnGenerateVisual(Slot root)
		//      {
		//	Slot slot2 = base.Slot;
		//	int num = Outputs.Count;
		//	float num2 = (float)num * 32f + (float)(num + 1) * 4f + 8f;
		//	float num3 = 144f;
		//	var old = base.GenerateUI(root, num3 + 10, num2);
		//	root.FindInChildren("Canvas")?.FindInChildren("Image")?.FindInChildren("Vertical Layout")?.Destroy();
		//	var e = root.AddSlot("MadMan");
		//	e.Position_Field.Value = new float3(-0.001795141f,0.002062947f,0.005676508f);
		//	UIBuilder uIBuilder = new UIBuilder(e, num3, num2,0.001f);
		//	float num4 = 44f;
		//	var index = 0;
		//	foreach (var item in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
		//	{
		//		if (typeof(IWorldElement).IsAssignableFrom(item.FieldType))
		//		{
		//			SpriteProvider typeSprite2 = LogixHelper.GetTypeSprite(base.World, item.FieldType.GetDimensions());
		//			float3 localPoint = item.FieldType.GetColor().rgb;
		//			color tint = new color(in localPoint, 0.8f);
		//			var rectTransform = uIBuilder.Image(typeSprite2, in tint, preserveAspect: false).RectTransform;
		//			uIBuilder.Nest();
		//			LocaleString text = item.Name;
		//			uIBuilder.Text(in text);
		//			uIBuilder.NestOut();
		//			var fixedRect = new Rect(0f, 0f - num4, num3, 32f);
		//			RectTransform rectTransform2 = rectTransform;
		//			Rect rect = fixedRect;
		//			var xy = new float2(0f, 1f);
		//			rectTransform2.SetFixedRect(rect, in xy);
		//			num4 += 36f;
		//			var slot5 = rectTransform.Slot;
		//			slot5.AttachComponent<RectSlotDriver>();
		//			Sync<float3> size2 = slot5.AttachComponent<BoxCollider>().Size;
		//			xy = fixedRect.size * 0.001f;
		//			size2.Value = new float3(in xy, 0.01f);
		//			index--;
		//		}
		//	}
		//}

		protected override void OnGenerateVisual(Slot root)
		{
			base.OnGenerateVisual(root);
			var canvas = root[0].GetComponent<Canvas>();
			canvas.Size.Value = new float2(140f, canvas.Size.Value.y);
			var img = root[0][0];
			img[img.ChildrenCount - 1].Destroy();
			List<FieldInfo> info = new List<FieldInfo>(from l in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
								   where typeof(IWorldElement).IsAssignableFrom(l.FieldType)
								   select l);
			var index = 0;
			img.ForeachChild((slot) =>
			{
				var outproxy = slot.GetComponent<OutputProxy>();
				if (outproxy != null)
				{
					var rect = slot.GetComponent<RectTransform>();
					rect.OffsetMin.Value = new float2(12f, rect.OffsetMin.Value.y);
					rect.OffsetMax.Value = new float2(140f, rect.OffsetMax.Value.y);
					var text = slot.AddSlot("Text").AttachComponent<Text>();
					text.Content.Value = info[index].Name;
					text.AutoSizeMin.Value = 0f;
					text.AutoSizeMax.Value = 40f;
					text.HorizontalAutoSize.Value = true;
					text.VerticalAutoSize.Value = true;
					text.Align = Alignment.MiddleCenter;
					var collider = slot.GetComponent<BoxCollider>();
					collider.Size.Value = new float3(0.128f, collider.Size.Value.yz);
					//this is half of what is above this line
					slot[0].LocalPosition = new float3(0.064f, slot[0].LocalPosition.yz);
					index++;
				}
			});
		}


		protected override void OnAttach()
        {
			base.OnAttach();
			foreach (var item in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
			{
				if (typeof(IWorldElement).IsAssignableFrom(item.FieldType))
				{
					var putput = Outputs.Add();
					putput.ElementType = typeof(Holder<>).MakeGenericType(item.FieldType);
				}
			}			
		}

		protected override void OnInputChange()
		{
			var date = data.Evaluate();
			if (date is null)
			{
				return;
			}
			int index = 0;
			foreach (var item in typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
			{
				if (typeof(IWorldElement).IsAssignableFrom(item.FieldType))
				{
					var putput = Outputs[index];
					((CrazyPart)putput.Element).SetRef((IWorldElement)item.GetValue(date));
					index++;
				}
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("data", out var value))
			{
					if (value == null)
					{
						return null;
					}
				if (!typeof(Worker).IsAssignableFrom(value))
				{
					return null;
				}
				if (typeof(LogixNode).IsAssignableFrom(value))
				{
					return null;
				}
				var newslot = Slot.Parent.AddSlot("ValueGrab");
				newslot.AttachComponent(typeof(DynamicInterface<>).MakeGenericType(value));
				newslot.AttachComponent<Grabbable>();
				newslot.CopyTransform(Slot);
				Slot heldSlotReference = newslot;
				if (heldSlotReference != null && heldSlotReference != base.World.RootSlot)
				{
					heldSlotReference.GetComponentsInChildren<LogixNode>().ForEach(delegate (LogixNode n)
					{
						n.GenerateVisual();
					});
				}
				Slot.Destroy(true);
			}
			return null;
		}
	}


	[Category(new string[] { "LogiX/References" })]
	[NodeName("& - ValueGrab")]
	[NodeDefaultType(typeof(ValueGrab<float>))]
	public class ValueGrab<T> : LogixOperator<T> 
	{
		public readonly Input<IValue<T>> Reference;

		public override T Content
		{
			get
			{
				IValue<T> syncRef = Reference.EvaluateRaw();
				if (syncRef == null)
				{
					return (T)typeof(T).GetDefaultValue();
				}
				return syncRef.Value;
			}
		}

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (connectingTypes.inputs.TryGetValue("Reference", out var value))
			{
				Type type = value.EnumerateInterfacesRecursively().FirstOrDefault((Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValue<>));
				if (type != null)
				{
					return typeof(ValueGrab<>).MakeGenericType(type.GetGenericArguments()[0]);
				}
				return null;
			}
			return null;
		}

		protected override void NotifyOutputsOfChange()
		{
			((IOutputElement)this).NotifyChange();
		}
	}
}
