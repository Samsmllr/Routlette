namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class servicelength : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessTemplates", "ServiceLength", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessTemplates", "ServiceLength");
        }
    }
}
