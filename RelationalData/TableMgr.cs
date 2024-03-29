﻿using System.Collections.Generic;
using VectorLibrary;

namespace SharedClasses
{
    public class Store
    {
        public string tableName;
    }

    public class Relation
    {
        public string child, parent;
    }
    public interface IRelationalData
    {
        List<Store> tables();
        List<Relation> relations();
    }

    public static class TableMgr
    {
        public static bool equalStrings(object _1, object _2)
        {
            return (string)_1 == (string)_2;
        }

        //public static List<Node<string>> tablesByDependency(IRelationalData mgr, DataStructure.Compare order)
        //{
        //    List<Node<string>> result = new List<Node<string>>();

        //    List<Store> tables = mgr.tables();
        //    List<Relation> foreignKeys = mgr.relations();

        //    List<string> nodes = new List<string>();

        //    foreach (Store table in tables)
        //        nodes.Add(table.tableName);

        //    List<Edge<string>> edges = new List<Edge<string>>();

        //    foreach (Relation rec in foreignKeys)
        //        edges.Add(new Edge<string>(rec.parent, rec.child));

        //    Vector<string> v = new Vector<string>(nodes, edges, equalStrings, order);

        //    if (v.nodes.Count == 0)
        //        return result;

        //    foreach (Node<string> rec in v.nodes)
        //    {
        //        result.Add(rec);
        //    }

        //    return result;
        //}
    }
}

