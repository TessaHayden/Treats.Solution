using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlavorsNTreats.Models
{
  public class FlavorsNTreatsContext : DbContext
  {
    public DbSet<Flavor> Flavors { get; set; }
    public DbSet<Treat> Treats { get; set; }
    public DbSet<SweetNSavory> SweetNSavoryTreats { get; set; }
    public FlavorsNTreatsContext(DbContextOptions options) : base(options)
    {
    }
  }
}