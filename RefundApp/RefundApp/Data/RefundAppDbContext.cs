using Microsoft.EntityFrameworkCore;
using RefundApp.Models;
using System;

namespace RefundApp.Data
{
    public class RefundAppDbContext : DbContext
    {
        public DbSet<RefundModel> Refunds { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public RefundAppDbContext(DbContextOptions<RefundAppDbContext> options)
            : base(options)
        {
        }
    }
}
