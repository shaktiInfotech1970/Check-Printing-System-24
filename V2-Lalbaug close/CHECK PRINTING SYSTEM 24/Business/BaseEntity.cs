using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{
    [Serializable]
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        [Required]
        public DateTime UpdatedOn { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public virtual bool IsValid(IList<ValidationResult> results)
        {
            var context = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, context, results, true);

            if (results == null || results.Count == 0) return true;
            return false;
        }
    }
}
