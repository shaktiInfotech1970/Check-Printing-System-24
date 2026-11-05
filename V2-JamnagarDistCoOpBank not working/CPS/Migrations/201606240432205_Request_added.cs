namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ECSAccountCode = c.String(maxLength: 4000),
                        BranchId = c.Int(nullable: false),
                        PrintJobNo = c.Int(),
                        RequestNo = c.Int(nullable: false),
                        SerialNo = c.Int(nullable: false),
                        CityCode = c.Int(nullable: false),
                        BankCode = c.Int(nullable: false),
                        BranchCode = c.Int(nullable: false),
                        MICRCode = c.String(nullable: false, maxLength: 4000),
                        AccountNo = c.String(nullable: false, maxLength: 4000),
                        AccountNoFull = c.String(maxLength: 4000),
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
                        City = c.String(maxLength: 4000),
                        PostalCode = c.String(maxLength: 4000),
                        NoOfChequeBook = c.Int(nullable: false),
                        NoOfCheque = c.Int(nullable: false),
                        BearerOrder = c.String(nullable: false, maxLength: 4000),
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Request");
        }
    }
}
