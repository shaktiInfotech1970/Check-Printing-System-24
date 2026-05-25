namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_Import_Changed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Request", "ECSAccountCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "ECSAccountCode", c => c.String(maxLength: 4000));
        }
    }
}
