using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    public class LogicalExpr : Expression
    {
        void skipBlanks()
        {
            while (!endOfExpr() && Char.IsWhiteSpace(expression[position])) position++;
        }
        void readLexem()
        {
            prevLexem = currLexem;
            if (!endOfExpr())
            {
                switch (expression[position])
                {
                    case '#': case '*':
                        currLexem = new Lexem(LexemType.Quantifier, position, expression[position]);
                    break;
                    case '(':
                        if (prevLexem != null && prevLexem.Type == LexemType.Predicate)
                        {
                            ++argumentBrackets;
                            currLexem = new Lexem(LexemType.OpenArgsBracket, position);
                        }
                        else currLexem = new Lexem(LexemType.OpenBracket, position);
                    break;
                    case ')':
                        if (prevLexem != null && prevLexem.Type == LexemType.Var)
                        {
                            --argumentBrackets;
                            currLexem = new Lexem(LexemType.ClosingArgsBracket, position);
                        }
                        else currLexem = new Lexem(LexemType.ClosingBracket, position);
                    break;
                    case ',':
                        if (prevLexem.Type == LexemType.Var) currLexem = new Lexem(LexemType.Comma, position);
                        else throw new BadArgs(position);
                    break;
                    case '|': case '&': case '-': case '=':
                        string opName = expression[position].ToString(); 
                        if (prevLexem == null
                                || (prevLexem.Type != LexemType.ClosingBracket
                                    && prevLexem.Type != LexemType.ClosingArgsBracket))
                            throw (new BadOperator(position));
                        else if (expression[position] == '-')
                        {
                            ++position;
                            if (endOfExpr() || expression[position] != '>') throw (new BadOperator(position));
                            opName += expression[position];
                        }
                        currLexem = new Lexem(LexemType.Binary, position, opName);
                    break;
                    case '!':
                        currLexem = new Lexem(LexemType.Unary, position, expression[position]);
                    break;
                    default:
                        if (Char.IsLower(expression[position]))
                            currLexem = new Lexem(LexemType.Var, position, expression[position]);
                        else if (Char.IsUpper(expression[position]))
                            currLexem = new Lexem(LexemType.Predicate, position, expression[position]);
                        else throw new BadLexem(position);
                    break;
                }
                ++position;
            }   
        }
        bool endOfExpr()
        {
            return position == expression.Length;
        }
        public LogicalExpr(string str)
        {
            expression = str;
        }
        public override Lexem NextLexem
        {
            get
            {
                skipBlanks();
                if (!endOfExpr())
                {
                    readLexem();
                    switch (currLexem.Type)
                    {
                        case LexemType.OpenBracket:
                            openedBrackets++;
                            break;
                        case LexemType.ClosingBracket:
                            openedBrackets--;
                            if (openedBrackets < 0) throw (new BadBracket(position - 1));
                            break;
                    }
                    return currLexem;
                }
                else return null;
            }
        }
    }
}
