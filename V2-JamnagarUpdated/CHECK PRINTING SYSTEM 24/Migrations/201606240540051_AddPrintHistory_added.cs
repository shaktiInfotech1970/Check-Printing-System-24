namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrintHistory_added : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PrintHistory");
        }
    }
}
