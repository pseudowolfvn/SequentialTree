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
        Dictionary<string, Dictionary<List<string>, LogicalValue>> counterExample;
        public Example()
        {
            delta = new Dictionary<string, string>();
            counterExample = new Dictionary<string, Dictionary<List<string>, LogicalValue>>();
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
            if (!counterExample.ContainsKey(p.Name))
                counterExample.Add(p.Name, new Dictionary<List<string>, LogicalValue>());
            counterExample[p.Name].Add(newVars, p.Value);
        }
    }
}
