namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllSeeded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessOwners",
                c => new
                    {
                        BusinessId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        ApplicationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BusinessId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.BusinessTemplates",
                c => new
                    {
                        TemplateId = c.Int(nullable: false, identity: true),
                        BusinessId = c.Int(nullable: false),
                        JobName = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        ServiceLength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TemplateId)
                .ForeignKey("dbo.BusinessOwners", t => t.BusinessId, cascadeDelete: true)
                .Index(t => t.BusinessId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        ApplicationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ServiceRequesteds",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        TemplateId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.BusinessTemplates", t => t.TemplateId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.TemplateId)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceRequesteds", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.ServiceRequesteds", "TemplateId", "dbo.BusinessTemplates");
            DropForeignKey("dbo.Customers", "ApplicationId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BusinessTemplates", "BusinessId", "dbo.BusinessOwners");
            DropForeignKey("dbo.BusinessOwners", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.ServiceRequesteds", new[] { "CustomerId" });
            DropIndex("dbo.ServiceRequesteds", new[] { "TemplateId" });
            DropIndex("dbo.Customers", new[] { "ApplicationId" });
            DropIndex("dbo.BusinessTemplates", new[] { "BusinessId" });
            DropIndex("dbo.BusinessOwners", new[] { "ApplicationId" });
            DropTable("dbo.ServiceRequesteds");
            DropTable("dbo.Customers");
            DropTable("dbo.BusinessTemplates");
            DropTable("dbo.BusinessOwners");
        }
    }
}
