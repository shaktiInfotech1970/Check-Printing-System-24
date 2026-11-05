namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankMaster_Change1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankMaster", "WebAddress", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankMaster", "WebAddress");
        }
    }
}
