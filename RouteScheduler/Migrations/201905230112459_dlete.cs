namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dlete : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BusinessTemplates", "ServiceLength");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BusinessTemplates", "ServiceLength", c => c.Int(nullable: false));
        }
    }
}
