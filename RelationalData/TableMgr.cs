﻿using System.Collections.Generic;
using VectorLibrary;

namespace SharedClasses.RelationalData
{
    public struct Relation
    {
        public string child, parent;
    }
    public interface IRelationalData
    {
        List<string> tables();
        List<Relation> relations();
    }

    public static class TableMgr
    {
        public static bool equalStrings(object _1, object _2)
        {
            return ((string)_1).CompareTo((string)_2) == 0;
        }

        public static List<string> tablesByDependency(IRelationalData mgr, DataStructure.Compare order)
        {
            List<string> result = new List<string>();

            List<string> tables = mgr.tables();
            List<Relation> foreignKeys = mgr.relations();

            List<string> nodes = new List<string>();

            foreach (string table in tables)
                nodes.Add(table);

            List<Edge<string>> edges = new List<Edge<string>>();

            foreach (Relation rec in foreignKeys)
                edges.Add(new Edge<string>(rec.parent, rec.child));

            Vector<string> v = new Vector<string>(nodes, edges, equalStrings, order);

            if (v.nodes.Count == 0)
                return result;

            string logCmd = string.Empty;
            foreach (Node<string> rec in v.nodes)
            {
                result.Add(rec.data);
            }

            return result;
        }
    }
}

