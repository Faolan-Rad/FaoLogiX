using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using CollectionsX.Objs;

namespace CollectionsX.Array
{
	[Category(new string[] { "LogiX/Collections/Array" })]
	[GenericTypes(GenericTypes.Group.NeosPrimitives, new Type[]
{
	typeof(Slot),
	typeof(User)
})]
	public class ArrayIndexOf<T> : LogixNode, IChangeable, IWorldElement
	{
        public readonly Output<int> Index;

        public readonly Output<bool> NotFound;

		public readonly Input<ArrayX<T>> List;

		public readonly Input<T> Value;
		protected override void OnEvaluate()
		{
			ArrayX<T> _listobj;
            _listobj = List.Evaluate();
            if (_listobj != null)
            {
				try
				{
					this.Index.Value = _listobj.IndexOf( Value.Evaluate());
					NotFound.Value = this.Index.Value < 0;
					NotifyOutputsOfChange();
				}
				catch 
				{
                    NotFound.Value = true;
					NotifyOutputsOfChange();
				}
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
            ((IOutputElement)this.Index).NotifyChange();
			((IOutputElement)this.NotFound).NotifyChange();

		}
	}

}
