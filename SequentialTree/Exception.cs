using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    public class Exception : SystemException
    {
        string info;
        protected Exception(string str, int pos)
        {
            info = str + (pos + 1).ToString() + ";";
        }
        protected Exception(string str)
        {
            info = str + ";";
        }
        public string Info { get { return info; } }
    }
    public class BadBracket : Exception
    {
        public BadBracket(int pos) : base("Bad bracket at position: ", pos) { }
    }

    public class BadOperator : Exception
    {
        public BadOperator(int pos) : base("Bad operator at position: ", pos) { }
    }
    public class BadQuant : Exception
    {
        public BadQuant(int pos) : base("Bad quantifier at position: ", pos) { }
    }
    public class BadArgs : Exception
    {
        public BadArgs(int pos) : base("Bad argument at position: ", pos) { }
    }
    public class BadLexem : Exception
    {
        public BadLexem(int pos) : base("Bad lexem at position: ", pos) { }
    }
    public class BadPredicate : Exception
    {
        public BadPredicate(int pos) : base("Bad predicate at position: ", pos) { }
    }
}
