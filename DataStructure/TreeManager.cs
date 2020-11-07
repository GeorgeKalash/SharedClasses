
using System.Collections.Generic;

namespace TreeStructure
{
    public class Edge<T>
    {
        public T childId;
        public T parentId;
        public short? depth;
    }

    public class TreeManager
    {
        public static DataStructure.Compare compareDepths<T>(object _edge1, object _edge2)
        {
            if (((Edge<T>)_edge1).depth < ((Edge<T>)_edge2).depth)
                return DataStructure.Compare.LESS;
            if (((Edge<T>)_edge1).depth > ((Edge<T>)_edge2).depth)
                return DataStructure.Compare.GREATER;
            return DataStructure.Compare.EQUAL;
        }

        public static bool isChildOf<T>(TreeNode<T> root, T _childObject, T _parentObject, DataStructure.equals _equals)
        {
            if (root == null)
                return false;

            TreeNode<T> currentNode = null;
            do
            {
                currentNode = root.FindTreeNode(node => node.Data != null && _equals(node.Data, _childObject));
                if (currentNode == null)
                    return false;
                if (_equals(currentNode.Data, _parentObject))
                    return true;
                _childObject = currentNode.Parent.Data;
            }
            while (_childObject != null);

            return false;
        }
        public static void assignChildrenDepthLevel<T>(List<Edge<T>> _list, T _parentId, short _depth, DataStructure.equals _equals)
        {
            for (int idx = 0; idx < _list.Count; idx++)
                if (_equals(_list[idx].parentId, _parentId))
                    (_list[idx]).depth = _depth;
        }

        public static void updateDepths<T>(List<Edge<T>> _list, DataStructure.equals _equals)
        {
            short currentDepthBuild = 1;
            bool hasNullDepthNodes;
            do
            {
                hasNullDepthNodes = false;
                for (int idx = 0; idx < _list.Count; idx++)
                {
                    if (_list[idx].depth == null)
                        hasNullDepthNodes = true;
                    else
                        if (_list[idx].depth == currentDepthBuild)
                        assignChildrenDepthLevel(_list, (T)_list[idx].childId, (short)(currentDepthBuild + 1), _equals);
                }
                ++currentDepthBuild;
            }
            while (hasNullDepthNodes == true);

            DataStructure.Sort.selectionSort(_list, DataStructure.Compare.LESS, compareDepths<T>);

            return;
        }
        public static TreeNode<T> buildTree<T>(List<Edge<T>> _edges, DataStructure.equals _equals)
        {
            TreeNode<T> root = new TreeNode<T>();

            int idx = 0;

            while (idx < _edges.Count && _edges[idx].parentId == null)
            {
                root.AddChild((T)_edges[idx].childId);
                ++idx;
            }

            while (idx < _edges.Count)
            {
                TreeNode<T> parentNode = root.FindTreeNode(node => node.Parent != null && _equals(node.Data, _edges[idx].parentId));
                parentNode.AddChild((T)_edges[idx].childId);
                ++idx;
            }

            return root;
        }

        public static TreeNode<T> getTreeRoot<T>(List<Edge<T>> _edges, DataStructure.equals _equals)
        {
            TreeManager.updateDepths<T>(_edges, _equals);
            return TreeManager.buildTree<T>(_edges, _equals);
        }
    }



}