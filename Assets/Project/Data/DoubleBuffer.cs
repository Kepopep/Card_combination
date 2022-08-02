
using System;
using System.Collections.Generic;

namespace Project.Data
{
    internal class DoubleBuffer<T>
    {
        public List<T> Current => _current;

        private List<T> _current;
        private List<T> _next;

        public event Action<T> OnItemAdd;
        public event Action<T> OnItemRemove;

        public DoubleBuffer()
        {
            _current = new List<T>();
            _next = new List<T>();
        }

        public void Add(T item)
        {
            _next.Add(item);

            OnItemAdd?.Invoke(item);
        }

        public void Remove(T item)
        {
            _next.Remove(item);

            if (item != null)
            {
                OnItemRemove?.Invoke(item);
            }
        }

        public void Swap()
        {
            _current.Clear();

            foreach (var item in _next)
            {
                _current.Add(item);
            }
        }

        public void Clear()
        {
            _current.Clear();
            _next.Clear();
        }

    }
}
