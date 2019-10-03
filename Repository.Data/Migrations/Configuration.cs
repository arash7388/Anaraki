namespace Repository.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Repository.Entity.Domain;

    internal sealed class Configuration : DbMigrationsConfiguration<Repository.Data.MTOContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Repository.Data.MTOContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //if (context.Projects.Find(1) == null) context.Projects.Add(new Project() { Code = 1, Name = "aaa" });
            //if (context.Projects.Find(2) == null) context.Projects.Add(new Project() { Code = 2, Name = "bbb" });

           
            //  p => p.FullName,
            //  new Person { FullName = "Andrew Peters" },
            //  new Person { FullName = "Brice Lambson" },
            //  new Person { FullName = "Rowan Miller" }
            //);
            //

            if (context.Advertisements.Any(a => a.AreaId == null))
            {
                var firstArea = context.Areas.FirstOrDefault();
                context.Advertisements.ToList().ForEach(a=> a.AreaId=firstArea.Id);
                context.SaveChanges();
            }

            if (context.Users.Any(a => a.Username == "admin"))
            {
                context.Users.Add(new User()
                {
                    FriendlyName = "admin",
                    Password = "1",
                    Status = -1,
                    InsertDateTime = DateTime.Now
                });
                context.SaveChanges();
            }

        }
    }
}
