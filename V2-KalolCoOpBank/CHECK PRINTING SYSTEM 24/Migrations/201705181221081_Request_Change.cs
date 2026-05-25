namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_Change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "Unknown1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "AccountNo", c => c.String(nullable: false, maxLength: 4000));
            AddColumn("dbo.Request", "tele_r_no", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "mobile_no", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "tele_o_no", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f3", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f4", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f5", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f6", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f7", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f8", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f9", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "PrintJobNo", c => c.Int());
            AlterColumn("dbo.Request", "RequestNo", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "SerialNo", c => c.Int(nullable: false));
            DropColumn("dbo.Request", "telr");
            DropColumn("dbo.Request", "telo");
            DropColumn("dbo.Request", "mob");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "mob", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telo", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telr", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "SerialNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Request", "RequestNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Request", "PrintJobNo", c => c.Long());
            DropColumn("dbo.Request", "additional_f9");
            DropColumn("dbo.Request", "additional_f8");
            DropColumn("dbo.Request", "additional_f7");
            DropColumn("dbo.Request", "additional_f6");
            DropColumn("dbo.Request", "additional_f5");
            DropColumn("dbo.Request", "additional_f4");
            DropColumn("dbo.Request", "additional_f3");
            DropColumn("dbo.Request", "additional_f2");
            DropColumn("dbo.Request", "additional_f1");
            DropColumn("dbo.Request", "tele_o_no");
            DropColumn("dbo.Request", "mobile_no");
            DropColumn("dbo.Request", "tele_r_no");
            DropColumn("dbo.Request", "AccountNo");
            DropColumn("dbo.Request", "Unknown1");
        }
    }
}
