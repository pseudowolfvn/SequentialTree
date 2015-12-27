using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    public class Predicate : Formula
    {
        readonly int PrimeHash = 31;
        List<string> args;
        public List<string> Args { get { return args; } }
        public Predicate(string name, List<string> args, LogicalValue value = LogicalValue.Undetermined)
        {
            this.name = name;
            this.args = args;
            this.value = value;
        }
        public override void Rename(string oldName, string newName)
        {
            int index = args.IndexOf(oldName);
            if (index != -1) args[index] = newName;
        }
        public override HashSet<string> FreeVarNames()
        {
            return new HashSet<string>(args);
        }
        public override HashSet<string> VarNames()
        {
            return FreeVarNames();
        }
        public override bool Equals(object obj)
        {
            Predicate p = (obj as Predicate);
            if (this == p) return true;
            if (p == null) return false;
            return this.name.Equals(p.name)
                && this.args.SequenceEqual(p.args)
                && this.value.Equals(p.value);
        }
        public override int GetHashCode()
        {
            int result = name.GetHashCode();
            foreach (var arg in args)
                result = PrimeHash * result + arg.GetHashCode();
            result = PrimeHash * result + value.GetHashCode();
            return result;
        }
        public override string ToString()
        {
            StringBuilder arguments = new StringBuilder(args.Count);
            foreach (var arg in args)
                arguments.Append(arg);
            return name + "(" + arguments.ToString() + ")";
        }
        public override Formula Clone()
        {
            return new Predicate(this.name, new List<string>(this.args), this.value);
        }
    }
}
