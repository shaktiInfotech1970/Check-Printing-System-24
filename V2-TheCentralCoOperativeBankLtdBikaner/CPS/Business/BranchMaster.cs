using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    [Table("BranchMaster")]
    [Serializable]
    public class BranchMasterDTO : BaseEntity
    {
        [Required, RegularExpression(@"^\d*$", ErrorMessage = "Invalid Code. (Number only)")]
        public string Code { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string ShortName { get; set; }

        [Required]
        public string IFSC { get; set; }

        [Required, RegularExpression(@"^\d*$", ErrorMessage = "Invalid MICR. (Number only)")]
        public string MICR { get; set; }

        [Required, MaxLength(100)]
        public string AddressLine1 { get; set; }

        [MaxLength(100)]
        public string AddressLine2 { get; set; }

        [MaxLength(100)]
        public string AddressLine3 { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, RegularExpression(@"^\d*$", ErrorMessage = "Invalid Postal Code. (Numbers only)")]
        public string PostalCode { get; set; }

        [MaxLength(20), RegularExpression(@"^\d*$", ErrorMessage = "Invalid Telephone1. (Numbers only)")]
        public string Telephone1 { get; set; }

        [MaxLength(20), RegularExpression(@"^\d*$", ErrorMessage = "Invalid Telephone2. (Numbers only)")]
        public string Telephone2 { get; set; }

        [MaxLength(20), RegularExpression(@"^\d*$", ErrorMessage = "Invalid Mobile. (Numbers only)")]
        public string Mobile { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email2 address is not valid.")]
        public string Email2 { get; set; }

        [MaxLength(20), RegularExpression(@"^\d*$", ErrorMessage = "Invalid Fax. (Numbers only)")]
        public string Fax { get; set; }

        [MaxLength(100)]
        public string Time1 { get; set; }

        [MaxLength(100)]
        public string Time2 { get; set; }

        [MaxLength(100)]
        public string ImportPath { get; set; }

        [MaxLength(100)]
        public string ExportPath { get; set; }

        public static List<LookupItem<int, string>> GetLookups()
        {
            using(var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<BranchMasterDTO>(context);
                return repository.GetAll().Select(s => new LookupItem<int, string> { Key = s.Id, Value = s.Name }).ToList();
            }
        }
        public static BankMasterDTO GetBankInfo()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<BankMasterDTO>(context);
                return repository.GetAll().FirstOrDefault();
            }
        }

        public override bool IsValid(IList<ValidationResult> results)
        {
            base.IsValid(results);
            using(var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<BranchMasterDTO>(context);
                var branch = repository.FindBy(f => f.Code == this.Code && f.Id != this.Id).FirstOrDefault();
                if (branch != null)
                    results.Add(new ValidationResult("Branch with same code already exists!" ));
            }

            return results.Count == 0;
        }
    }
}
