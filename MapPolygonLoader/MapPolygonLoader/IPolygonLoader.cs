using MapPolygonLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPolygonLoader
{
    public interface IPolygonLoader
    {
        Polygon GetPolygonPoints(string address, int frequencyOfPoints);
    }
}
