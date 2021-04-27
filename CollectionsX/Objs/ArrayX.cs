using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CollectionsX.Objs
{
    public interface ArrayX<T>: IEquatable<ArrayX<T>>
    {
        int Count { get; }
        void Append(T value = default(T));
        int Add(T value = default(T));

        T this[int index] { get;  }

        void RemoveAt(int index);
        void Remove(int index, int count);

        void Write(T value, int index);
        void Insert(T value, int index);
        int IndexOf(T value);
    }
}
