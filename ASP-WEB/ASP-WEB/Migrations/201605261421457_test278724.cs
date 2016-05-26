namespace ASP_WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test278724 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Offices", "ZipCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offices", "ZipCode", c => c.Int(nullable: false));
        }
    }
}
