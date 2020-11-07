using System.Collections.Generic;

namespace VectorLibrary
{
    public delegate bool equals(object _x, object _y);
    public delegate DataStructure.Compare compare(object _x, object _y);

    public class Node<T>
    {
        public T data;
        public short depth;
    }

    public class Edge<T>
    {
        public T parent, child;
        public Edge()
        {
        }
        public Edge(T _parent, T _child) : this()
        {
            parent = _parent;
            child = _child;
        }
    }

    public class Vector<T>
    {
        public equals eq;
        public List<Node<T>> nodes;
        public List<Edge<T>> edges;
        public static DataStructure.Compare compareDepth(object _1, object _2)
        {
            if (((Node<T>)_1).depth < ((Node<T>)_2).depth)
                return DataStructure.Compare.LESS;
            if (((Node<T>)_1).depth > ((Node<T>)_2).depth)
                return DataStructure.Compare.GREATER;
            return DataStructure.Compare.EQUAL;
        }
        public Vector(List<T> _data, List<Edge<T>> _edges, equals _equals, DataStructure.Compare _sort)
        {
            eq = _equals;
            initNodes(_data);
            edges = _edges;
            foreach (Edge<T> edge in edges)
            {
                int parentIdx = position(edge.parent);
                int childIdx = position(edge.child);

                if (nodes[parentIdx].depth >= nodes[childIdx].depth)
                    dipSubTree(childIdx, (short)(1 + nodes[parentIdx].depth));
            }

            DataStructure.Sort.selectionSort<Node<T>>(nodes, _sort, compareDepth);
        }
        public List<T> parents(T _data)
        {
            List<T> list = new List<T>();
            foreach (Edge<T> edge in edges)
            {
                if (eq(edge.child, _data))
                {
                    list.Add(edge.parent);
                }
            }
            return list;
        }
        public List<T> children(T _data)
        {
            List<T> list = new List<T>();
            foreach (Edge<T> edge in edges)
            {
                if (eq(edge.parent, _data))
                {
                    list.Add(edge.child);
                }
            }
            return list;
        }
        private void initNodes(List<T> _data)
        {
            nodes = new List<Node<T>>();
            foreach (T data in _data)
                nodes.Add(new Node<T>() { data = data, depth = 1 });
        }
        private int position(T _data)
        {
            int idx = 0;
            while (idx < nodes.Count)
            {
                if (eq(_data, nodes[idx].data))
                    break;
                ++idx;
            }
            return idx < nodes.Count ? idx : -1;
        }

        private void dipSubTree(int _idx, short _depth)
        {
            nodes[_idx].depth = _depth;

            foreach (Edge<T> edge in edges)
            {
                if (eq(edge.parent, nodes[_idx].data))
                {
                    int positionId = position(edge.child);
                    if (nodes[positionId].depth <= _depth)
                        dipSubTree(positionId, (short)(1 + _depth));
                }
            }
        }

    }
}