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


        public static PrintHistoryDTO GetPrintHistory(int PrintJobNo, string AccountNoFull)
        {
            using (var context = new CPSDbContext())
            {
                var repositoryRequest = new PersistenceBase<RequestDTO>(context);
                var repositoryPrintHistory = new PersistenceBase<PrintHistoryDTO>(context);

                var query = (from ph in repositoryPrintHistory.GetAll()
                             join r in repositoryRequest.GetAll() on ph.RequestId equals r.Id
                             where r.PrintJobNo == PrintJobNo && r.AccountNoFull == AccountNoFull && ph.PrintType == PrintType.ChequeBook
                             select ph
                             ).OrderBy(o => o.CreatedOn);
                return query.FirstOrDefault();
            }
        }

    }
}
