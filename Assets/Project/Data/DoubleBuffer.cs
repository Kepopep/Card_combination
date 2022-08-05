using System.Collections.Generic;

namespace Project.Data
{
    internal class DoubleBuffer<T>
    {
        public List<T> Current => _current;

        private List<T> _current;
        private List<T> _next;

        public DoubleBuffer()
        {
            _current = new List<T>();
            _next = new List<T>();
        }

        public void Add(T item)
        {
            _next.Add(item);
        }

        public void Remove(T item)
        {
            _next.Remove(item);
        }

        public void Swap()
        {
            _current.Clear();
            _current.AddRange(_next);
        }

        public void Clear()
        {
            _current = new List<T>();
            _next = new List<T>();
        }
    }
}
