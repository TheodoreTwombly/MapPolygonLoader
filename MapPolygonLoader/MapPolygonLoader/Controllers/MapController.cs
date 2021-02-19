using MapPolygonLoader.Models;
using MapPolygonLoader.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            Polygon polygon = await _polygonLoader.GetPolygonPoints(model.Address, model.FrequencyOfPoints);

            if (polygon == null)
            {
                polygon = new Polygon()
                {
                    Name = "Not found...",
                    Points = new List<Point>()
                };
            }
            return CreateFile(polygon, model.FileName);
        }

        public FileStreamResult CreateFile(Polygon polygon, string fileName)
        {
            var points = polygon.Points.Select(p => "[" + p.Latitude + "," + p.Longitude + "]");
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(polygon.Name + "\n" + string.Join("\n", points)));
            return new FileStreamResult(stream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = fileName + ".txt"
            };
        }
    }
}
