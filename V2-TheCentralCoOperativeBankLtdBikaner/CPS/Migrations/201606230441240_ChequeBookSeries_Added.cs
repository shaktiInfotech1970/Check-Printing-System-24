namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChequeBookSeries_Added : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChequeBookSeries");
        }
    }
}
