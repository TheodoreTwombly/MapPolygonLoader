using MapPolygonLoader.Models;
using System.Threading.Tasks;

namespace MapPolygonLoader
{
    public interface IPolygonLoader
    {
        public Task<Polygon> GetPolygonPoints(string address, int frequencyOfPoints);
    }
}
