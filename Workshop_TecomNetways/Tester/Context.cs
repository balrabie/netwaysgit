using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Context : DbContext
    {
        public Context() : base("dummy")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<Student> Student { get; set; }
    }
}

