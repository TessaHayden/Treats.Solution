using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using FlavorsNTreats.Models;

namespace FlavorsNTreats.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly FlavorsNTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public TreatsController(UserManager<ApplicationUser> userManager,FlavorsNTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Treat> userTreats = _db.Treats
                          .Where(entry => entry.User.Id == currentUser.Id)
                          .Include(model => model.JoinEntities)
                          .ThenInclude(join => join.Flavor)
                          .ToList();
      return View(userTreats);
    }
    public ActionResult Create()
    {
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(Treat treat, int FlavorId)
    {
      if(!ModelState.IsValid)
      {
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
        return View(treat);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        treat.User = currentUser;
        _db.Treats.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
                        .Include(model => model.JoinEntities)
                        .ThenInclude(join => join.Flavor)
                        .FirstOrDefault(model => model.TreatId == id);
      return View(thisTreat);
    }
    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(model => model.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View(thisTreat);
    }
    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(model => model.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View(thisTreat);
    }
    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
#nullable enable
      SweetNSavory? joinEntity = _db.SweetNSavoryTreats.FirstOrDefault(join => (join.FlavorId == flavorId && join.TreatId == treat.TreatId));
      #nullable disable
      if(joinEntity == null && flavorId != 0)
      {
        _db.SweetNSavoryTreats.Add(new SweetNSavory()
        {
          FlavorId = flavorId,
          TreatId = treat.TreatId
        });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(model => model.TreatId == id);
      return View(thisTreat);
    }
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(model => model.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      SweetNSavory joinEntry = _db.SweetNSavoryTreats.FirstOrDefault(model => model.SweetNSavoryId == joinId);
      _db.SweetNSavoryTreats.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}