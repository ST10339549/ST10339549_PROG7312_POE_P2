using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServiceApp.Domain.DataStructures
{
    // A tiny, hand-built singly linked list (no List<T>, no arrays).
    public class SinglyLinkedList<T> : IEnumerable<T>
    {
        private sealed class Node
        {
            public T Value;
            public Node? Next;
            public Node(T v) { Value = v; }
        }

        private Node? _head, _tail;
        public int Count { get; private set; }

        public void AddLast(T value)
        {
            var n = new Node(value);
            if (_head is null) _head = _tail = n;
            else { _tail!.Next = n; _tail = n; }
            Count++;
        }

        public T? FirstOrDefault(System.Func<T, bool> predicate)
        {
            foreach (var x in this) if (predicate(x)) return x;
            return default;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var cur = _head;
            while (cur is not null)
            {
                yield return cur.Value;
                cur = cur.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
