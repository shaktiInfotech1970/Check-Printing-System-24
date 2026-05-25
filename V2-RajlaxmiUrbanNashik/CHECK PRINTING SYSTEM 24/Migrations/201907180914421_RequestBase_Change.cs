namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestBase_Change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "additional_f10", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "additional_f10");
        }
    }
}
