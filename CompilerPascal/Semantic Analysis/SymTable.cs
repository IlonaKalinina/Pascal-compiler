﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CompilerPascal
{
    public class SymTable
    {
        Dictionary<string, Symbol> table;
        public Dictionary<string, Symbol> GetData()
        {
            return table;
        }
        public void Add(string name, Symbol type)
        {
            if (table.TryAdd(name, type))
            {
                return;
            }
            else
            {
                throw new Exception($"Duplicate identifier \"{name}\"");
            }
        }
        public Symbol Get(string name)
        {
            Symbol value;
            Symbol? check;
            if (table.TryGetValue(name, out check))
            {
                value = check;
                return value;
            }
            else
            {
                throw new Exception("Variable not declared");
            }
        }
        public SymTable(Dictionary<string, Symbol> table)
        {
            this.table = table;
        }
    }
    public class SymTableStack
    {
        List<SymTable> tables;
        public int GetCountTables()
        {
            return tables.Count;
        }
        public SymTable GetTable(int index)
        {
            return tables[index];
        }
        public SymTable GetBackTable()
        {
            return tables[^1];
        }
        public void AddTable(SymTable table)
        {
            tables.Add(table);
        }
        public void PopBack()
        {
            tables.RemoveAt(tables.Count - 1);
        }
        public void Check(string name)
        {
            if (GetBackTable().GetData().ContainsKey(name))
            {
                throw new Exception($"Duplicate identifier \"{name}\"");
            }
        }
        public void Add(string name, Symbol type)
        {
            try
            {
                GetBackTable().Add(name, type);
            }
            catch
            {
                throw new Exception($"Duplicate identifier \"{name}\"");
            }
        }
        public Symbol Get(string name)
        {
            Symbol res = new Symbol("");
            bool decl = false;
            for (int i = tables.Count - 1; i >= 0; i--)
            {
                try
                {
                    res = tables[i].Get(name);
                }
                catch
                {
                    continue;
                }
                finally
                {
                    decl = true;
                }
            }
            if (!decl)
            {
                throw new Exception("Variable not declared");
            }
            return res;
        }
        public SymTableStack()
        {
            tables = new List<SymTable>();
        }
    }
}
