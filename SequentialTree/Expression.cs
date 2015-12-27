using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    abstract public class Expression
    {
        protected string expression;
        protected int position;
        protected int openedBrackets;
        protected int argumentBrackets;
        protected Lexem prevLexem;
        protected Lexem currLexem;
        public int OpenedBrackets { get { return openedBrackets; } }
        protected int ArgumentBrackets { get { return argumentBrackets; } }
        public Lexem CurrLexem { get { return CurrLexem; } }
        abstract public Lexem NextLexem { get; }
    }
    
}
