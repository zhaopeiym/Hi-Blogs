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

        //string str = @"Data Source=;Database=;User ID=;Password=;pooling=true;CharSet=utf8;port=3306;sslmode=none";
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        //    optionsBuilder.UseMySql(str);

        public DbSet<BlogInfo> BlogInfos { get; set; }
    }
}
