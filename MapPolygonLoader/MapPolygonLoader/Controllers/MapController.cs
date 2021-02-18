using MapPolygonLoader.Models;
using MapPolygonLoader.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPolygonLoader.Controllers
{
    public class MapController : Controller
    {
        IPolygonLoader _polygonLoader;

        public MapController(IPolygonLoader polygonLoader)
        {
            _polygonLoader = polygonLoader;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUserPolygonPoints(UserInputParameters model)
        {
            Polygon result = await _polygonLoader.GetPolygonPoints(model.Address, model.FrequencyOfPoints);

            return RedirectToAction("Index");
        }
    }
}
