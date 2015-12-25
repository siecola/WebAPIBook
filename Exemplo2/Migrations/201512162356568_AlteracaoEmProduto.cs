namespace Exemplo2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracaoEmProduto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Url", c => c.String(maxLength: 80));
            AlterColumn("dbo.Products", "codigo", c => c.String(nullable: false, maxLength: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "codigo", c => c.String(nullable: false));
            DropColumn("dbo.Products", "Url");
        }
    }
}
