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
                        
            if (!context.Users.Any(a => a.Username == "admin"))
            {
                context.Users.Add(new User()
                {
                    FriendlyName = "admin",
                    Username="admin",
                    Password = "1",
                    Status = -1,
                    InsertDateTime = DateTime.Now
                });
                context.SaveChanges();
            }

            //if (!context.Processes.Any())
            //{
            //    context.Processes.Add(new Process()
            //    {
            //        Name = "سوهان کاری",
            //        Status = -1,
            //        InsertDateTime = DateTime.Now
            //    });

            //    context.Processes.Add(new Process()
            //    {
            //        Name = "پوست کاری",
            //        Status = -1,
            //        InsertDateTime = DateTime.Now
            //    });

            //    context.Processes.Add(new Process()
            //    {
            //        Name = "پرداخت کاری",
            //        Status = -1,
            //        InsertDateTime = DateTime.Now
            //    });

            //    context.Processes.Add(new Process()
            //    {
            //        Name = "شستشو",
            //        Status = -1,
            //        InsertDateTime = DateTime.Now
            //    });

            //    context.Processes.Add(new Process()
            //    {
            //        Name = "آبکاری",
            //        Status = -1,
            //        InsertDateTime = DateTime.Now
            //    });

            //    context.Processes.Add(new Process()
            //    {
            //        Name = "براش کاری",
            //        Status = -1,
            //        InsertDateTime = DateTime.Now
            //    });
            //}

            if (!context.Processes.Any(a => a.Name == "اتمام تولید"))
            {
                var cmd = "SET IDENTITY_INSERT dbo.processes ON " +
                          "INSERT INTO dbo.processes(ID, [Name], InsertDateTime, status) VALUES(99, N'اتمام تولید', getdate(), -1) " +
                          "  " +
                          " SET IDENTITY_INSERT dbo.processes off " +
                          "  ";

                context.Database.ExecuteSqlCommand(cmd);
                context.Processes.Add(new Process()
                {
                    Name = "اتمام تولید",
                    Status = -1,
                    InsertDateTime = DateTime.Now
                });
            }

            context.SaveChanges();

        }
    }
}
