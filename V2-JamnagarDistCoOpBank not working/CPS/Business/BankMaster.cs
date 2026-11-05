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
    [Table("BankMaster")]
    public class BankMasterDTO : BaseEntity
    {
        [Required, RegularExpression(@"^\d*$", ErrorMessage = "Invalid Code. (Number only)")]
        public string Code { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string AddressLine1 { get; set; }

        [MaxLength(100)]
        public string AddressLine2 { get; set; }

        [MaxLength(100)]
        public string AddressLine3 { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(100)]
        public string State { get; set; }

        [Required, MaxLength(100)]
        public string Country { get; set; }

        [Required, RegularExpression(@"^\d*$", ErrorMessage = "Invalid Postal Code. (Numbers only)")]
        public string PostalCode { get; set; }

        [MaxLength(20), RegularExpression(@"^\d*$", ErrorMessage = "Invalid Phone. (Numbers only)")]
        public string Phone { get; set; }

        [MaxLength(20), RegularExpression(@"^\d*$", ErrorMessage = "Invalid Mobile. (Numbers only)")]
        public string Mobile { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }

        [MaxLength(20), RegularExpression(@"^\d*$", ErrorMessage = "Invalid Fax. (Numbers only)")]
        public string Fax { get; set; }

        [MaxLength(100)]
        public string WebAddress { get; set; }

        public static string GetBankName()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<BankMasterDTO>(context);
                return repository.GetAll().FirstOrDefault()?.Name;
            }
        }
    }
}
