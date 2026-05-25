namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchMaster_Change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchMaster", "Time1", c => c.String(maxLength: 256));
            AddColumn("dbo.BranchMaster", "Time2", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BranchMaster", "Time2");
            DropColumn("dbo.BranchMaster", "Time1");
        }
    }
}
