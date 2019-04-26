
namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class servicescomplete : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceCompleteds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BusinessId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DateCompleted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessOwners", t => t.BusinessId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.BusinessId)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceCompleteds", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.ServiceCompleteds", "BusinessId", "dbo.BusinessOwners");
            DropIndex("dbo.ServiceCompleteds", new[] { "CustomerId" });
            DropIndex("dbo.ServiceCompleteds", new[] { "BusinessId" });
            DropTable("dbo.ServiceCompleteds");
        }
    }
}
