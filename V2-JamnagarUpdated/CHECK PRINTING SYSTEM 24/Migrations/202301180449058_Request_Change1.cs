namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_Change1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "CustomerId", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "VPIS", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "State", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Country", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "TransactionType", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "TransactionType");
            DropColumn("dbo.Request", "Country");
            DropColumn("dbo.Request", "State");
            DropColumn("dbo.Request", "VPIS");
            DropColumn("dbo.Request", "CustomerId");
        }
    }
}
