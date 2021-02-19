using MapPolygonLoader.Models;
using MapPolygonLoader.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
            var polygon = await _polygonLoader.GetPolygonPoints(model.Address, model.FrequencyOfPoints);

            return CreateFile(polygon, model.FileName);
        }

        public FileStreamResult CreateFile(Polygon polygon, string fileName)
        {
            var points = polygon.Points.Select(p => p.Latitude + " " + p.Longitude);
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(polygon.Name + "/n" + string.Join("/n", points)));
            return new FileStreamResult(stream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = fileName + ".txt"
            };
        }
    }
}
