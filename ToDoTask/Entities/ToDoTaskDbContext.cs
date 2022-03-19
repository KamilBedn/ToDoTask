using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoTask.Entities
{
    public class ToDoTaskDbContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>()
                .Property(d => d.DateAndTimeOfExiry)
                .IsRequired();

            modelBuilder.Entity<ToDo>()
                .Property(t => t.Title)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
             => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=loleczek;Database=ToDoTaskDb");

    }
}
