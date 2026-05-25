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

        [Display(Name = "City Code", Order = 2)]
        [Parse(2)]
        public int CityCode { get; set; }

        [Display(Name = "Bank Code", Order = 3)]
        [Parse(3)]
        public int BankCode { get; set; }

        [Display(Name = "Branch Code", Order = 4)]
        [Required]
        [Parse(4)]
        public int BranchCode { get; set; }

        [Display(Name = "MICR", Order = 5)]
        [Parse(5)]
        public string MICRCode { get; set; }

        [Display(Name = "A/C No", Order = 6)]
        [Required]
        [Parse(6)]
        public string AccountNo { get; set; }

        [Display(Name = "A/C BH", Order = 7)]
        [Required]
        [Parse(7)]
        public string AccountNoFull { get; set; }

        [Display(Name = "Trx Code", Order = 8)]
        [Required]
        [Parse(8)]
        public int TransactionCode { get; set; }

        [Display(Name = "Name", Order = 9)]
        [Required]
        [Parse(9)]
        public string Name { get; set; }

        [Display(Name = "Joint Name 1", Order = 10)]
        [Parse(10)]
        public string JointName1 { get; set; }

        [Display(Name = "Joint Name 2", Order = 11)]
        [Parse(11)]
        public string JointName2 { get; set; }

        [Display(Name = "Signatory 1", Order = 12)]
        [Parse(12)]
        public string Signatory1 { get; set; }

        [Display(Name = "Signatory 2", Order = 13)]
        [Parse(13)]
        public string Signatory2 { get; set; }

        [Display(Name = "Signatory 3", Order = 14)]
        [Parse(14)]
        public string Signatory3 { get; set; }

        [Display(Name = "Address 1", Order = 15)]
        [Parse(15)]
        public string Address1 { get; set; }

        [Display(Name = "Address 2", Order = 16)]
        [Parse(16)]
        public string Address2 { get; set; }

        [Display(Name = "Address 3", Order = 17)]
        [Parse(17)]
        public string Address3 { get; set; }

        [Display(Name = "City", Order = 18)]
        [Parse(18)]
        public string City { get; set; }

        [Display(Name = "PinCode", Order = 19)]
        [Parse(19)]
        public string PostalCode { get; set; }

        [Display(Name = "No of ChqBook", Order = 20)]
        [Required]
        [Parse(20)]
        public int NoOfChequeBook { get; set; }

        [Required]
        [Display(Name = "No of Cheque", Order = 21)]
        [Parse(21)]
        public int NoOfCheque { get; set; }

        [Display(Name = "Bearer Order", Order = 22)]
        [Required]
        [Parse(22)]
        public string BearerOrder { get; set; }

        [Required]
        [Display(Name = "AtPar", Order = 23)]
        [MaxLength(1)]
        [Parse(23)]
        public string AtPar { get; set; }

        [Required]
        [Display(Name = "Cheque From", Order = 24)]
        [Parse(24)]
        public int ChequeFrom { get; set; }

        [Required]
        [Display(Name = "Cheque To", Order = 25)]
        [Parse(25)]
        public int ChequeTo { get; set; }

        [Display(Name = "IFSC", Order = 26)]
        [Parse(26)]
        public string IFSC { get; set; }

        [Display(Name = "additional_f1", Order = 27)]
        [Parse(27)]
        public string additional_f1 { get; set; }

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
                    if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.AccountNoFull == this.AccountNoFull && (((w.ChequeFrom >= this.ChequeFrom) && (w.ChequeFrom <= this.ChequeTo)) || ((this.ChequeFrom >= w.ChequeFrom) && (this.ChequeFrom <= w.ChequeTo)))).FirstOrDefault() != null)
                    {
                        results.Add(new ValidationResult("Cheque series are already exist."));
                    }
                    if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.AccountNoFull == this.AccountNoFull && w.TransactionCode != this.TransactionCode).FirstOrDefault() != null)
                    {
                        results.Add(new ValidationResult("A/C transaction code is not valid."));
                    }
                    //if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.SerialNo == this.SerialNo).FirstOrDefault() != null)
                    //{
                    //    results.Add(new ValidationResult("Serial number is already exist."));
                    //}
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
                .Where(w => w.GetCustomAttributes(typeof(ParseAttribute), false).Count() > 0)
                .OrderBy(o => (o.GetCustomAttributes(typeof(ParseAttribute), false).FirstOrDefault() as ParseAttribute).Order);
            foreach (var property in properties)
            {
                var parseAttribute = (property.GetCustomAttributes(typeof(ParseAttribute), false)[0] as ParseAttribute);
                var order = parseAttribute.Order - 1;

                property.SetValue(importData, Convert.ChangeType(token[order].Trim().Trim(new char[] { '"' }), property.PropertyType), null);
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
