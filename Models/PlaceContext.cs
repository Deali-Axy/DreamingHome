using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DreamingHome.Models
{
    public class PlaceContext : DbContext
    {
        public PlaceContext() { }
        public PlaceContext(DbContextOptions<PlaceContext> options) : base(options) { }
        public DbSet<Place> Places { get; set; }
    }
}
