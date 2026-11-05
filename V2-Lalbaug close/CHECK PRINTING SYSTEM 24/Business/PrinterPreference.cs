using System.ComponentModel.DataAnnotations.Schema;

namespace CPS.Business
{
    [Table("PrinterPreference")]
    public class PrinterPreference : BaseEntity
    {
        public string Name { get; set; }
        public int RequestTray { get; set; }
        public int ChequeTray { get; set; }
    }
}
