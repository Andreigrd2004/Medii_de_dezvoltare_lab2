using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using lab2.models;

namespace lab2.Data
{
    public class lab2Context : DbContext
    {
        public lab2Context (DbContextOptions<lab2Context> options)
            : base(options)
        {
        }

        public DbSet<lab2.models.Book> Book { get; set; } = default!;
        public DbSet<lab2.models.Publisher> Publisher { get; set; } = default!;
        public DbSet<lab2.models.Author> Author { get; set; } = default!;
        public DbSet<lab2.models.Category> Category { get; set; } = default!;
    }
}
