using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DreamingHome.Models
{
    public class PostContext : DbContext
    {
        public PostContext() { }
        public PostContext(DbContextOptions<PostContext> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }
    }
}
