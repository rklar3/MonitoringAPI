using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitoringAPI.Models;

namespace MonitoringAPI.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .HasOne(p => p.User)
                .WithMany(b => b.TodoItems)
                .HasForeignKey(c => c.UserID); 
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
