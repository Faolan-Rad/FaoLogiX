using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
namespace CollectionsX
{
    public class Collectionsobj<T> : SyncObject, IEquatable<Collectionsobj<T>>
    {
        public readonly Sync<T> Value;

		public override bool Equals(object other)
		{
			if (other is Collectionsobj<T>)
			{
				return this.Equals((Collectionsobj<T>)other);
			}
			return false;
		}

        public bool Equals(Collectionsobj<T> other)
        {
            return EqualityComparer<T>.Default.Equals(other.Value.DirectValue, this.Value.DirectValue);
        }

	}
}
