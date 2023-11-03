using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using FlavorsNTreats.Models;

namespace FlavorsNTreats.Controllers
{
  public class HomeController : Controller
  {
    private readonly FlavorsNTreatsContext _db;
    public HomeController(FlavorsNTreatsContext db)
    {
      _db = db;
    }
    public ActionResult Index()
    {
      return View();
    }
  }
}