namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrinterPreference_Added : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PrinterPreference");
        }
    }
}
