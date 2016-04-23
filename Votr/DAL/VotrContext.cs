using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Votr.Models;

namespace Votr.DAL
{
    public class VotrContext : DbContext
    {
        public VotrContext(string connectionString)
        {
            //base.DbContext
        }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Option> Options { get; set; }
    }
}