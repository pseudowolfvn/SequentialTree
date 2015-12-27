using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    static public class VarNamesGenerator
    {
        static HashSet<string> usedVarNames = new HashSet<string>();
        static StringBuilder currentVarName = new StringBuilder("a");
        static public string LastUsedVarName { get { return currentVarName.ToString(); } } 
        static public void AddUsedNames(HashSet<string> varNames)
        {
            foreach (var name in varNames)
                usedVarNames.Add(name);
        }
        static public string nextVarName()
        {
            while (usedVarNames.Contains(currentVarName.ToString()))
            {
                int index = currentVarName.Length - 1;
                while (index >= 0 && currentVarName[index] == 'z')
                    currentVarName[index--] = 'a';
                if (index < 0)
                    currentVarName.Append('a');
                else currentVarName[index] = Char.Parse(Char.ToString((char)(currentVarName[index] + 1)));
            }
            usedVarNames.Add(currentVarName.ToString());
            return currentVarName.ToString();
        }
    }
}
