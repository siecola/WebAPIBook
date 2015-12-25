namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        cpf = c.String(nullable: false, maxLength: 12),
                        name = c.String(nullable: false),
                        address = c.String(nullable: false),
                        city = c.String(nullable: false),
                        state = c.String(nullable: false),
                        country = c.String(nullable: false),
                        zip = c.String(nullable: false),
                        email = c.String(nullable: false),
                        mobile = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Customers");
        }
    }
}
