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


        public static PrintHistoryDTO GetFirstPrint(string AccountNoFull, PrintType PrintType)
        {
            using (var context = new CPSDbContext())
            {
                var repositoryRequest = new PersistenceBase<RequestDTO>(context);
                var repositoryPrintHistory = new PersistenceBase<PrintHistoryDTO>(context);
                var query = (from r in repositoryRequest.GetAll()
                             join ph in repositoryPrintHistory.GetAll() on r.Id equals ph.RequestId
                             where r.AccountNoFull == AccountNoFull && ph.PrintType == PrintType
                             select ph
                             ).OrderBy(o => o.CreatedOn);
                return query.FirstOrDefault();
            }
        }
    }
}
