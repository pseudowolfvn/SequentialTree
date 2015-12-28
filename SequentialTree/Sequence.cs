using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    public class Sequence
    {
        List<Formula> formulas;
        public List<Formula> Formulas { get { return formulas; } }
        Example counterExample;
        public Example CounterExample
        {
            get
            {
                if (counterExample == null && (!IsAtomic() || IsClosed()))
                {
                    foreach (var formula in formulas)
                    {
                        Predicate p = formula as Predicate;
                        if (p != null) counterExample.Add(p);
                    }
                }
                return counterExample;
            }
        }
        public Sequence(params Formula[] formulas)
        {
            this.formulas = formulas.ToList();
        }
        public Sequence(Sequence sequence)
        {
            this.formulas = new List<Formula>(sequence.formulas);
        }
        public bool IsClosed()
        {
            HashSet<Predicate> predicates = new HashSet<Predicate>();
            foreach (var formula in formulas)
            {
                Predicate P = formula as Predicate;
                if (P != null)
                {
                    Predicate notP = P.Clone() as Predicate;
                    notP.Value = Operator.Not(P.Value);
                    if (predicates.Contains(notP)) return true;
                    predicates.Add(P);
                }
            }
            return false;
        }
        public bool IsAtomic() // only predicates are atomic
        {
            foreach (var formula in formulas)
            {
                Predicate P = formula as Predicate;
                if (P == null) return false;
            }
            return true;
        }
        public Formula GetFirstExpandable() // return first non-predicate formula
        {
            foreach (var formula in formulas)
            {
                Operator op = formula as Operator;
                if (op != null) return op;
            }
            return null;
        }
        public Sequence[] Expand()
        {
            Operator formula = this.GetFirstExpandable() as Operator;
            Sequence sigma = this.Clone();
            sigma.formulas.Remove(formula);
            Sequence[] sequences = ExpandRules.Expand(formula, sigma);
            foreach (var sequence in sequences)
                sequence.formulas = sequence.formulas.Distinct().ToList();
             return sequences;
        }
        public HashSet<string> FreeVarNames()
        {
            HashSet<string> names = new HashSet<string>();
            foreach (var formula in formulas)
                foreach (var name in formula.FreeVarNames())
                    names.Add(name);
            return names;
        }
        public Sequence Clone()
        {
            return new Sequence(this);
        }
        public override string ToString()
        {
            string sequence = "";
            foreach (var formula in formulas)
            {
                if (formula.Value == LogicalValue.True) sequence += "+";
                else if (formula.Value == LogicalValue.False) sequence += "-";
                sequence += formula.ToString();
                sequence += ",";
            }
            return sequence;
        }
    }
}
