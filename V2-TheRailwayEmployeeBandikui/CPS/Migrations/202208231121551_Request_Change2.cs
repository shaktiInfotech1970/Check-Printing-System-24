namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_Change2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "brsid2", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "brsid2");
        }
    }
}
