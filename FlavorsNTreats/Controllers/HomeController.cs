using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using FlavorsNTreats.Models;

namespace FlavorsNTreats.Controllers
{
  public class HomeController : Controller
  {
    private readonly FlavorsNTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public HomeController(UserManager<ApplicationUser> userManager, FlavorsNTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    [HttpGet("/")]
    public async Task<ActionResult> Index()
    {
      Flavor[] flavors = _db.Flavors.ToArray();
      Dictionary<string, object[]> model = new Dictionary<string, object[]>();
      model.Add("flavors", flavors);
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      if(currentUser != null)
      {
        Treat[] treats = _db.Treats
                        .Where(entry => entry.User.Id == currentUser.Id)
                        .ToArray();
        model.Add("treats", treats);
      }
      return View(model);
    }
  }
}