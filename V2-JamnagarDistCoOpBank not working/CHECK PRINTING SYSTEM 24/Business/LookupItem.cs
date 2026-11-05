using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    public class LookupItem<Tkey, TValue>
    {
        public Tkey Key { get; set; }
        public TValue Value { get; set; }
    }

    public class LookupItem<TOperator, TOperand, TValue>
    {
        public TOperator Operator { get; set; }
        public TOperand Operand { get; set; }
        public TValue Value { get; set; }
    }

    public enum Operator
    {
        And,
        OR
    }
}
