using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuissANT
{
    public class PriorityQueue<T>
    {
        private readonly LinkedList<Tuple<int, T>> _queue;

        public PriorityQueue()
        {
            _queue = new LinkedList<Tuple<int, T>>();
        }
        
        public void Enqueue(int priorty, T value)
        {
            if (IsEmpty())
            {
                _queue.AddFirst(new Tuple<int, T>(priorty, value));
            }
            else
            {
                LinkedListNode<Tuple<int, T>> node = _queue.First;
                while (node.Next != null && priorty >= node.Value.Item1)
                {
                    node = node.Next;
                }
                _queue.AddAfter(node, new Tuple<int, T>(priorty, value));
            }
        }

        public T Dequeue()
        {
            LinkedListNode<Tuple<int, T>> node = _queue.First;
            _queue.RemoveFirst();
            return node.Value.Item2;
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public T Peek()
        {
            return _queue.First.Value.Item2;
        }

        public bool ContainsValue(T value)
        {
            return _queue.Any(n => n.Item2.Equals(value));
        }

        public bool IsEmpty()
        {
            return _queue.First == null;
        }
    }
}