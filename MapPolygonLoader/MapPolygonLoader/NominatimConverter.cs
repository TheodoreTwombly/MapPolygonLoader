using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace NominatimConverter
{
    public partial class NominatimResponse
    {
        [JsonProperty("place_id")]
        public long PlaceId { get; set; }

        [JsonProperty("licence")]
        public string Licence { get; set; }

        [JsonProperty("osm_type")]
        public OsmType OsmType { get; set; }

        [JsonProperty("osm_id")]
        public long OsmId { get; set; }

        [JsonProperty("boundingbox")]
        public string[] Boundingbox { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lon")]
        public string Lon { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("importance")]
        public double Importance { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Icon { get; set; }

        [JsonProperty("geojson")]
        public Geojson Geojson { get; set; }
    }

    public partial class Geojson
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public GeojsonCoordinate[] Coordinates { get; set; }
    }

    public enum OsmType { Node, Relation, Way };

    public partial struct PointsCoordinate
    {
        public double? Double;
        public double[] Point;

        public static implicit operator PointsCoordinate(double Double) => new PointsCoordinate { Double = Double };
        public static implicit operator PointsCoordinate(double[] DoubleArray) => new PointsCoordinate { Point = DoubleArray };
    }

    public partial struct PolygonCoordinate
    {
        public PointsCoordinate[] PointsCoordinates;
        public double? Double;

        public static implicit operator PolygonCoordinate(PointsCoordinate[] AnythingArray) => new PolygonCoordinate { PointsCoordinates = AnythingArray };
        public static implicit operator PolygonCoordinate(double Double) => new PolygonCoordinate { Double = Double };
    }

    public partial struct GeojsonCoordinate
    {
        public PolygonCoordinate[] PolygonCoordinates;
        public double? Double;

        public static implicit operator GeojsonCoordinate(PolygonCoordinate[] AnythingArray) => new GeojsonCoordinate { PolygonCoordinates = AnythingArray };
        public static implicit operator GeojsonCoordinate(double Double) => new GeojsonCoordinate { Double = Double };
    }

    public partial class NominatimResponse
    {
        public static NominatimResponse[] FromJson(string json) => JsonConvert.DeserializeObject<NominatimResponse[]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NominatimResponse[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                GeojsonCoordinateConverter.Singleton,
                FluffyCoordinateConverter.Singleton,
                PurpleCoordinateConverter.Singleton,
                OsmTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class GeojsonCoordinateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(GeojsonCoordinate) || t == typeof(GeojsonCoordinate?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new GeojsonCoordinate { Double = doubleValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<PolygonCoordinate[]>(reader);
                    return new GeojsonCoordinate { PolygonCoordinates = arrayValue };
            }
            throw new Exception("Cannot unmarshal type GeojsonCoordinate");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (GeojsonCoordinate)untypedValue;
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.PolygonCoordinates != null)
            {
                serializer.Serialize(writer, value.PolygonCoordinates);
                return;
            }
            throw new Exception("Cannot marshal type GeojsonCoordinate");
        }

        public static readonly GeojsonCoordinateConverter Singleton = new GeojsonCoordinateConverter();
    }

    internal class FluffyCoordinateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PolygonCoordinate) || t == typeof(PolygonCoordinate?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new PolygonCoordinate { Double = doubleValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<PointsCoordinate[]>(reader);
                    return new PolygonCoordinate { PointsCoordinates = arrayValue };
            }
            throw new Exception("Cannot unmarshal type FluffyCoordinate");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (PolygonCoordinate)untypedValue;
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.PointsCoordinates != null)
            {
                serializer.Serialize(writer, value.PointsCoordinates);
                return;
            }
            throw new Exception("Cannot marshal type FluffyCoordinate");
        }

        public static readonly FluffyCoordinateConverter Singleton = new FluffyCoordinateConverter();
    }

    internal class PurpleCoordinateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PointsCoordinate) || t == typeof(PointsCoordinate?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new PointsCoordinate { Double = doubleValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<double[]>(reader);
                    return new PointsCoordinate { Point = arrayValue };
            }
            throw new Exception("Cannot unmarshal type PurpleCoordinate");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (PointsCoordinate)untypedValue;
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.Point != null)
            {
                serializer.Serialize(writer, value.Point);
                return;
            }
            throw new Exception("Cannot marshal type PurpleCoordinate");
        }

        public static readonly PurpleCoordinateConverter Singleton = new PurpleCoordinateConverter();
    }

    internal class OsmTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(OsmType) || t == typeof(OsmType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "node":
                    return OsmType.Node;
                case "relation":
                    return OsmType.Relation;
                case "way":
                    return OsmType.Way;
            }
            throw new Exception("Cannot unmarshal type OsmType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (OsmType)untypedValue;
            switch (value)
            {
                case OsmType.Node:
                    serializer.Serialize(writer, "node");
                    return;
                case OsmType.Relation:
                    serializer.Serialize(writer, "relation");
                    return;
                case OsmType.Way:
                    serializer.Serialize(writer, "way");
                    return;
            }
            throw new Exception("Cannot marshal type OsmType");
        }

        public static readonly OsmTypeConverter Singleton = new OsmTypeConverter();
    }
}