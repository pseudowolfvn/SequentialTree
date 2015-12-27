using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialTree
{
    static public class StringToFormula
    {
        static public Formula Parse(string expression)
        {
            var rpn = new RPNConverter<LogicalExpr>(new LogicalExpr(expression));
            rpn.Convert();
            return rpn.Instantiate();
        }
    }
}
