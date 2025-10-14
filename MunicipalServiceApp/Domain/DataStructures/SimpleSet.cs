using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServiceApp.Domain.DataStructures
{
    // Minimal set built on our SinglyLinkedList (no HashSet/List/arrays).
    public class SimpleSet<T>
    {
        private readonly SinglyLinkedList<T> _items = new();
        public bool Add(T value)
        {
            if (Contains(value)) return false;
            _items.AddLast(value);
            return true;
        }
        public bool Contains(T value)
        {
            var cmp = EqualityComparer<T>.Default;
            foreach (var x in _items) if (cmp.Equals(x, value)) return true;
            return false;
        }
        public IEnumerable<T> Items() => _items;
    }
}
