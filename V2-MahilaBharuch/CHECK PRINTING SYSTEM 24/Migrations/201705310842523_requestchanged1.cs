namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestchanged1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Request", "MICRCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "MICRCode", c => c.String(maxLength: 4000));
        }
    }
}
