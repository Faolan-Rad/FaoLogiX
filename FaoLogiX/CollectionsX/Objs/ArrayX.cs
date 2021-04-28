using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using CollectionsX.Delegates;
namespace CollectionsX.Objs
{
    public interface ArrayX<T>: IEquatable<ArrayX<T>>, ICustomInspector
    {
        int Count { get; }
        void Append(T value = default(T));
        int Add(T value = default(T));

        event ArrayXDataChange<T> DataWritten;

        event ArrayXLengthChange<T> DataShortened;

        event ArrayXDataChange<T> DataInsert;

        string ToString();
        ICollectionsObj<T> GetObj(int index);
        T this[int index] { get;  }

        void RemoveAt(int index);
        void Remove(int index, int count);

        void Write(T value, int index);
        void Insert(T value, int index);
        int IndexOf(T value);
    }
}
