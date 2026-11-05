using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CPS.Business
{
    [Table("RequestLayout")]
    public class RequestLayout
    {
        [Key]
        public int Id { get; set; }

        public bool branchAddress1Visble { get; set; }
        public float branchAddress1X { get; set; }
        public float branchAddress1Y { get; set; }

        public bool branchAddress2Visble { get; set; }
        public float branchAddress2X { get; set; }
        public float branchAddress2Y { get; set; }

        public bool chequeFrom1Visble { get; set; }
        public float chequeFrom1X { get; set; }
        public float chequeFrom1Y { get; set; }

        public bool chequeTo1Visble { get; set; }
        public float chequeTo1X { get; set; }
        public float chequeTo1Y { get; set; }

        public bool chequeFrom2Visble { get; set; }
        public float chequeFrom2X { get; set; }
        public float chequeFrom2Y { get; set; }

        public bool chequeTo2Visble { get; set; }
        public float chequeTo2X { get; set; }
        public float chequeTo2Y { get; set; }

        public bool nameAddress1Visble { get; set; }
        public float nameAddress1X { get; set; }
        public float nameAddress1Y { get; set; }

        public bool nameAddress2Visble { get; set; }
        public float nameAddress2X { get; set; }
        public float nameAddress2Y { get; set; }

        public bool accountNo1Visble { get; set; }
        public float accountNo1X { get; set; }
        public float accountNo1Y { get; set; }

        public bool accountNo2Visble { get; set; }
        public float accountNo2X { get; set; }
        public float accountNo2Y { get; set; }

        public bool barcode1Visble { get; set; }
        public float barcode1X { get; set; }
        public float barcode1Y { get; set; }

        public bool barcode2Visble { get; set; }
        public float barcode2X { get; set; }
        public float barcode2Y { get; set; }

        public bool audiText1Visble { get; set; }
        public float audiText1X { get; set; }
        public float audiText1Y { get; set; }

        public bool audiText2Visble { get; set; }
        public float audiText2X { get; set; }
        public float audiText2Y { get; set; }
    }
}
