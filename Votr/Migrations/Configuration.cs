namespace Votr.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Votr.DAL.VotrContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Votr.DAL.VotrContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            DateTime basetime = DateTime.Now;


            context.Polls.AddOrUpdate(
                poll => poll.Title, // Is it in the database already?
                new Poll { Title = "Best Pizza Joint 2014", StartDate = basetime, EndDate = basetime.AddDays(1) },
                new Poll { Title = "Best Pizza Joint 2012", StartDate = basetime.AddMonths(2), EndDate = basetime.AddMonths(2).AddHours(6) },
                new Poll { Title = "Best Pizza Joint 2013", StartDate = basetime.AddHours(1), EndDate = basetime.AddHours(7) },
                new Poll { Title = "Best Pizza Joint 2015", StartDate = basetime, EndDate = basetime.AddMonths(1).AddDays(1).AddHours(12) }
            );
        }
    }
}
