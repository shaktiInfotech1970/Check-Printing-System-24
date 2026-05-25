namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChequeSeries_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChequeSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SAN = c.String(nullable: false, maxLength: 50),
                        LastChequePrint = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChequeSeries");
        }
    }
}
