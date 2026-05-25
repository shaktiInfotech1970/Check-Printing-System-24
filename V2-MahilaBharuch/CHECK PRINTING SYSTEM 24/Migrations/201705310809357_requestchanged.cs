namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestchanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "MICRCode", c => c.String(maxLength: 4000));
            DropColumn("dbo.Request", "AccountNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "AccountNo", c => c.String(nullable: false, maxLength: 4000));
            DropColumn("dbo.Request", "MICRCode");
        }
    }
}
