using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;

namespace CollectionsX.Array
{
	[Category(new string[] { "LogiX/Collections/Array" })]
	[GenericTypes(GenericTypes.Group.NeosPrimitives, new Type[]
{
	typeof(Slot),
	typeof(User)
})]
	public class ArrayAdd<T> : LogixNode, IChangeable, IWorldElement
	{
        public readonly Output<int> Index;

        public readonly Input<SyncArray<Collectionsobj<T>>> List;

		public readonly Input<T> AddedValue;

		public readonly Impulse Added;

		[ImpulseTarget]
		public void Add()
		{
			SyncArray<Collectionsobj<T>> _listobj;
            _listobj = List.Evaluate();
			if (_listobj != null)
			{
				Collectionsobj<T> tempObj;
                tempObj = new Collectionsobj<T>();
				tempObj.Initialize(World, _listobj);
				tempObj.Value.Value = AddedValue.Evaluate();
				_listobj.Append(tempObj);
				this.Index.Value = _listobj.Count;
				this.Added.Trigger();
			}
		}
		protected override void OnGenerateVisual(Slot root)
		{
			UIBuilder uIBuilder;
			uIBuilder = base.GenerateUI(root, 184f, 76f);
			VerticalLayout verticalLayout;
			verticalLayout = uIBuilder.VerticalLayout(4f);
			verticalLayout.PaddingLeft.Value = 8f;
			verticalLayout.PaddingRight.Value = 16f;
			uIBuilder.Style.MinHeight = 32f;
			LocaleString text;
			text = typeof(T).Name;
			uIBuilder.Text(in text);
		}
		protected override void NotifyOutputsOfChange()
		{
		}
	}

}
