using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    [Table("ChequeBookSeries")]
    public class ChequeBookSeriesDTO : BaseEntity
    {
        [Required]
        public int BranchId { get; set; }

        [NotMapped]
        public string BranchName { get; set; }

        [Required]
        public int AccountTypeId { get; set; }

        [NotMapped]
        public string AccountType { get; set; }

        [Required]
        public int StartChequeNumber { get; set; }

        [Required]
        public int EndChequeNumber { get; set; }

        [Required]
        public int LastChequeNumber { get; set; }

        [Required]
        public int AvailableCheques { get; set; }

        public override bool IsValid(IList<ValidationResult> results)
        {
            base.IsValid(results);
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<ChequeBookSeriesDTO>(context);
                var branch = repository.FindBy(f => f.BranchId == this.BranchId && f.AccountTypeId == this.AccountTypeId && f.Id != this.Id).FirstOrDefault();
                if (branch != null)
                    results.Add(new ValidationResult("Check book series for this Branch & Account Type combination already exists!"));
            }

            return results.Count == 0;
        }
    }
}
