namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestpij : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "bsr_code", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "actyp_vbcs", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Address4", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Address5", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "tele_r_no", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "tele_o_no", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "mobile_no", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "prcode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f3", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f4", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f5", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f6", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f7", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f8", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f9", c => c.String(maxLength: 4000));
            DropColumn("dbo.Request", "ECSAccountCode");
            DropColumn("dbo.Request", "MICRCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "MICRCode", c => c.String(nullable: false, maxLength: 4000));
            AddColumn("dbo.Request", "ECSAccountCode", c => c.String(maxLength: 4000));
            DropColumn("dbo.Request", "additional_f9");
            DropColumn("dbo.Request", "additional_f8");
            DropColumn("dbo.Request", "additional_f7");
            DropColumn("dbo.Request", "additional_f6");
            DropColumn("dbo.Request", "additional_f5");
            DropColumn("dbo.Request", "additional_f4");
            DropColumn("dbo.Request", "additional_f3");
            DropColumn("dbo.Request", "additional_f2");
            DropColumn("dbo.Request", "additional_f1");
            DropColumn("dbo.Request", "prcode");
            DropColumn("dbo.Request", "mobile_no");
            DropColumn("dbo.Request", "tele_o_no");
            DropColumn("dbo.Request", "tele_r_no");
            DropColumn("dbo.Request", "Address5");
            DropColumn("dbo.Request", "Address4");
            DropColumn("dbo.Request", "actyp_vbcs");
            DropColumn("dbo.Request", "bsr_code");
        }
    }
}
