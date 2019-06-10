using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DreamingHome.Models;

namespace DreamingHome
{
    public class MainContext : DbContext
    {
        public MainContext() { }
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseSqlite(configuration["database:connection"]);
            // optionsBuilder.UseSqlite(@"Data Source=..\DreamingHome.db");
        }

        public DbSet<Case> Cases { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostPicture> PostPictures { get; set; }
        public DbSet<User> Users { get; set; }
    }
}