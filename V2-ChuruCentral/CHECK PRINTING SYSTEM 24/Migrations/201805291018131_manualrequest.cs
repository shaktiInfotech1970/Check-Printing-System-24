namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manualrequest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Request", "PrintJobNo", c => c.Long());
            DropColumn("dbo.Request", "brsid");
            DropColumn("dbo.Request", "AccountNo");
            DropColumn("dbo.Request", "additional_f1");
            DropColumn("dbo.Request", "additional_f2");
            DropColumn("dbo.Request", "additional_f3");
            DropColumn("dbo.Request", "additional_f4");
            DropColumn("dbo.Request", "additional_f5");
            DropColumn("dbo.Request", "additional_f6");
            DropColumn("dbo.Request", "additional_f7");
            DropColumn("dbo.Request", "additional_f8");
            DropColumn("dbo.Request", "additional_f9");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "additional_f9", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f8", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f7", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f6", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f5", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f4", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f3", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "AccountNo", c => c.String(nullable: false, maxLength: 4000));
            AddColumn("dbo.Request", "brsid", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "PrintJobNo", c => c.Int());
        }
    }
}
