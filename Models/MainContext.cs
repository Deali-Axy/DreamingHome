using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DreamingHome.Models
{
    public class MainContext : DbContext
    {
        public MainContext() { }
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostPicture> PostPictures { get; set; }
        public DbSet<User> Users { get; set; }
    }
}