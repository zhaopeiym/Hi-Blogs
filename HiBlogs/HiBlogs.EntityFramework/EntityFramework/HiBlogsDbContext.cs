using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using HiBlogs.Core;

namespace HiBlogs.EntityFramework.EntityFramework
{
    public class HiBlogsDbContext : DbContext
    {
        public HiBlogsDbContext(DbContextOptions<HiBlogsDbContext> options) : base(options)
        {
        }

        private string _connection;
        public HiBlogsDbContext(string connection)
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(_connection))
                optionsBuilder.UseMySql(_connection);

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
