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

                foreach( var coordinates in firstResult.Geojson.Coordinates)
                    foreach (var polygonCoordinates in coordinates.PolygonCoordinates)
                        for(int i = 0; i < polygonCoordinates.PointsCoordinates.Count(); i += frequencyOfPoints)
                        {
                            polygon.Points.Add(new Point()
                            {
                                Longitude = polygonCoordinates.PointsCoordinates[i].Point[0],
                                Latitude = polygonCoordinates.PointsCoordinates[i].Point[1]
                            });
                        }

                return polygon;

            }
            return null;

        }
    }
}
