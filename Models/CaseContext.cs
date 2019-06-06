using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DreamingHome.Models
{
    public class CaseContext : DbContext
    {
        public CaseContext() { }
        public CaseContext(DbContextOptions<CaseContext> options) : base(options) { }
        public DbSet<Case> cases { get; set; }
    }
}
