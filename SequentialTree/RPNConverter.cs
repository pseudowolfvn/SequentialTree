using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    class RPNEntity
    {
        string name;
        LexemType type;
        public string Name { get { return name; } set { name = value; } }
        public LexemType Type { get { return type; } }
        public RPNEntity(string name, LexemType type)
        {
            this.name = name;
            this.type = type;
        }
        public RPNEntity(Lexem lexem)
        {
            this.name = lexem.Value;
            this.type = lexem.Type;
        }
    }
    class RPNPredicate : RPNEntity
    {
        List<string> args;
        public List<string> Args { get { return args; } }
        public RPNPredicate(string name, List<string> args) : base(name, LexemType.Predicate)
        {
            this.args = args;
        }
    }
    class RPNOperator : RPNEntity
    {
        public RPNOperator(string name, LexemType type) : base(name, LexemType.Operator) { }
    }
    class RPNQuantifier : RPNEntity
    {
        public RPNQuantifier(string name) : base(name, LexemType.Quantifier) { }
    }
    class RPNConverter<ExprT> where ExprT : Expression 
    {
        ExprT expression;
        List<RPNEntity> rpn;
        Stack<RPNEntity> operations;
        public RPNConverter(ExprT expr)
        {
            expression = expr;
            operations = new Stack<RPNEntity>();
        }
        public void Convert()
        {
            Lexem current = null;
            List<RPNEntity> result = new List<RPNEntity>();
            while ((current = expression.NextLexem) != null)
            {
                switch (current.Type)
                {
                    case LexemType.Predicate:
                        string name = current.Value;
                        List<string> args = new List<string>();
                        if ((current = expression.NextLexem) == null 
                            || current.Type != LexemType.OpenArgsBracket)
                                throw new BadPredicate(current.Position);
                        if ((current = expression.NextLexem) == null 
                            || current.Type != LexemType.Var)
                                throw new BadArgs(current.Position);
                        args.Add(current.Value);
                        while ((current = expression.NextLexem) != null 
                            && current.Type == LexemType.Comma)
                        {
                            if ((current = expression.NextLexem) == null 
                                || current.Type != LexemType.Var)
                                    throw new BadArgs(current.Position);
                            args.Add(current.Value);
                        }
                        if (current == null || current.Type != LexemType.ClosingArgsBracket)
                                throw new BadArgs(current.Position);
                        result.Add(new RPNPredicate(name, args));
                    break;
                    case LexemType.Quantifier:
                        string symb = current.Value;
                        if ((current = expression.NextLexem) == null
                            || current.Type != LexemType.Var)
                            throw new BadQuant(current.Position);
                        operations.Push(new RPNQuantifier(symb + current.Value));
                    break;
                    case LexemType.Binary: case LexemType.Unary:
                        RPNEntity entity = new RPNEntity(current);
                        while (operations.Count != 0 
                            && operations.Peek().Type != LexemType.OpenBracket
                            && Operator.PriorityOf(operations.Peek().Name) >= Operator.PriorityOf(entity.Name))
                                result.Add(operations.Pop());
                        operations.Push(entity);
                        break;
                    case LexemType.OpenBracket:
                        operations.Push(new RPNEntity(current));
                    break;
                    case LexemType.ClosingBracket:
                        while (operations.Count != 0 && operations.Peek().Type != LexemType.OpenBracket)
                            result.Add(operations.Pop());
                        operations.Pop();
                    break;
                }
            }
            while (operations.Count != 0) result.Add(operations.Pop());
            this.rpn = result;
        }
        public Formula Instantiate()
        {
            Stack<Formula> stack = new Stack<Formula>();
            Formula result = null;
            foreach (var entity in rpn)
            {
                switch (entity.Type)
                {
                    case LexemType.Predicate:
                        RPNPredicate predicate = (RPNPredicate)entity;
                        stack.Push(new Predicate(predicate.Name, predicate.Args));
                    break;
                    case LexemType.Binary:
                        if (entity.Name == "=")
                            if (stack.Count < 2) break;
                        Formula left = stack.Pop(), right = stack.Pop();
                        if (!Operator.IsRightAssociative(entity.Name))
                            stack.Push(new Operator(entity.Name, left, right));
                        else stack.Push(new Operator(entity.Name, right, left));
                        break;
                    case LexemType.Unary: case LexemType.Quantifier:
                        stack.Push(new Operator(entity.Name, stack.Pop()));
                    break;

                }
            }
            result = stack.Pop();
            return result;
        }
    }
}
