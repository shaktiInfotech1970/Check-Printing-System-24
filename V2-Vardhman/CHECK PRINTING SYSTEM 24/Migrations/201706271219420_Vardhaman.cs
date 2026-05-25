namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vardhaman : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Request", "RequestNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Request", "SerialNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Request", "AccountNoFull", c => c.String(nullable: false, maxLength: 4000));
            DropColumn("dbo.Request", "ECSAccountCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "ECSAccountCode", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "AccountNoFull", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "SerialNo", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "RequestNo", c => c.Int(nullable: false));
        }
    }
}
