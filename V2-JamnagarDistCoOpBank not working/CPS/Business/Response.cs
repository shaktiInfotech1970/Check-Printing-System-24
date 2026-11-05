using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    public class Response<T>
    {
        public Response() : this(default(T))
        {
        }

        public Response(T output)
        {
            IsSucceed = true;
            Output = output;
            ValidationResults = new List<ValidationResult>();
        }

        public bool IsSucceed { get; set; }
        public T Output { get; set; }
        public IList<ValidationResult> ValidationResults { get; set; }
    }
}
