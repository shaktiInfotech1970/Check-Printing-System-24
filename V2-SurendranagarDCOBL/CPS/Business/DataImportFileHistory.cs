using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Business
{   
    [Table("DataImportFileHistory")]
    public class DataImportFileHistoryDTO : BaseEntity
    {
        [Required]
        public string CheckSum { get; set; }        
    }
}
