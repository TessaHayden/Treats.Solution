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
  public class FlavorsController : Controller
  {
    private readonly FlavorsNTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public FlavorsController(UserManager<ApplicationUser> userManager, FlavorsNTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    public ActionResult Index()
    {
      List<Flavor> model = _db.Flavors.ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor, int FlavorId)
    {
      if(!ModelState.IsValid)
      {
        ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
        return View(flavor);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        flavor.User = currentUser;
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    public ActionResult Details (int id)
    {
      Flavor thisFlavor = _db.Flavors
                          .Include(join => join.JoinEntities)
                          .ThenInclude(flavor => flavor.Treat)
                          .FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }
    public ActionResult Edit (int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(model => model.FlavorId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(thisFlavor);
    }
    [HttpPost]
    public async Task<ActionResult> Edit(Flavor flavor)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      flavor.User = currentUser;
      _db.Flavors.Update(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult AddTreat(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(model => model.FlavorId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(thisFlavor);
    }
    [HttpPost]
    public ActionResult AddTreat(Flavor flavor, int treatId)
    {
#nullable enable
      SweetNSavory? joinEntity = _db.SweetNSavoryTreats.FirstOrDefault(join => (join.TreatId == treatId && join.FlavorId == flavor.FlavorId));
      #nullable disable
      if(joinEntity == null && treatId != 0)
      {
        _db.SweetNSavoryTreats.Add(new SweetNSavory() {
          TreatId = treatId,
          FlavorId = flavor.FlavorId});
      _db.SaveChanges();
      }
      return RedirectToAction("Details", new {id = flavor.FlavorId});
    }
    public ActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(model => model.FlavorId == id);
      return View(thisFlavor);
    }
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(model => model.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
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