namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Counter_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Counter",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 32),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Counter");
        }
    }
}
