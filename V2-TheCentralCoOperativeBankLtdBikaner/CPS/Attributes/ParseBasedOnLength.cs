using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS.Attributes
{
    public class ParseBasedOnLengthAttribute : Attribute
    {

        public ParseBasedOnLengthAttribute(int start, int end)
        {
            Start = start;
            End = end;
        }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
