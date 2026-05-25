namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VUVSBL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "BSRCode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Column6", c => c.String(maxLength: 4000));
            DropColumn("dbo.Request", "bsr_code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "bsr_code", c => c.String(maxLength: 4000));
            DropColumn("dbo.Request", "Column6");
            DropColumn("dbo.Request", "BSRCode");
        }
    }
}
