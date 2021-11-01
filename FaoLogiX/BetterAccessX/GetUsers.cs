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
	[NodeName("Get Users")]
	[Category(new string[] { "LogiX/Users" })]
	public class GetUsers : LogixOperator<ArrayX<User>>, IValue<ArrayX<User>>, IChangeable, IWorldElement
	{
		public readonly Impulse Loaded;

		public readonly RefArrayX<User> Value;

		public override ArrayX<User> Content => this.Value;
		ArrayX<User> IValue<ArrayX<User>>.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				if (value != null)
				{
					this.Value.Copy(value);
				}
				else
				{
					this.Value.Clear();
				}
			}
		}

		[ImpulseTarget]
		public void Load()
		{
			Value.Clear();
			List<User> t = new List<User>();
			World.GetUsers(t);
			foreach (var item in t)
            {
				Value.Add(item);
            }
			Loaded.Trigger();
		}

	}
}
