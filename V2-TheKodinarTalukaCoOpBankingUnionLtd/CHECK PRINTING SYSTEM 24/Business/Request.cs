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
using CPS.Common;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

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

        [Display(Name = "MICR Code", Order = 2)]
        [NotMapped]
        [Parse(27)]
        public string MapperMICRCode { get; set; }

        [Display(Name = "CustomerId", Order = 3)] 
        [Parse(2)]
        public string CustomerId { get; set; }

        [Display(Name = "A/C BH", Order = 4)]
        [Required]
        [Parse(3)]
        public string AccountNoFull { get; set; }

        [Display(Name = "A/C No", Order = 5)]
        [Required]
        [Parse(19)]
        public string AccountNo { get; set; }

        [Display(Name = "Name", Order = 6)]
        [Required]
        [Parse(4)]
        public string Name { get; set; }

        [Display(Name = "Joint Name 1", Order = 7)]
        [Parse(20)]
        public string JointName1 { get; set; }

        [Display(Name = "Joint Name 2", Order = 8)]
        [Parse(21)]
        public string JointName2 { get; set; }

        [Display(Name = "Address 1", Order = 9)]
        [Parse(6)]
        public string Address1 { get; set; }

        [Display(Name = "Address 2", Order = 10)]
        [Parse(7)]
        public string Address2 { get; set; }

        [Display(Name = "Address 3", Order = 11)]
        [Parse(8)]
        public string Address3 { get; set; }

        [Display(Name = "City", Order = 12)]
        [Parse(9)]
        public string City { get; set; }

        [Display(Name = "State", Order = 13)]
        [Parse(10)]
        public string State { get; set; }

        [Display(Name = "PinCode", Order = 14)]
        [Parse(11)]
        public string PostalCode { get; set; }

        [Display(Name = "Country", Order = 15)]
        [Parse(12)]
        public string Country { get; set; }

        [Display(Name = "Telr", Order = 16)]
        [Parse(13)]
        public string telr { get; set; }

        [Display(Name = "Mobile", Order = 17)]
        [Parse(14)]
        public string mob { get; set; }

        [Display(Name = "No of ChqBook", Order = 18)]
        [Required]
        [Parse(16)]
        public int NoOfChequeBook { get; set; }

        [Required]
        [Display(Name = "No of Cheque", Order = 19)]
        [Parse(17)]
        public int NoOfCheque { get; set; }

        [Display(Name = "Transaction Type", Order = 20)]
        [Parse(18)]
        public string TransactionType { get; set; }

        [Display(Name = "Trx Code", Order = 21)]
        [Required]
        [Parse(25)]
        public int TransactionCode { get; set; }

        [Display(Name = "VPIS", Order = 22)]
        [Parse(5)]
        public string VPIS { get; set; }

        [Required]
        [Display(Name = "Cheque From", Order = 23)]
        [Parse(23)]
        public int ChequeFrom { get; set; }

        [Required]
        [Display(Name = "Cheque To", Order = 24)]
        [Parse(24)]
        public int ChequeTo { get; set; }       

        [NotMapped]
        public string MICRCode { get { return string.Format("{0}{1}{2}", CityCode.ToString("000"), BankCode.ToString("000"), BranchCode.ToString("000")); } }

        [Display(Name = "City Code", Order = 25)]
        public int CityCode { get; set; }

        [Display(Name = "Bank Code", Order = 26)]
        public int BankCode { get; set; }

        [Display(Name = "Branch Code", Order = 27)]
        public int BranchCode { get; set; }
        public string brsid { get; set; } = null;

        string _BearerOrder;
        [Required]
        [Display(Name = "Bearer Order", Order = 28)]
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

        [Required]        
        [MaxLength(1)]
        public string AtPar { get; set; } = "N";        
        public string prcode { get; set; } = null;

        public bool IsManualEntry { get; set; }

        public bool IsPrinted { get; set; }


        [Display(Name = "additional_f1", Order = 29)]
        [Parse(15)]
        public string additional_f1 { get; set; }

        [Display(Name = "additional_f2", Order = 30)]
        [Parse(26)]
        public string additional_f2 { get; set; }

        [Display(Name = "additional_f3", Order = 31)]
        [Parse(28)]
        public string additional_f3 { get; set; }

        [Display(Name = "additional_f4", Order = 32)]
        [Parse(29)]
        public string additional_f4 { get; set; }

        [Display(Name = "additional_f5", Order = 33)]
        [Parse(30)]
        public string additional_f5 { get; set; }

        [Display(Name = "additional_f6", Order = 34)]
        [Parse(22)]
        public string additional_f6 { get; set; }

        [Display(Name = "additional_f7", Order = 35)]
        [Parse(32)]
        public string additional_f7 { get; set; }

        [Display(Name = "additional_f8", Order = 36)]
        [Parse(33)]
        public string additional_f8 { get; set; }

        [Display(Name = "additional_f9", Order = 37)]
        [Parse(34)]
        public string additional_f9 { get; set; }

        public string Signatory1 { get; set; } = null;
        public string Signatory2 { get; set; } = null;
        public string Signatory3 { get; set; } = null;
        public string Address4 { get; set; } = null;
        public string Address5 { get; set; } = null;
        public string telo { get; set; }
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
                    //After Discussion within the Team we are disabling this validation to minimize the support request.
                    //if (repository.FilterBy(w => w.Id != this.Id && w.BranchId == this.BranchId && w.SerialNo == this.SerialNo).FirstOrDefault() != null)
                    //{
                    //    errorMsg = string.Format("\"{0}\" Serial number is already exist", this.SerialNo);
                    //    errorMsg += string.IsNullOrEmpty(this.AccountNoFull) ? "." : string.Format(" for account no {0}.", this.AccountNoFull);
                    //    results.Add(new ValidationResult(errorMsg));
                    //}
                    if ((this.NoOfChequeBook * this.NoOfCheque) != (this.ChequeTo - this.ChequeFrom) + 1)
                    {
                        errorMsg = string.Format("[{0}-{1}] Invalid cheque series", this.ChequeFrom, this.ChequeTo);
                        errorMsg += string.IsNullOrEmpty(this.AccountNoFull) ? "." : string.Format(" for account no {0}.", this.AccountNoFull);
                        results.Add(new ValidationResult(errorMsg));
                    }
                    if (!((int[])Enum.GetValues(typeof(enumCheckBookSize))).Contains(this.NoOfCheque))
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

        //Extended Properties
        //---------------------
        //Excel 97 - 2003 Workbook(.xls)           "Excel 8.0"
        //Excel Workbook(.xlsx)                    "Excel 12.0 Xml"
        //Excel Macro-enabled workbook(.xlsm)      "Excel 12.0 Macro"
        //Excel Non-XML binary workbook(.xlsb)     "Excel 12.0"

        // Need To Install "Microsoft Access Database Engine 2010 Redistributable" 
        // https://www.microsoft.com/en-in/download/details.aspx?id=13255
        // For 32 bit Install AccessDatabaseEngine.exe
        // For 64 bit Install AccessDatabaseEngine_X64.exe

        // Exception Case 
        // My Scenario: 64-Bit Application, Win10-64, Office 2007 32-Bit installed.
        // Installation of the 32-Bit Installer AccessDatabaseEngine.exe as downloaded from MS reports success, but is NOT installed, as verified with the Powershell Script of one of the postings above here.
        // Installation of the 64-Bit installer AccessDatabaseEngine_X64.exe reported a shocking error message:
        // enter image description here
        // The very simple solution has been found here on an Autodesk site. Just add the parameter /passive to the commandline string, like this:
        // AccessDatabaseEngine_X64.exe /passive
        // Installation successful, the OleDb driver worked.

        private const string EXCELCONNECTIONSTRING = "Provider=Microsoft.ACE.OLEDB.12.0; data source={0}; " +
                                                     "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\" ";
        public bool Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File does not exist.");
            }
            string fileExtension = Path.GetExtension(filePath).ToLower();
            switch (fileExtension)
            {
                case ".xls":
                case ".xlsx":
                    string filename = filePath;
                    List<string> sheetNames = getExcelSheetList(filename);
                    foreach (var sheetName in sheetNames)
                    {
                        DataTable dt;
                        dt = getDataTableFromSheet(filename, sheetName);
                        _data = ConvertDataTable<RequestDTO>(dt);
                        break;
                    }
                    break;
                default:
                    string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        // Skip header
                        // if (i == 0) continue; 

                        var obj = new RequestDTO();
                        if (Parse(lines[i].Replace("\"", ""), obj))
                            _data.Add(obj);
                    }

                    break;
            }
            return true;
        }
        private List<string> getExcelSheetList(string filename)
        {
            System.Data.OleDb.OleDbConnection connection = null;
            List<string> sheetNames = new List<string>();
            try
            {
                connection = new System.Data.OleDb.OleDbConnection(string.Format(EXCELCONNECTIONSTRING, filename));
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                DataTable dtSheet = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                foreach (DataRow drSheet in dtSheet.Rows)
                    if (drSheet["TABLE_NAME"].ToString().Contains("$"))
                    {
                        string sheetName = drSheet["TABLE_NAME"].ToString();
                        sheetNames.Add(sheetName.StartsWith("'") ? sheetName.Substring(1, sheetName.Length - 3) : sheetName.Substring(0, sheetName.Length - 1));
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return sheetNames;
        }
        private DataTable getDataTableFromSheet(string filename, string sheetName)
        {
            System.Data.OleDb.OleDbConnection connection = null;
            DataTable dtImport = new DataTable();
            try
            {
                connection = new System.Data.OleDb.OleDbConnection(string.Format(EXCELCONNECTIONSTRING, filename));
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                System.Data.OleDb.OleDbDataAdapter importCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + sheetName + "$]", connection);
                importCommand.Fill(dtImport);
                //dtImport = dtImport.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return dtImport;
        }

        private List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            var properties = temp.GetProperties()
            .Where(w => w.GetCustomAttributes(typeof(ParseAttribute), false).Count() > 0)
            .OrderBy(o => (o.GetCustomAttributes(typeof(ParseAttribute), false).FirstOrDefault() as ParseAttribute).Order);
            foreach (PropertyInfo property in properties)
            {
                var parseAttribute = (property.GetCustomAttributes(typeof(ParseAttribute), false)[0] as ParseAttribute);
                var order = parseAttribute.Order - 1;
                property.SetValue(obj, Convert.ChangeType(dr[order], property.PropertyType), null);
            }
            return obj;
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
    [Serializable]
    public class PrintRequestAndHistory
    {
        public RequestDTO Request { get; set; }

        public BranchMasterDTO Branch { get; set; }

        public AccountTypeDTO AccountType { get; set; }

        public PrintHistoryDTO PrintHistory { get; set; }

        public PrintRequestAndHistory DeepCopy()
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, this);
            m.Position = 0;
            return (PrintRequestAndHistory)b.Deserialize(m);
        }
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
