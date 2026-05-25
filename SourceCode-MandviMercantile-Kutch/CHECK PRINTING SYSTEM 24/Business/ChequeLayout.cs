using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CPS.Business
{
    [Table("ChequeLayout")]
    public class ChequeLayout
    {
        [Key]
        public int Id { get; set; }

        public bool branchAddressVisble { get; set; }
        public float branchAddressX { get; set; }
        public float branchAddressY { get; set; }

        public bool ifscVisble { get; set; }
        public float ifscX { get; set; }
        public float ifscY { get; set; }

        public bool orderOrBarerVisble { get; set; }
        public float orderOrBarerX { get; set; }
        public float orderOrBarerY { get; set; }

        public bool accountNoVisble { get; set; }
        public float accountNoX { get; set; }
        public float accountNoY { get; set; }

        public bool stampVisble { get; set; }
        public float stampX { get; set; }
        public float stampY { get; set; }

        public bool micrVisble { get; set; }
        public float micrX { get; set; }
        public float micrY { get; set; }

        public bool barcodeVisble { get; set; }
        public float barcodeX { get; set; }
        public float barcodeY { get; set; }

        public bool audiTextVisble { get; set; }
        public float audiTextX { get; set; }
        public float audiTextY { get; set; }

        public bool accountPayeeVisble { get; set; }
        public float accountPayeeX { get; set; }
        public float accountPayeeY { get; set; }
    }
}
