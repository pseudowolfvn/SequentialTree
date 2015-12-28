using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    public enum LogicalOp { Not, Or, And, Implication, ForAll, Exists };
    public enum LogicalValue { Undetermined, True, False };
    public class Operator : Formula
    {
        protected Formula left = null, right = null;
        LogicalOp opType;
        string varName = "";
        public string VarName { get { return varName; } }
        public LogicalOp Type { get { return opType; } }
        public Formula Left { get { return left; } }
        public Formula Right { get { return right; } }
        public bool IsUnary()
        {
            if (left == null) return true;
            else return false;
        }
        public Operator(string str, Formula first, Formula second, LogicalValue value = LogicalValue.Undetermined)
        {
            if (str == "->" || str == "=") opType = LogicalOp.Implication;
            else if (str == "&") opType = LogicalOp.And;
            else if (str == "|") opType = LogicalOp.Or;
            else if (str == "!") opType = LogicalOp.Not;
            else if (str[0] == '#') opType = LogicalOp.Exists;
            else if (str[0] == '*') opType = LogicalOp.ForAll;
            
            if (str[0] == '#' || str[0] == '*')
                varName = str.Remove(0, 1);
            if (str == "=") str = "->";
            base.name = str;
            
            left = first;
            right = second;

            this.value = value;
        }
        public Operator(string str, Formula first, LogicalValue value = LogicalValue.Undetermined) : this(str, null, first, value)
        {
        }
        static public int PriorityOf(LogicalOp op)
        {
            int prior = 0;
            switch (op)
            {
                case LogicalOp.Implication:
                    prior = 1;
                break;
                case LogicalOp.Or:
                    prior = 2;
                break;
                case LogicalOp.And:
                    prior = 3;
                break;
                case LogicalOp.Not:
                case LogicalOp.ForAll:
                case LogicalOp.Exists:
                    prior = 4;
                break;
            }
            return prior;
        }
        static public int PriorityOf(string str)
        {
            LogicalOp opType;
            if (str == "->") opType = LogicalOp.Implication;
            else if (str == "&") opType = LogicalOp.And;
            else if (str == "|") opType = LogicalOp.Or;
            else if (str == "!") opType = LogicalOp.Not;
            else if (str[0] == '#') opType = LogicalOp.Exists;
            else if (str[0] == '*') opType = LogicalOp.ForAll;
            else return 0;
            return Operator.PriorityOf(opType);
        }
        static public LogicalValue Not(LogicalValue value)
        {
            if (value == LogicalValue.False) value = LogicalValue.True;
            else if (value == LogicalValue.True) value = LogicalValue.False;
            return value;
        }
        static public bool IsRightAssociative(string name)
        {
            if (name == "->" || name == "=") return true;
            else return false;
        }
        public override void Rename(string oldName, string newName)
        {
            if (!IsUnary())
                left.Rename(oldName, newName);
            right.Rename(oldName, newName);
        }
        public override HashSet<string> FreeVarNames()
        {
            HashSet<string> vars = new HashSet<string>();
            if (!IsUnary())
                foreach (var var in left.FreeVarNames())
                    vars.Add(var);
            foreach (var var in right.FreeVarNames())
                vars.Add(var);
            if (vars.Contains(varName)) vars.Remove(varName);
            return vars;
        }
        public override HashSet<string> VarNames()
        {
            HashSet<string> vars = new HashSet<string>(left.VarNames());
            if (!IsUnary())
                foreach (var var in right.VarNames())
                    vars.Add(var);
            return vars;
        }
        public override string ToString()
        {
            return (!this.IsUnary() ? left.ToString() : "") + base.name + right.ToString();
        }
        public override Formula Clone()
        {
            if (this.IsUnary())
                return new Operator(base.name, Right.Clone(), this.value);
            else
                return new Operator(base.name, Left.Clone(), Right.Clone(), this.value);
        }
    }
}
