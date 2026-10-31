namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sync : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankMaster", "WebAddress", c => c.String(maxLength: 100));
            AddColumn("dbo.BranchMaster", "Email2", c => c.String(maxLength: 4000));
            AddColumn("dbo.BranchMaster", "Time1", c => c.String(maxLength: 100));
            AddColumn("dbo.BranchMaster", "Time2", c => c.String(maxLength: 100));
            AddColumn("dbo.ChequeLayout", "custom1Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom1X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom1Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom1Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.ChequeLayout", "custom2Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom2X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom2Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom2Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.ChequeLayout", "custom3Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom3X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom3Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom3Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.ChequeLayout", "custom4Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom4X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom4Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom4Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.ChequeLayout", "custom5Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom5X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom5Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom5Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "CityCode", c => c.Int(nullable: false));
            AddColumn("dbo.Request", "BankCode", c => c.Int(nullable: false));
            AddColumn("dbo.Request", "brsid", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "JointName1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "JointName2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Signatory1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Signatory2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Signatory3", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "PostalCode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telr", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telo", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "mob", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "prcode", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "AccountNo", c => c.String(nullable: false, maxLength: 4000));
            AlterColumn("dbo.Request", "BearerOrder", c => c.String(nullable: false, maxLength: 4000));
            AlterColumn("dbo.Request", "AtPar", c => c.String(nullable: false, maxLength: 1));
            DropColumn("dbo.Request", "CustomerId");
            DropColumn("dbo.Request", "VPIS");
            DropColumn("dbo.Request", "State");
            DropColumn("dbo.Request", "PinCode");
            DropColumn("dbo.Request", "Country");
            DropColumn("dbo.Request", "TransactionType");
            DropColumn("dbo.Request", "SignatureJointName1");
            DropColumn("dbo.Request", "SignatureJointName2");
            DropColumn("dbo.Request", "MICRCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "MICRCode", c => c.String(nullable: false, maxLength: 4000));
            AddColumn("dbo.Request", "SignatureJointName2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "SignatureJointName1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "TransactionType", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Country", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "PinCode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "State", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "VPIS", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "CustomerId", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "AtPar", c => c.String(maxLength: 1));
            AlterColumn("dbo.Request", "BearerOrder", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "AccountNo", c => c.String(maxLength: 4000));
            DropColumn("dbo.Request", "prcode");
            DropColumn("dbo.Request", "mob");
            DropColumn("dbo.Request", "telo");
            DropColumn("dbo.Request", "telr");
            DropColumn("dbo.Request", "PostalCode");
            DropColumn("dbo.Request", "Signatory3");
            DropColumn("dbo.Request", "Signatory2");
            DropColumn("dbo.Request", "Signatory1");
            DropColumn("dbo.Request", "JointName2");
            DropColumn("dbo.Request", "JointName1");
            DropColumn("dbo.Request", "brsid");
            DropColumn("dbo.Request", "BankCode");
            DropColumn("dbo.Request", "CityCode");
            DropColumn("dbo.ChequeLayout", "custom5Text");
            DropColumn("dbo.ChequeLayout", "custom5Y");
            DropColumn("dbo.ChequeLayout", "custom5X");
            DropColumn("dbo.ChequeLayout", "custom5Visible");
            DropColumn("dbo.ChequeLayout", "custom4Text");
            DropColumn("dbo.ChequeLayout", "custom4Y");
            DropColumn("dbo.ChequeLayout", "custom4X");
            DropColumn("dbo.ChequeLayout", "custom4Visible");
            DropColumn("dbo.ChequeLayout", "custom3Text");
            DropColumn("dbo.ChequeLayout", "custom3Y");
            DropColumn("dbo.ChequeLayout", "custom3X");
            DropColumn("dbo.ChequeLayout", "custom3Visible");
            DropColumn("dbo.ChequeLayout", "custom2Text");
            DropColumn("dbo.ChequeLayout", "custom2Y");
            DropColumn("dbo.ChequeLayout", "custom2X");
            DropColumn("dbo.ChequeLayout", "custom2Visible");
            DropColumn("dbo.ChequeLayout", "custom1Text");
            DropColumn("dbo.ChequeLayout", "custom1Y");
            DropColumn("dbo.ChequeLayout", "custom1X");
            DropColumn("dbo.ChequeLayout", "custom1Visible");
            DropColumn("dbo.BranchMaster", "Time2");
            DropColumn("dbo.BranchMaster", "Time1");
            DropColumn("dbo.BranchMaster", "Email2");
            DropColumn("dbo.BankMaster", "WebAddress");
        }
    }
}
