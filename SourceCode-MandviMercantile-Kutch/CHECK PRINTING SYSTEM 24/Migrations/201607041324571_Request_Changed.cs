namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_Changed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Request", "AccountNoFull", c => c.String(nullable: false, maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Request", "AccountNoFull", c => c.String(maxLength: 4000));
        }
    }
}
