using System.ComponentModel.DataAnnotations.Schema;

namespace CPS.Business
{
    [Table("DatabaseBackup")]
    public class DatabaseBackup : BaseEntity
    {
        public string Path { get; set; }
    }
}
