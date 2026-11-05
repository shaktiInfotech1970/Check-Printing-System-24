using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS.Attributes
{
    public class ParseAttribute : Attribute
    {

        public ParseAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }

}
