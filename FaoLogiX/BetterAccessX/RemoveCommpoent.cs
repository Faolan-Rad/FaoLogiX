using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using CollectionsX.Objs;

namespace FaoLogiX.BetterAccessX
{
	[NodeName("Add Component")]
	[Category(new string[] { "LogiX/Components"})]
	public class RemoveCommpoent : LogixNode, IChangeable, IWorldElement
	{
		public readonly Input<Component> comp;

		public readonly Impulse Removed;

		[ImpulseTarget]
		public void Remove()
		{
			if(comp.Evaluate() is null)
            {
				return;
            }
			comp.Evaluate().Destroy();
			Removed.Trigger();
		}
		
	}
}
