using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    [Table("UserMaster")]
    public class UserMasterDTO : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string UserId { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public string Password { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? LockDate { get; set; }

        public string LockReason { get; set; }

        public override bool IsValid(IList<ValidationResult> results)
        {
            base.IsValid(results);
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<UserMasterDTO>(context);
                var branch = repository.FindBy(f => f.UserId == this.UserId && f.Id != this.Id).FirstOrDefault();
                if (branch != null)
                    results.Add(new ValidationResult("This user id has already been taken. Please try different user id."));
            }

            return results.Count == 0;
        }

        public static List<LookupItem<int, string>> GetLookups()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<UserMasterDTO>(context);
                return repository.GetAll().Where(o => o.UserId != "superadmin" && o.UserId != "admin").Select(o => new LookupItem<int, string> { Key = o.Id, Value = o.UserId }).ToList();
            }
        }
    }
}
