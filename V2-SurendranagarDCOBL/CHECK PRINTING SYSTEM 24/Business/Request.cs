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
using CPS.Attributes;

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
        public long RequestNo { get; set; }

        [Display(Name = "Serial No", Order = 1)]
        [Parse(1)]
        public long SerialNo { get; set; }

        [Display(Name = "CustomerId", Order = 2)]
        [Parse(2)]
        public string CustomerId { get; set; }

        [Display(Name = "A/C BH", Order = 3)]
        [Required]
        [Parse(3)]
        public string AccountNoFull { get; set; }

        [Display(Name = "Name", Order = 4)]
        [Required]
        [Parse(4)]
        public string Name { get; set; }

        [Display(Name = "VPIS", Order = 5)]
        [Parse(5)]
        public string VPIS { get; set; }

        [Display(Name = "Address 1", Order = 6)]
        [Parse(6)]
        public string Address1 { get; set; }

        [Display(Name = "Address 2", Order = 7)]
        [Parse(7)]
        public string Address2 { get; set; }

        [Display(Name = "Address 3", Order = 8)]
        [Parse(8)]
        public string Address3 { get; set; }

        [Display(Name = "City", Order = 9)]
        [Parse(9)]
        public string City { get; set; }

        [Display(Name = "State", Order = 10)]
        [Parse(10)]
        public string State { get; set; }

        [Display(Name = "PinCode", Order = 11)]
        [Parse(11)]
        public string PinCode { get; set; }


        [Display(Name = "Country", Order = 12)]
        [Parse(12)]
        public string Country { get; set; }

        [Display(Name = "Phone 1", Order = 13)]
        [Parse(13)]
        public string Address4 { get; set; }

        [Display(Name = "Phone 2", Order = 14)]
        [Parse(14)]
        public string Address5 { get; set; }

        [Display(Name = "additional_f1", Order = 15)]
        [Parse(15)]
        public string additional_f1 { get; set; }

        [Display(Name = "No of ChqBook", Order = 16)]
        [Required]
        [Parse(16)]
        public int NoOfChequeBook { get; set; }

        [Required]
        [Display(Name = "No of Cheque", Order = 17)]
        [Parse(17)]
        public int NoOfCheque { get; set; }

        [Display(Name = "Transaction Type", Order = 18)]
        [Parse(18)]
        public string TransactionType { get; set; }

        [Display(Name = "AccountNo", Order = 19)]
        [Parse(19)]
        public string AccountNo { get; set; }

        [Display(Name = "Signature/JointName 1", Order = 20)]
        [Parse(20)]
        public string SignatureJointName1 { get; set; }

        [Display(Name = "Signature/JointName 2", Order = 21)]
        [Parse(21)]
        public string SignatureJointName2 { get; set; }

        [Display(Name = "Branch Code", Order = 22)]
        [Required]
        [Parse(22)]
        public int BranchCode { get; set; }

        [Display(Name = "Cheque From", Order = 23)]
        [Parse(23)]
        public int ChequeFrom { get; set; }

        [Display(Name = "Cheque To", Order = 24)]
        [Parse(24)]
        public int ChequeTo { get; set; }

        [Display(Name = "Trx Code", Order = 25)]
        [Required]
        [Parse(25)]
        public int TransactionCode { get; set; }

        [Display(Name = "additional_f2", Order = 26)]
        [Parse(26)]
        public string additional_f2 { get; set; }

        [Display(Name = "MICR Code", Order = 27)]
        [Required]
        [Parse(27)]
        public string MICRCode { get; set; }

        [Display(Name = "additional_f3", Order = 28)]
        [Parse(28)]
        public string additional_f3 { get; set; }

        [Display(Name = "additional_f4", Order = 29)]
        [Parse(29)]
        public string additional_f4 { get; set; }

        [Display(Name = "additional_f5", Order = 30)]
        [Parse(30)]
        public string additional_f5 { get; set; }

        [Display(Name = "additional_f6", Order = 31)]
        [Parse(31)]
        public string additional_f6 { get; set; }

        [Display(Name = "additional_f7", Order = 32)]
        [Parse(32)]
        public string additional_f7 { get; set; }

        [Display(Name = "additional_f8", Order = 33)]
        [Parse(33)]
        public string additional_f8 { get; set; }

        [Display(Name = "additional_f9", Order = 34)]
        [Parse(34)]
        public string additional_f9 { get; set; }

        [NotMapped]
        public int CityCode { get; set; }

        [NotMapped]
        public int BankCode { get; set; }

        string _BearerOrder;
        [Display(Name = "Bearer Order", Order = 27)]
        public string BearerOrder
        {
            get
            {
                return this._BearerOrder;
            }
            set
            {
                if (TransactionCode == 12)
                {
                    this._BearerOrder = "Order";
                }
                else
                {
                    this._BearerOrder = "Bearer";
                }
            }
        }

        [Display(Name = "AtPar")]
        [MaxLength(1)]
        public string AtPar { get; set; } = "N";

        public bool IsManualEntry { get; set; }

        public bool IsPrinted { get; set; }


    }

    [Table("Request")]
    [Serializable]
    public class RequestDTO : RequestBase
    {
        
        public static List<LookupItem<int, int>> GetPrintJobLookups()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<RequestDTO>(context);
                return repository.GetAll().Where(o => o.PrintJobNo.HasValue).Select(s => new LookupItem<int, int> { Key = s.PrintJobNo.Value, Value = s.PrintJobNo.Value }).Distinct().OrderByDescending(o => o.Key).Take(100).ToList();
            }
        }

        public override bool IsValid(IList<ValidationResult> results)
        {
            base.IsValid(results);

            try
            {
                using (var context = new CPSDbContext())
                {
                    var repository = new PersistenceBase<RequestDTO>(context);
                    var errorMsg = string.Empty;
                    if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.AccountNoFull == this.AccountNoFull && (((w.ChequeFrom >= this.ChequeFrom) && (w.ChequeFrom <= this.ChequeTo)) || ((this.ChequeFrom >= w.ChequeFrom) && (this.ChequeFrom <= w.ChequeTo)))).FirstOrDefault() != null)
                    {
                        errorMsg = string.Format("[{0}-{1}] Cheque series are already exist", this.ChequeFrom, this.ChequeTo);
                        errorMsg += string.IsNullOrEmpty(this.AccountNoFull) ? "." : string.Format(" for account no {0}.", this.AccountNoFull);
                        results.Add(new ValidationResult(errorMsg));
                    }
                    if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.AccountNoFull == this.AccountNoFull && w.TransactionCode != this.TransactionCode).FirstOrDefault() != null)
                    {
                        errorMsg = string.Format("\"{0}\" A/C transaction code is not valid", this.TransactionCode);
                        errorMsg += string.IsNullOrEmpty(this.AccountNoFull) ? "." : string.Format(" for account no {0}.", this.AccountNoFull);
                        results.Add(new ValidationResult(errorMsg));
                    }
                    if (string.IsNullOrWhiteSpace(this.AccountNo))
                    {
                        results.Add(new ValidationResult(string.Format("SAN is not valid for Account No: {0}", this.AccountNoFull)));
                    }
                    if ((this.NoOfChequeBook * this.NoOfCheque) != (this.ChequeTo - this.ChequeFrom) + 1)
                    {
                        errorMsg = string.Format("[{0}-{1}] Invalid cheque series", this.ChequeFrom, this.ChequeTo);
                        errorMsg += string.IsNullOrEmpty(this.AccountNoFull) ? "." : string.Format(" for account no {0}.", this.AccountNoFull);
                        results.Add(new ValidationResult(errorMsg));
                    }
                    List<int> bookSize = new List<int>() { 15, 30, 45, 60 };
                    if (!bookSize.Contains(this.NoOfCheque))
                    {
                        errorMsg = string.Format("\"{0}\" Invalid cheque book size", this.NoOfCheque);
                        errorMsg += string.IsNullOrEmpty(this.AccountNoFull) ? "." : string.Format(" for account no {0}.", this.AccountNoFull);
                        results.Add(new ValidationResult(errorMsg));
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
        private const string FIELDSEPARATOR = "\\|(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

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
                .Where(w => w.GetCustomAttributes(typeof(ParseAttribute), false).Count() > 0)
                .OrderBy(o => (o.GetCustomAttributes(typeof(ParseAttribute), false).FirstOrDefault() as ParseAttribute).Order);
            foreach (var property in properties)
            {
                var parseAttribute = (property.GetCustomAttributes(typeof(ParseAttribute), false)[0] as ParseAttribute);
                var order = parseAttribute.Order - 1;
                var value = token[order].Trim().Trim(new char[] { '"' });
                if (property.PropertyType == typeof(int) && string.IsNullOrWhiteSpace(value))
                    value = "0";
                property.SetValue(importData, Convert.ChangeType(value, property.PropertyType), null);
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

    [Serializable]
    public class ExportRequest : PrintRequest
    {
        public PrintHistoryDTO PrintHistory { get; set; }
    }
}
