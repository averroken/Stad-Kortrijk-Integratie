namespace ASP_WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Faqs",
                c => new
                    {
                        FaqID = c.Int(nullable: false, identity: true),
                        ThemeID = c.Int(nullable: false),
                        SubthemeID = c.Int(nullable: false),
                        Question = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.FaqID)
                .ForeignKey("dbo.Subthemes", t => t.SubthemeID, cascadeDelete: false)
                .ForeignKey("dbo.Themes", t => t.ThemeID, cascadeDelete: false)
                .Index(t => t.ThemeID)
                .Index(t => t.SubthemeID);
            
            CreateTable(
                "dbo.Subthemes",
                c => new
                    {
                        SubthemeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ThemeID = c.Int(nullable: false),
                        FotoURL = c.String(),
                    })
                .PrimaryKey(t => t.SubthemeID)
                .ForeignKey("dbo.Themes", t => t.ThemeID, cascadeDelete: false)
                .Index(t => t.ThemeID);
            
            CreateTable(
                "dbo.Offices",
                c => new
                    {
                        OfficeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        URL = c.String(),
                        Street = c.String(),
                        HouseNumber = c.String(),
                        ZipCode = c.Int(nullable: false),
                        City = c.String(),
                        PhoneNumber = c.String(),
                        OpeningHours = c.String(),
                        EmailAddress = c.String(),
                    })
                .PrimaryKey(t => t.OfficeID);
            
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        ThemeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FotoURL = c.String(),
                    })
                .PrimaryKey(t => t.ThemeID);
            
            CreateTable(
                "dbo.OfficeSubthemes",
                c => new
                    {
                        Office_OfficeID = c.Int(nullable: false),
                        Subtheme_SubthemeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Office_OfficeID, t.Subtheme_SubthemeID })
                .ForeignKey("dbo.Offices", t => t.Office_OfficeID, cascadeDelete: false)
                .ForeignKey("dbo.Subthemes", t => t.Subtheme_SubthemeID, cascadeDelete: false)
                .Index(t => t.Office_OfficeID)
                .Index(t => t.Subtheme_SubthemeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Faqs", "ThemeID", "dbo.Themes");
            DropForeignKey("dbo.Faqs", "SubthemeID", "dbo.Subthemes");
            DropForeignKey("dbo.Subthemes", "ThemeID", "dbo.Themes");
            DropForeignKey("dbo.OfficeSubthemes", "Subtheme_SubthemeID", "dbo.Subthemes");
            DropForeignKey("dbo.OfficeSubthemes", "Office_OfficeID", "dbo.Offices");
            DropIndex("dbo.OfficeSubthemes", new[] { "Subtheme_SubthemeID" });
            DropIndex("dbo.OfficeSubthemes", new[] { "Office_OfficeID" });
            DropIndex("dbo.Subthemes", new[] { "ThemeID" });
            DropIndex("dbo.Faqs", new[] { "SubthemeID" });
            DropIndex("dbo.Faqs", new[] { "ThemeID" });
            DropTable("dbo.OfficeSubthemes");
            DropTable("dbo.Themes");
            DropTable("dbo.Offices");
            DropTable("dbo.Subthemes");
            DropTable("dbo.Faqs");
        }
    }
}
