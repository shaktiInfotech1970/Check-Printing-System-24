using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    public enum PrintType
    {
        ChequeBook = 1,
        RePrintChequeBook = 2,
        RePrintCheque = 3,
        Request = 4,
        RePrintRequest = 5
    }

    [Table("PrintHistory")]
    public class PrintHistoryDTO : BaseEntity
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public PrintType PrintType { get; set; }

        [Required]
        public int ChequeNoFrom { get; set; }

        [Required]
        public int ChequeNoTo { get; set; }
    }
}
