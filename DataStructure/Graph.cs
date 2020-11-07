using System.Collections.Generic;
using VectorLibrary;

namespace TreeLibrary
{
    public class TreeNode<T>
    {
        public T data;
        public List<TreeNode<T>> children, parents;

        public TreeNode()
        {
            children = new List<TreeNode<T>>();
            parents = new List<TreeNode<T>>();
        }
        public void addChild(TreeNode<T> _node)
        {
            children.Add(_node);
        }
        public void addParent(TreeNode<T> _node)
        {
            parents.Add(_node);
        }
    }

    public class Graph<T>
    {
        Vector<T> vector;

        protected List<TreeNode<T>> roots;

        public Graph(List<T> _data, List<Edge<T>> _edges, equals _equals)
        {
            vector = new Vector<T>(_data, _edges, _equals, DataStructure.Compare.LESS);
        }

        public Graph(Vector<T> _vector)
        {
            vector = _vector;
        }

        public void build()
        {
            roots = new List<TreeNode<T>>();

            int idx = 0;

            // set the forest tree roots
            while (idx < vector.nodes.Count && vector.nodes[idx].depth == 1)
            {
                roots.Add(new TreeNode<T>() { data = vector.nodes[idx].data });
                ++idx;
            }
            // build the forest trees
            while (idx < vector.nodes.Count)
            {
                T data = vector.nodes[idx].data;
                List<T> par = vector.parents(data);
                TreeNode<T> n = new TreeNode<T>();
                n.data = data;
                foreach (T p in par)
                {
                    TreeNode<T> _ = getNode(p);
                    _.addChild(n);
                    n.parents.Add(_);
                }
                ++idx;
            }
            return;
        }

        protected TreeNode<T> getNode(T _data)
        {
            foreach (TreeNode<T> root in roots)
            {
                TreeNode<T> node = getNode(_data, root);
                if (node != null)
                    return node;
            }
            return null;
        }

        protected TreeNode<T> getNode(T _data, TreeNode<T> _root)
        {
            if (vector.eq(_root.data, _data))
                return _root;

            if (_root.children.Count == 0)
                return null;

            foreach (TreeNode<T> node in _root.children)
            {
                if (vector.eq(node.data, _data))
                    return node;

                TreeNode<T> foundInTree = getNode(_data, node);

                if (foundInTree != null)
                    return foundInTree;
            }

            return null;
        }

        private bool directAccess(TreeNode<T> _parent, T _child)
        {
            if (_parent.children.Count == 0)
                return false;
            foreach (TreeNode<T> node in _parent.children)
            {
                if (vector.eq(node.data, _child))
                    return true;
                else
                {
                    bool found = directAccess(node, _child);
                    if (found)
                        return true;
                }
            }
            return false;
        }

        public bool directAccess(T _child, T _parent)
        {
            TreeNode<T> node = getNode(_parent);
            return node != null && directAccess(node, _child);
        }
    }
}
