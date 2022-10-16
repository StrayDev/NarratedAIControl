using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otherworld.Core
{
    /// <summary>
    /// This base class is a list that can be referenced by
    /// assets/prefabs and must be populated at runtime.
    /// </summary>

    public abstract class GenericRuntimeList<T> : ScriptableObject, IEnumerable<T>
    {
        protected readonly List<T> _list;

        protected GenericRuntimeList()
        {
            _list = new List<T>();
        }

        public T this[int i]
        {
            get => _list[i];
            set => _list[i] = value;
        }

        public T[] All => _list.ToArray();

        public int Capacity => _list.Capacity;
        public int Count => _list.Count;

        public T[]  ToArray() => _list.ToArray();
        
        public void Add(T value) => _list.Add(value);
        public void AddRange(T[] value) => _list.AddRange(value);
        public void Clear() => _list.Clear();
        public void RemoveRange(int index, int count) => _list.RemoveRange(index, count);
        
        public bool Contains(T value) => _list.Contains(value);
        public bool Remove(T value) => _list.Remove(value);

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    }

}
