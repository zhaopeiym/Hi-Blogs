using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using HiBlogs.Core;
using HiBlogs.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HiBlogs.EntityFramework.EntityFramework
{
    public class HiBlogsDbContext : IdentityDbContext<User, Role, int>
    {
        public HiBlogsDbContext(DbContextOptions<HiBlogsDbContext> options) : base(options)
        {
        }

        private string connection;
        public HiBlogsDbContext(string connection) => this.connection = connection;     

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(connection))
                optionsBuilder.UseMySql(connection);

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<RolePermissionName> RolePermissionNames { get; set; }

        public DbSet<BlogBlogTag> BlogBlogTags { get; set; }
        public DbSet<BlogBlogType> BlogBlogTypes { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<BlogType> BlogTypes { get; set; }
        public DbSet<Remark> Remarks { get; set; }

        public DbSet<ReplyRemark> ReplyRemarks { get; set; }
    }
}
