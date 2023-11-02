using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Treats.Models
{
  public class TreatsContext : DbContext
  {
    public TreatsContext(DbContextOptions options) : base(options)
    {
      
    }
  }
}