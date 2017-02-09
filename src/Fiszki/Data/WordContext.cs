using Fiszki.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiszki.Data
{
    public class WordContext : DbContext
    {
        public WordContext(DbContextOptions<WordContext> options) : base(options)
        {
        }

            public DbSet<Word> Words { get; set; }
    }
}
