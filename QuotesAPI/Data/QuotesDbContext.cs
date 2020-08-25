using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using QuotesAPI.Models;

namespace QuotesAPI.Data
{
    //QuotesDbContext is responsible for CRUD operations with database
    public class QuotesDbContext : DbContext

    {
        public DbSet<Quote> Quotes { get; set; }
    }
}