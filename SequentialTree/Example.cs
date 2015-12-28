using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    public class Example
    {
        Dictionary<string, string> delta;
        HashSet<Predicate> counterExample;
        public Example()
        {
            delta = new Dictionary<string, string>();
            counterExample = new HashSet<Predicate>();
        }
        public void Add(Predicate p)
        {
            List<string> newVars = new List<string>();
            foreach (string var in p.Args)
            {
                string newName = "";
                if (delta.ContainsKey(var))
                    newName = delta[var];
                else
                {
                    newName = VarNamesGenerator.nextVarName();
                    delta.Add(var, newName);
                }
                newVars.Add(newName);
            }
            Predicate exampleP = new Predicate(p.Name, newVars, p.Value);
            if (!counterExample.Contains(exampleP))
                counterExample.Add(exampleP);
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("ẟ = [");
            foreach (var pair in delta)
            {
                result.Append(pair.Key + " ↦ " + pair.Value + ", ");
            }
            result.Remove(result.Length - 2, 2);
            result.Append("], ");
            foreach (var p in counterExample)
            {
                result.Append(p.ToString());
                result.Append(" = " + p.Value.ToString() + ", ");
            }
            result.Remove(result.Length - 2, 2);
            result.Append(";\n");
            return result.ToString();
        }
    }
}
