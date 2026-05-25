namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestsurendranagardcobl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChequeSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SAN = c.String(nullable: false, maxLength: 50),
                        LastChequePrint = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Request", "CustomerId", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "VPIS", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "State", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "PinCode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Country", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "TransactionType", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "SignatureJointName1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "SignatureJointName2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "MICRCode", c => c.String(nullable: false, maxLength: 4000));
            AlterColumn("dbo.Request", "AccountNo", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "BearerOrder", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "AtPar", c => c.String(maxLength: 1));
            DropColumn("dbo.Request", "CityCode");
            DropColumn("dbo.Request", "BankCode");
            DropColumn("dbo.Request", "brsid");
            DropColumn("dbo.Request", "JointName1");
            DropColumn("dbo.Request", "JointName2");
            DropColumn("dbo.Request", "Signatory1");
            DropColumn("dbo.Request", "Signatory2");
            DropColumn("dbo.Request", "Signatory3");
            DropColumn("dbo.Request", "PostalCode");
            DropColumn("dbo.Request", "telr");
            DropColumn("dbo.Request", "telo");
            DropColumn("dbo.Request", "mob");
            DropColumn("dbo.Request", "prcode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "prcode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "mob", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telo", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telr", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "PostalCode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Signatory3", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Signatory2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Signatory1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "JointName2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "JointName1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "brsid", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "BankCode", c => c.Int(nullable: false));
            AddColumn("dbo.Request", "CityCode", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "AtPar", c => c.String(nullable: false, maxLength: 1));
            AlterColumn("dbo.Request", "BearerOrder", c => c.String(nullable: false, maxLength: 4000));
            AlterColumn("dbo.Request", "AccountNo", c => c.String(nullable: false, maxLength: 4000));
            DropColumn("dbo.Request", "MICRCode");
            DropColumn("dbo.Request", "SignatureJointName2");
            DropColumn("dbo.Request", "SignatureJointName1");
            DropColumn("dbo.Request", "TransactionType");
            DropColumn("dbo.Request", "Country");
            DropColumn("dbo.Request", "PinCode");
            DropColumn("dbo.Request", "State");
            DropColumn("dbo.Request", "VPIS");
            DropColumn("dbo.Request", "CustomerId");
            DropTable("dbo.ChequeSeries");
        }
    }
}
