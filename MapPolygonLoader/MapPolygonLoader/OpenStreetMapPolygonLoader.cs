using MapPolygonLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                var content = JsonConvert.DeserializeObject<JToken>(response.Content);


                IList<JToken> results = content[0]["geojson"]["coordinates"].Children().ToList();
                string name = content[0]["display_name"].Value<string>();


                //IList<SearchResult> searchResults = new List<SearchResult>();

                //foreach (JToken result in results)
                //{
                //    SearchResult searchResult = result.ToObject<SearchResult>();
                //    searchResults.Add(searchResult);
                //}

                //Get the league caption
                //var leagueCaption = content["leagueCaption"].Value<string>();
                ;

            }

            //TODO: log error, throw exception or do other stuffs for failed requests here.
            Console.WriteLine(response.Content);

            return null;

            //return new Polygon();
        }
    }
}
