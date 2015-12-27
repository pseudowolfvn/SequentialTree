using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    abstract public class Formula
    {
        protected LogicalValue value = LogicalValue.Undetermined;
        protected string name;
        public string Name { get { return name; } }
        public LogicalValue Value { get { return value; } set { this.value = value; } }
        abstract public void Rename(string oldName, string newName);
        abstract public HashSet<string> FreeVarNames();
        abstract public HashSet<string> VarNames();
        abstract public Formula Clone();
    }
}
