using System;
using System.Collections.Generic;
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
        ChequeLayoutPreference
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
}
