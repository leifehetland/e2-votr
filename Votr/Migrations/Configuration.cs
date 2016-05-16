namespace Votr.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

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

            List<Option> pizza_options = new List<Option>
            {
                new Option { Content = "Mafioso's",OptionId = 1},
                new Option { Content = "California Pizza Kitchen",OptionId = 2},
            };

            List<Option> sushi_options = new List<Option>
            {
                new Option { Content = "Samurai",OptionId = 3},
                new Option { Content = "Sonobana",OptionId = 4},
                new Option { Content = "Ken's",OptionId = 5},
            };

            context.Polls.AddOrUpdate(
                poll => poll.Title, // Is it in the database already?
                new Poll {PollId = 1, Title = "Best Pizza Joint 2013", StartDate = basetime.AddHours(1), EndDate = basetime.AddHours(7), Options = pizza_options },
                new Poll {PollId = 2, Title = "Best Sushi Joint 2015", StartDate = basetime, EndDate = basetime.AddMonths(1).AddDays(1).AddHours(12), Options = sushi_options }
            );

            string[] some_tags = new string[] { "food", "nashville", "cookiemonster" };

            foreach (var item in some_tags)
            {
                context.Tags.AddOrUpdate(
                    tag => tag.Name,
                    new Tag { Name = item }
                );
            }
        }
    }
}
