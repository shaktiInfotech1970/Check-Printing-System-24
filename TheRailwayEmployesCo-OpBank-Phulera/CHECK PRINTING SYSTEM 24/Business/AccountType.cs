using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    [Table("AccountType")]
    [Serializable]
    public class AccountTypeDTO : BaseEntity
    {
        [Required]
        public int Code { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// returns list of id,name (MongoDb auto generated id)
        /// </summary>
        /// <returns></returns>
        public static List<LookupItem<int, string>> GetLookups()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<AccountTypeDTO>(context);
                return repository.GetAll().Select(s => new LookupItem<int, string> { Key = s.Id, Value = s.Name }).ToList();
            }
        }

        /// <summary>
        /// returns list of code,name
        /// </summary>
        /// <returns></returns>
        public static List<LookupItem<int, string>> GetLookups2()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<AccountTypeDTO>(context);
                return repository.GetAll().Select(s => new LookupItem<int, string> { Key = s.Code, Value = s.Name }).ToList();
            }
        }

        public override bool IsValid(IList<ValidationResult> results)
        {
            base.IsValid(results);
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<AccountTypeDTO>(context);
                var branch = repository.FindBy(f => f.Code == this.Code && f.Id != this.Id).FirstOrDefault();
                if (branch != null)
                    results.Add(new ValidationResult("Account code already exists!"));
            }

            return results.Count == 0;
        }
    }
}
