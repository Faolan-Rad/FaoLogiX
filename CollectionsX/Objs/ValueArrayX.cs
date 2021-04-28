using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using BaseX;
using CollectionsX.Objs;
using FrooxEngine.UIX;
using CollectionsX.Delegates;

namespace CollectionsX.Objs
{
    public class ValueArrayX<T> : SyncObject, ArrayX<T>
    {
        private readonly SyncRefList<CollectionsItemValue<T>> Array;

        private readonly SyncBag<CollectionsItemValue<T>> Save;

        public event ArrayXDataChange<T> DataWritten;

        public event ArrayXLengthChange<T> DataShortened;

        public event ArrayXDataChange<T> DataInsert;


        public ICollectionsObj<T> GetObj(int index)
        {
            return Array[index];
        }

        private CollectionsItemValue<T> MakeObj(T val)
        {
            CollectionsItemValue<T> tempObj = Save.Add();
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
            base.World.RunSynchronously(delegate
            {
                Array.Add(MakeObj(value));
                if (base.LocalUser == base.World.HostUser)
                {
                    DataWritten(this, Count - 1, 1);
                }
            });
        }

        public int Add(T value = default(T))
        {
            Append(value);
            return Count - 1;
        }
        public void RemoveAt(int index)
        {
            base.World.RunSynchronously(delegate
            {
                Save.Remove(Array[index]);
            Array.RemoveAt(index);
                if (base.LocalUser == base.World.HostUser)
                {
                    DataShortened(this, index, 1);
                }
            });
        }

        public void Remove(int index, int count)
        {
            base.World.RunSynchronously(delegate
            {
                for (int i = 0; i < count; i++)
            {
                Array[index + i].Dispose();
                Array.RemoveAt(i);
            }
                
                if (base.LocalUser == base.World.HostUser)
                {
                    DataShortened(this, index, count);
                }
            });
        }


        public void Write(T value, int index)
        {
            base.World.RunSynchronously(delegate
            {
                Array[index].Value.Value = value;
                DataWritten(this, index, 1);
                if (base.LocalUser == base.World.HostUser)
                {
                    DataWritten(this, index, 1);
                }
            });
        }

        public void Insert(T value, int index)
        {
            base.World.RunSynchronously(delegate
            {
                Array.Insert(index, MakeObj(value));
                DataInsert(this, index, 1);
                if (base.LocalUser == base.World.HostUser)
                {
                    DataInsert(this, index, 1);
                }
            });
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

        public void BuildInspectorUI(UIBuilder ui)
        {
            ui.PushStyle();
            ui.Style.MinHeight = -1f;
            ui.VerticalLayout(4f);
            ui.Style.MinHeight = 24f;
            LocaleString text;
            ui.Style.MinHeight = -1f;
            ui.VerticalLayout(4f);
            ArrayXEditor<T> obj = ui.Root.AttachComponent<ArrayXEditor<T>>();
            ui.NestOut();
            ui.Style.MinHeight = 24f;
            text = "Add";
            Button addButton;
            addButton = ui.Button(in text);
            ui.NestOut();
            obj.Setup(this, addButton);
            ui.PopStyle();
        }
    }
}
