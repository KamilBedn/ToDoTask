using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoTask.Entities
{
    public class ToDoTaskDbContext : DbContext
    {
        public ToDoTaskDbContext(DbContextOptions<ToDoTaskDbContext> options) : base(options)
        {

        }
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

    }
}
