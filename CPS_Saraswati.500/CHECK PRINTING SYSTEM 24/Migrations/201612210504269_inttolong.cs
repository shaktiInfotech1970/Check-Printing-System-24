namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inttolong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Request", "PrintJobNo", c => c.Long());
            AlterColumn("dbo.Request", "RequestNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Request", "SerialNo", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Request", "SerialNo", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "RequestNo", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "PrintJobNo", c => c.Int());
        }
    }
}
