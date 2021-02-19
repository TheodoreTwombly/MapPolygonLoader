using MapPolygonLoader.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using NominatimConverter;

namespace MapPolygonLoader
{
    public class OpenStreetMapPolygonLoader : IPolygonLoader
    {
        public async Task<Polygon> GetPolygonPoints(string address, int frequencyOfPoints)
        {

            var client = new RestClient($"https://nominatim.openstreetmap.org/search?q={address}&format=json&polygon_geojson=1");
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var nominatimResponse = NominatimResponse.FromJson(response.Content);
                var firstResult = nominatimResponse[0];

                var polygon = new Polygon() { Name = firstResult.DisplayName, Points = new List<Point>() };

                if(firstResult.Geojson.Coordinates.Count() > 1)
                {
                    foreach (var coordinates in firstResult.Geojson.Coordinates)
                        foreach (var polygonCoordinates in coordinates.PolygonCoordinates)
                            for (int i = 0; i < polygonCoordinates.PointsCoordinates.Count(); i += frequencyOfPoints)
                            {
                                polygon.Points.Add(new Point()
                                {
                                    Longitude = polygonCoordinates.PointsCoordinates[i].Point[0],
                                    Latitude = polygonCoordinates.PointsCoordinates[i].Point[1]
                                });
                            }
                }
                else
                {
                    for (int i = 0; i < firstResult.Geojson.Coordinates[0].PolygonCoordinates.Count(); i += frequencyOfPoints)
                    {
                        polygon.Points.Add(new Point()
                        {
                            Longitude = (double)firstResult.Geojson.Coordinates[0].PolygonCoordinates[i].PointsCoordinates[0].Double,
                            Latitude = (double)firstResult.Geojson.Coordinates[0].PolygonCoordinates[i].PointsCoordinates[1].Double
                        });
                    }            
                }

                return polygon;

            }
            return null;
        }
    }
}
