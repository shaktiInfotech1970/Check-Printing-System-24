using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPS.Business
{
    public class Column
    {
        public string Header { get; set; }
        public string PropertyName { get; set; }
    }

    [Serializable]
    public class RequestBase : BaseEntity
    {
        [NotMapped]
        public bool IsSelected { get; set; }

        public int BranchId { get; set; }

        public int? PrintJobNo { get; set; }

        [Required]
        public int RequestNo { get; set; }

        [Display(Name = "Serial No", Order = 1)]
        public int SerialNo { get; set; }

        [Display(Name = "City Code", Order = 2)]
        public int CityCode { get; set; }

        [Display(Name = "Bank Code", Order = 3)]
        public int BankCode { get; set; }

        [Display(Name = "Branch Code", Order = 4)]
        [Required]
        public int BranchCode { get; set; }

        [NotMapped]
        public string MICRCode { get { return string.Format("{0:000}{1:000}{2:000}", CityCode, BankCode, BranchCode); } }

        [Display(Name="BSR Code", Order=5)]
        public string bsr_code{ get; set; }

        [Display(Name = "Acc Code", Order = 6)]
        public string actyp_vbcs { get; set; }

        [Display(Name = "A/C No", Order = 7)]
        [Required, RegularExpression(@"\d+")]
        public string AccountNo { get; set; }

        [Display(Name = "A/C BH", Order = 41)]
        [Required, RegularExpression(@"\d+")]
        public string AccountNoFull { get; set; }

        [Display(Name = "Trx Code", Order = 8)]
        [Required]
        public int TransactionCode { get; set; }

        [Display(Name = "Name", Order = 9)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Joint Name 1", Order = 10)]
        public string JointName1 { get; set; }

        [Display(Name = "Joint Name 2", Order = 11)]
        public string JointName2 { get; set; }

        [Display(Name = "Signatory 1", Order = 12)]
        public string Signatory1 { get; set; }

        [Display(Name = "Signatory 2", Order = 13)]
        public string Signatory2 { get; set; }

        [Display(Name = "Signatory 3", Order = 14)]
        public string Signatory3 { get; set; }

        [Display(Name = "Address 1", Order = 15)]
        public string Address1 { get; set; }

        [Display(Name = "Address 2", Order = 16)]
        public string Address2 { get; set; }

        [Display(Name = "Address 3", Order = 17)]
        public string Address3 { get; set; }

        [Display(Name = "Address 4", Order = 18)]
        public string Address4 { get; set; }

        [Display(Name = "Address 5", Order = 19)]
        public string Address5 { get; set; }

        [Display(Name = "City", Order = 20)]
        public string City { get; set; }

        [Display(Name = "PinCode", Order = 21)]
        public string PostalCode { get; set; }

        [Display(Name = "Home Phone", Order = 22)]
        public string tele_r_no { get; set; }

        [Display(Name = "Office Phone", Order = 23)]
        public string tele_o_no { get; set; }

        [Display(Name = "Mobile No", Order = 24)]
        public string mobile_no { get; set; }

        [Display(Name = "No of ChqBook", Order = 25)]
        [Required]
        public int NoOfChequeBook { get; set; }

        [Required]
        [Display(Name = "No of Cheque", Order = 26)]
        public int NoOfCheque { get; set; }

        [Display(Name = "Bearer Order", Order = 27)]
        [Required]
        public string BearerOrder { get; set; }

        [Required]
        [Display(Name = "AtPar", Order = 28)]
        [MaxLength(1)]
        public string AtPar { get; set; }

        [Display(Name = "prcode", Order = 29)]
        public string prcode { get; set; }

        [Required]
        [Display(Name = "Cheque From", Order = 30)]
        public int ChequeFrom { get; set; }

        [Required]
        [Display(Name = "Cheque To", Order = 31)]
        public int ChequeTo { get; set; }

        public bool IsManualEntry { get; set; }

        public bool IsPrinted { get; set; }

        [Display(Name = "additional_f1", Order = 32)]
        public string additional_f1 { get; set; }

        [Display(Name = "additional_f2", Order = 33)]
        public string additional_f2 { get; set; }

        [Display(Name = "additional_f3", Order = 34)]
        public string additional_f3 { get; set; }

        [Display(Name = "additional_f4", Order = 35)]
        public string additional_f4 { get; set; }

        [Display(Name = "additional_f5", Order = 36)]
        public string additional_f5 { get; set; }

        [Display(Name = "additional_f6", Order = 37)]
        public string additional_f6 { get; set; }

        [Display(Name = "additional_f7", Order = 38)]
        public string additional_f7 { get; set; }

        [Display(Name = "additional_f8", Order = 39)]
        public string additional_f8 { get; set; }

        [Display(Name = "additional_f9", Order = 40)]
        public string additional_f9 { get; set; }
    }

    [Table("Request")]
    [Serializable]
    public class RequestDTO : RequestBase
    {
        //[Display(Name = "ECS Account If", Order = 26)]
        //public string ECSAccountCode { get; set; }

        public override bool IsValid(IList<ValidationResult> results)
        {
            base.IsValid(results);

            try
            {
                using (var context = new CPSDbContext())
                {
                    var repository = new PersistenceBase<RequestDTO>(context);
                    if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.AccountNoFull == this.AccountNoFull && (((w.ChequeFrom >= this.ChequeFrom) && (w.ChequeFrom <= this.ChequeTo)) || ((this.ChequeFrom >= w.ChequeFrom) && (this.ChequeFrom <= w.ChequeTo)))).FirstOrDefault() != null)
                    {
                        results.Add(new ValidationResult("Cheque series are already exist."));
                    }
                    if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.AccountNoFull == this.AccountNoFull && w.TransactionCode != this.TransactionCode).FirstOrDefault() != null)
                    {
                        results.Add(new ValidationResult("A/C transaction code is not valid."));
                    }
                    if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.SerialNo == this.SerialNo).FirstOrDefault() != null)
                    {
                        results.Add(new ValidationResult("Serial number is already exist."));
                    }
                    if ((this.NoOfChequeBook * this.NoOfCheque) != (this.ChequeTo - this.ChequeFrom) + 1)
                    {
                        results.Add(new ValidationResult("Invalid cheque series."));
                    }
                }
            }
            catch (Exception)
            {
                results.Add(new ValidationResult("Server Error"));
            }

            return results.Count == 0;
        }
    }

    public class ImportRequest
    {
        private const string FIELDSEPARATOR = "~(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

        public bool Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File does not exist.");
            }

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            for (int i = 0; i < lines.Count(); i++)
            {
                // Skip header
                // if (i == 0) continue; 

                var obj = new RequestDTO();
                if (Parse(lines[i].Replace("\"", ""), obj))
                    _data.Add(obj);
            }

            return true;
        }

        private List<RequestDTO> _data = new List<RequestDTO>();
        public List<RequestDTO> Data { get { return _data; } private set { _data = value; } }
        public List<Column> Columns()
        {
            return typeof(RequestDTO).GetProperties()
                .Where(w => w.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0)
                .OrderBy(o => (o.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute).GetOrder())
                .Select(s => new Column { Header = (s.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute).GetName(), PropertyName = s.Name }).ToList();
        }
        private bool Parse(string line, RequestDTO importData)
        {
            var token = Regex.Split(line, FIELDSEPARATOR);
            var properties = importData.GetType().GetProperties()
                .Where(w => w.GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0)
                .OrderBy(o => (o.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute).GetOrder());
            var index = 0;
            foreach (var property in properties)
            {
                property.SetValue(importData, Convert.ChangeType(token[index++].Trim().Trim(new char[] { '"' }), property.PropertyType), null);
            }

            return true;
        }
    }

    [Serializable]
    public class PrintRequest
    {
        public RequestDTO Request { get; set; }

        public BranchMasterDTO Branch { get; set; }

        public AccountTypeDTO AccountType { get; set; }

        public PrintRequest DeepCopy()
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, this);
            m.Position = 0;
            return (PrintRequest)b.Deserialize(m);
        }

        public int ChequeNoFrom { get; set; }

        public int ChequeNoTo { get; set; }
    }

    public class RequestGroup
    {
        public RequestGroup()
        {
            Requests = new List<PrintRequest>();
        }

        public int BookSize { get; set; }

        public IEnumerable<PrintRequest> Requests { get; set; }
    }
}
