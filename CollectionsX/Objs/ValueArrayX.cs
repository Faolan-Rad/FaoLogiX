using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using CollectionsX.Objs;

namespace CollectionsX.Objs
{
    public class ValueArrayX<T>: SyncObject, ArrayX<T>
    {
        private readonly SyncArray<CollectionsItemValue<T>> Array;

        private CollectionsItemValue<T> MakeObj(T val)
        {
            CollectionsItemValue<T> tempObj;
            tempObj = new CollectionsItemValue<T>();
            tempObj.Initialize(World, this);
            tempObj.Set(val);
            return tempObj;
        }
        public int Count { get { return Array.Count; } }

        public T this[int index]
        {
            get
            {
                return Array[index].Get();
            }
            set
            {
                Write(value, index);
            }
        }
        public void Append(T value = default(T))
        {
            Array.Append(MakeObj(value));
        }

        public int Add(T value = default(T))
        {
            Append(value);
            return Count - 1;
        }
        public void RemoveAt(int index)
        {
            Array[index].Dispose();
            Array.RemoveAt(index);
        }

        public void Remove(int index, int count)
        {
            for (int i = 0; i < count; i++)
            	{
                    Array[index + i].Dispose();
            	}
            Array.Remove(index, count);
        }


        public void Write(T value, int index)
        {
            Array.Write(MakeObj(value), index);
        }

        public void Insert(T value, int index)
        {
            Array.Insert(MakeObj(value), index);
        }


        public int IndexOf(T value = default(T))
        {
            CollectionsItemValue<T> TempObj = MakeObj(value);
            int index = Array.IndexOf(TempObj);
            TempObj.Dispose();
            return index;
        }
        public override bool Equals(object other)
        {
            if (other is ArrayX<T>)
            {
                return this.Equals((ArrayX<T>)other);
            }
            return false;
        }

        public bool Equals(ArrayX<T> other)
        {
            bool same = other.Count == Count;
            if (same)
            {
                for (int index = 0; index < this.Count; index++)
                {
                    same = EqualityComparer<T>.Default.Equals(this[index], other[index]) && same;
                }
            }
            return same;
        }
    }
}
