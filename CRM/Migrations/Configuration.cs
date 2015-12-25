namespace CRM.Migrations
{
    using CRM.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CRM.Models.CRMContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CRM.Models.CRMContext context)
        {
            context.Customers.AddOrUpdate(
            c => c.Id,
            new Customer
            {
                Id = 1,
                cpf = "12345678901",
                name = "CRM Web API",
                address = "Rua 1, 100",
                city = "São Paulo",
                state = "São Paulo",
                country = "Brasil",
                zip = "12345000",
                email = "matilde@siecolasystems.com",
                mobile = "+551112345678",
            }
            );
        }
    }
}
