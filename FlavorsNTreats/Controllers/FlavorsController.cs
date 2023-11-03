using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using FlavorsNTreats.Models;

namespace FlavorsNTreats.Controllers
{
  public class FlavorsController : Controller
  {
    private readonly FlavorsNTreatsContext _db;
    public FlavorsController(FlavorsNTreatsContext db)
    {
      _db = db;
    }
    public ActionResult Index()
    {
      List<Flavor> model = _db.Flavors.ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View();
    }
    [HttpPost]
    public ActionResult Create(Flavor flavor)
    {
      if(!ModelState.IsValid)
      {
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
        return View(flavor);
      }
      _db.Flavors.Add(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
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
    public ActionResult Edit(Flavor flavor)
    {
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
//     [HttpPost]
//     public ActionResult AddTreat(Flavor flavor, int treatId)
//     {
// #nullable enable
//       SweetNSavory? joinEntity = _db.SweetNSavoryTreats.FirstOrDefault(model => (model.TreatId == treatId && join.FlavorId == flavor.FlavorId));
//       #nullable disable
//       if(joinEntity == null && treatId != 0)
//       {
//         _db.SweetNSavoryTreats.Add(new SweetNSavory() {
//           TreatId = treatId;
//           FlavorId = flavor.FlavorId
//         });
//       _db.SaveChanges();
//       }
//       return RedirectToAction("Details", new {id = flavor.FlavorId});
//     }
    public ActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(model => model.FlavorId == id);
      return View(thisFlavor);
    }
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