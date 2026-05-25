namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KalolUrbanCoOpBank : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BankMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 4000),
                        Name = c.String(nullable: false, maxLength: 100),
                        AddressLine1 = c.String(nullable: false, maxLength: 100),
                        AddressLine2 = c.String(maxLength: 100),
                        AddressLine3 = c.String(maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 100),
                        Country = c.String(nullable: false, maxLength: 100),
                        PostalCode = c.String(nullable: false, maxLength: 4000),
                        Phone = c.String(maxLength: 20),
                        Mobile = c.String(maxLength: 20),
                        Email = c.String(maxLength: 4000),
                        Fax = c.String(maxLength: 20),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BranchMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 4000),
                        Name = c.String(nullable: false, maxLength: 100),
                        ShortName = c.String(nullable: false, maxLength: 50),
                        IFSC = c.String(nullable: false, maxLength: 4000),
                        MICR = c.String(nullable: false, maxLength: 4000),
                        AddressLine1 = c.String(nullable: false, maxLength: 100),
                        AddressLine2 = c.String(maxLength: 100),
                        AddressLine3 = c.String(maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        PostalCode = c.String(nullable: false, maxLength: 4000),
                        Telephone1 = c.String(maxLength: 20),
                        Telephone2 = c.String(maxLength: 20),
                        Mobile = c.String(maxLength: 20),
                        Email = c.String(maxLength: 4000),
                        Fax = c.String(maxLength: 20),
                        ImportPath = c.String(maxLength: 100),
                        ExportPath = c.String(maxLength: 100),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChequeBookSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        AccountTypeId = c.Int(nullable: false),
                        StartChequeNumber = c.Int(nullable: false),
                        EndChequeNumber = c.Int(nullable: false),
                        LastChequeNumber = c.Int(nullable: false),
                        AvailableCheques = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChequeLayout",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        branchAddressVisble = c.Boolean(nullable: false),
                        branchAddressX = c.Single(nullable: false),
                        branchAddressY = c.Single(nullable: false),
                        ifscVisble = c.Boolean(nullable: false),
                        ifscX = c.Single(nullable: false),
                        ifscY = c.Single(nullable: false),
                        orderOrBarerVisble = c.Boolean(nullable: false),
                        orderOrBarerX = c.Single(nullable: false),
                        orderOrBarerY = c.Single(nullable: false),
                        accountNoVisble = c.Boolean(nullable: false),
                        accountNoX = c.Single(nullable: false),
                        accountNoY = c.Single(nullable: false),
                        stampVisble = c.Boolean(nullable: false),
                        stampX = c.Single(nullable: false),
                        stampY = c.Single(nullable: false),
                        micrVisble = c.Boolean(nullable: false),
                        micrX = c.Single(nullable: false),
                        micrY = c.Single(nullable: false),
                        barcodeVisble = c.Boolean(nullable: false),
                        barcodeX = c.Single(nullable: false),
                        barcodeY = c.Single(nullable: false),
                        audiTextVisble = c.Boolean(nullable: false),
                        audiTextX = c.Single(nullable: false),
                        audiTextY = c.Single(nullable: false),
                        accountPayeeVisble = c.Boolean(nullable: false),
                        accountPayeeX = c.Single(nullable: false),
                        accountPayeeY = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Counter",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 32),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.DatabaseBackup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(maxLength: 4000),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Page = c.Int(nullable: false),
                        Permission = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrinterPreference",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        RequestTray = c.Int(nullable: false),
                        ChequeTray = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrintHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        PrintType = c.Int(nullable: false),
                        ChequeNoFrom = c.Int(nullable: false),
                        ChequeNoTo = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        PrintJobNo = c.Long(),
                        RequestNo = c.Long(nullable: false),
                        SerialNo = c.Long(nullable: false),
                        CityCode = c.Int(nullable: false),
                        BankCode = c.Int(nullable: false),
                        BranchCode = c.Int(nullable: false),
                        AccountNoFull = c.String(nullable: false, maxLength: 4000),
                        TransactionCode = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 4000),
                        JointName1 = c.String(maxLength: 4000),
                        JointName2 = c.String(maxLength: 4000),
                        Signatory1 = c.String(maxLength: 4000),
                        Signatory2 = c.String(maxLength: 4000),
                        Signatory3 = c.String(maxLength: 4000),
                        Address1 = c.String(maxLength: 4000),
                        Address2 = c.String(maxLength: 4000),
                        Address3 = c.String(maxLength: 4000),
                        Address4 = c.String(maxLength: 4000),
                        Address5 = c.String(maxLength: 4000),
                        City = c.String(maxLength: 4000),
                        PostalCode = c.String(maxLength: 4000),
                        telr = c.String(maxLength: 4000),
                        telo = c.String(maxLength: 4000),
                        mob = c.String(maxLength: 4000),
                        NoOfChequeBook = c.Int(nullable: false),
                        NoOfCheque = c.Int(nullable: false),
                        BearerOrder = c.String(nullable: false, maxLength: 4000),
                        AtPar = c.String(nullable: false, maxLength: 1),
                        prcode = c.String(maxLength: 4000),
                        ChequeFrom = c.Int(nullable: false),
                        ChequeTo = c.Int(nullable: false),
                        IsManualEntry = c.Boolean(nullable: false),
                        IsPrinted = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestLayout",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        branchAddress1Visble = c.Boolean(nullable: false),
                        branchAddress1X = c.Single(nullable: false),
                        branchAddress1Y = c.Single(nullable: false),
                        branchAddress2Visble = c.Boolean(nullable: false),
                        branchAddress2X = c.Single(nullable: false),
                        branchAddress2Y = c.Single(nullable: false),
                        chequeFrom1Visble = c.Boolean(nullable: false),
                        chequeFrom1X = c.Single(nullable: false),
                        chequeFrom1Y = c.Single(nullable: false),
                        chequeTo1Visble = c.Boolean(nullable: false),
                        chequeTo1X = c.Single(nullable: false),
                        chequeTo1Y = c.Single(nullable: false),
                        chequeFrom2Visble = c.Boolean(nullable: false),
                        chequeFrom2X = c.Single(nullable: false),
                        chequeFrom2Y = c.Single(nullable: false),
                        chequeTo2Visble = c.Boolean(nullable: false),
                        chequeTo2X = c.Single(nullable: false),
                        chequeTo2Y = c.Single(nullable: false),
                        nameAddress1Visble = c.Boolean(nullable: false),
                        nameAddress1X = c.Single(nullable: false),
                        nameAddress1Y = c.Single(nullable: false),
                        nameAddress2Visble = c.Boolean(nullable: false),
                        nameAddress2X = c.Single(nullable: false),
                        nameAddress2Y = c.Single(nullable: false),
                        accountNo1Visble = c.Boolean(nullable: false),
                        accountNo1X = c.Single(nullable: false),
                        accountNo1Y = c.Single(nullable: false),
                        accountNo2Visble = c.Boolean(nullable: false),
                        accountNo2X = c.Single(nullable: false),
                        accountNo2Y = c.Single(nullable: false),
                        barcode1Visble = c.Boolean(nullable: false),
                        barcode1X = c.Single(nullable: false),
                        barcode1Y = c.Single(nullable: false),
                        barcode2Visble = c.Boolean(nullable: false),
                        barcode2X = c.Single(nullable: false),
                        barcode2Y = c.Single(nullable: false),
                        audiText1Visble = c.Boolean(nullable: false),
                        audiText1X = c.Single(nullable: false),
                        audiText1Y = c.Single(nullable: false),
                        audiText2Visble = c.Boolean(nullable: false),
                        audiText2X = c.Single(nullable: false),
                        audiText2Y = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        UserId = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 50),
                        IsLocked = c.Boolean(nullable: false),
                        LockDate = c.DateTime(),
                        LockReason = c.String(maxLength: 4000),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserMaster");
            DropTable("dbo.RequestLayout");
            DropTable("dbo.Request");
            DropTable("dbo.PrintHistory");
            DropTable("dbo.PrinterPreference");
            DropTable("dbo.Permission");
            DropTable("dbo.DatabaseBackup");
            DropTable("dbo.Counter");
            DropTable("dbo.ChequeLayout");
            DropTable("dbo.ChequeBookSeries");
            DropTable("dbo.BranchMaster");
            DropTable("dbo.BankMaster");
            DropTable("dbo.AccountType");
        }
    }
}
