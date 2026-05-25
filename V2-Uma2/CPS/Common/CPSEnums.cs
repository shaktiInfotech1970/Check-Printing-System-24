using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CPS.Common
{
    public enum Page
    {
        AccountType = 1,
        BankMaster,
        BranchMaster,
        ChequeBookSeries,
        DataExport,
        DataImport,
        PrintChequeBook,
        RePrintChequeBook,
        RePrintRequest,
        RePrintSinglePage,
        RequestDataEntry,
        DaywiseChequePrint,
        PendingChequeRequest,
        PrintedCheque,
        PrintedChequePrintFile,
        PrintedChequeSeries,
        ReprintedCheque,
        ReprintedChequeSinglePage,
        TotalPrintCheque,
        TotalReprintCheque,
        Permission,
        UserMaster,
        Preferences,
        DatabaseBackup,
        RequestLayoutPreference,
        ChequeLayoutPreference,
        SearchReport,
        RemoveRequestData
    }

    [Flags]
    public enum Permission
    {
        None = 0,
        Read = 1,
        Write = 2,
        Print = 4,
        All = Read | Write | Print
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum enumCheckBookSize
    {
        [Description("15")]
        Pages15 = 15,
        [Description("30")]
        Pages30 = 30,
        [Description("45")]
        Pages45 = 45,
        [Description("60")]
        Pages60 = 60
    }   
}
