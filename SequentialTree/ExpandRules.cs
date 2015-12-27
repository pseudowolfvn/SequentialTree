using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    static class ExpandRules
    {
        static public Sequence[] Expand(Operator op, Sequence sigma)
        {
            if (op.Type == LogicalOp.And)
            {
                if (op.Value == LogicalValue.True)
                {
                    Sequence result = new Sequence(sigma);
                    Formula newRight = op.Right.Clone();
                    newRight.Value = LogicalValue.True;
                    result.Formulas.Insert(0, newRight);
                    Formula newLeft = op.Left.Clone();
                    newLeft.Value = LogicalValue.True;
                    result.Formulas.Insert(0, newLeft);
                    return new Sequence[] { result };
                }
                else
                {
                    Sequence firstBranch = new Sequence(sigma);
                    Formula newLeft = op.Left.Clone();
                    newLeft.Value = LogicalValue.False;
                    firstBranch.Formulas.Insert(0, newLeft);
                    Sequence secondBranch = new Sequence(sigma);
                    Formula newRight = op.Right.Clone();
                    newRight.Value = LogicalValue.False;
                    secondBranch.Formulas.Insert(0, newRight);
                    return new Sequence[] { firstBranch, secondBranch };
                }
            }
            else if (op.Type == LogicalOp.Or)
            {
                if (op.Value == LogicalValue.True)
                {
                    Sequence firstBranch = new Sequence(sigma);
                    Formula newLeft = op.Left.Clone();
                    newLeft.Value = LogicalValue.True;
                    firstBranch.Formulas.Insert(0, newLeft);
                    Sequence secondBranch = new Sequence(sigma);
                    Formula newRight = op.Right.Clone();
                    newRight.Value = LogicalValue.True;
                    secondBranch.Formulas.Insert(0, newRight);
                    return new Sequence[] { firstBranch, secondBranch };
                }
                else
                {
                    Sequence result = new Sequence(sigma);
                    Formula newLeft = op.Left.Clone();
                    newLeft.Value = LogicalValue.False;
                    result.Formulas.Insert(0, newLeft);
                    Formula newRight = op.Right.Clone();
                    newRight.Value = LogicalValue.False;
                    result.Formulas.Insert(0, newRight);
                    return new Sequence[] { result };
                }
            }
            else if (op.Type == LogicalOp.Implication)
            {
                if (op.Value == LogicalValue.True)
                {
                    Sequence firstBranch = new Sequence(sigma);
                    Formula newLeft = op.Left.Clone();
                    newLeft.Value = LogicalValue.False;
                    firstBranch.Formulas.Insert(0, newLeft);
                    Sequence secondBranch = new Sequence(sigma);
                    Formula newRight = op.Right.Clone();
                    newRight.Value = LogicalValue.True;
                    secondBranch.Formulas.Insert(0, newRight);
                    return new Sequence[] { firstBranch, secondBranch };
                }
                else
                {
                    Sequence result = new Sequence(sigma);
                    Formula newRight = op.Right.Clone();
                    newRight.Value = LogicalValue.False;
                    result.Formulas.Insert(0, newRight);
                    Formula newLeft = op.Left.Clone();
                    newLeft.Value = LogicalValue.True;
                    result.Formulas.Insert(0, newLeft);
                    return new Sequence[] { result };
                }
            }
            else if (op.Type == LogicalOp.Not)
            {
                Sequence result = new Sequence(sigma);
                Formula newRight = op.Right.Clone();
                if (newRight.Value == LogicalValue.False) newRight.Value = LogicalValue.True;
                else if (newRight.Value == LogicalValue.True) newRight.Value = LogicalValue.False;
                result.Formulas.Insert(0, newRight);
                return new Sequence[] { result };
            }
            else if (op.Type == LogicalOp.Exists)
            {
                if (op.Value == LogicalValue.True)
                {
                    Sequence result = new Sequence(sigma);
                    Formula newRight = op.Right.Clone();
                    newRight.Rename(op.VarName, VarNamesGenerator.nextVarName());
                    newRight.Value = LogicalValue.True;
                    result.Formulas.Insert(0, newRight);
                    return new Sequence[] { result };
                }
                else
                {
                    Sequence result = new Sequence(sigma);
                    result.Formulas.Add(op.Clone());
                    HashSet<string> freeVarNames = new HashSet<string>(sigma.FreeVarNames());
                    foreach (var var in op.FreeVarNames())
                        freeVarNames.Add(var);
                    foreach (var freeVarName in freeVarNames)
                    {
                        Formula formula = op.Right.Clone();
                        formula.Rename(op.VarName, freeVarName);
                        formula.Value = LogicalValue.False;
                        result.Formulas.Insert(0, formula);
                    }
                    return new Sequence[] { result };
                }
            }
            else if (op.Type == LogicalOp.ForAll)
            {
                if (op.Value == LogicalValue.True)
                {
                    Sequence result = new Sequence(sigma);
                    result.Formulas.Add(op.Clone());
                    HashSet<string> freeVarNames = new HashSet<string>(sigma.FreeVarNames());
                    foreach (var var in op.FreeVarNames())
                        freeVarNames.Add(var);
                    foreach (var freeVarName in freeVarNames)
                    {
                        Formula formula = op.Right.Clone();
                        formula.Rename(op.VarName, freeVarName);
                        formula.Value = LogicalValue.True;
                        result.Formulas.Insert(0, formula);
                    }
                    return new Sequence[] { result };
                }
                else
                {
                    Sequence result = new Sequence(sigma);
                    Formula newRight = op.Right.Clone();
                    newRight.Rename(op.VarName, VarNamesGenerator.nextVarName());
                    newRight.Value = LogicalValue.False;
                    result.Formulas.Insert(0, newRight);
                    return new Sequence[] { result };
                }
            }
            else return null;
        }
    }
}
