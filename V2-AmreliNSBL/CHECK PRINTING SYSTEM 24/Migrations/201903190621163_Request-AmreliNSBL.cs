namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestAmreliNSBL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "additional_f10", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f11", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f12", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f13", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "additional_f13");
            DropColumn("dbo.Request", "additional_f12");
            DropColumn("dbo.Request", "additional_f11");
            DropColumn("dbo.Request", "additional_f10");
        }
    }
}
