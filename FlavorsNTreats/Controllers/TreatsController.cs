using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using FlavorsNTreats.Models;

namespace FlavorsNTreats.Controllers
{
  public class TreatsController : Controller
  {
    private readonly FlavorsNTreatsContext _db;
    public TreatsController(FlavorsNTreatsContext db)
    {
      _db = db;
    }
    public ActionResult Index()
    {
      List<Treat> model = _db.Treats
                          .Include(model => model.JoinEntities)
                          .ThenInclude(join => join.Flavor)
                          .ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View();
    }
    [HttpPost]
    public ActionResult Create(Treat treat)
    {
      if(!ModelState.IsValid)
      {
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
        return View(treat);
      }
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
                        .Include(model => model.JoinEntities)
                        .ThenInclude(join => join.Flavor)
                        .FirstOrDefault(model => model.TreatId == id);
      return View(thisTreat);
    }
  }
}