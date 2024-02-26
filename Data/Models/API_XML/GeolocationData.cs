using System.Xml.Serialization;

namespace BoardGameBrawl.Data.Models.API_XML
{
    [XmlRoot("searchresults")]
    public class GeolocationData
    {
        [XmlIgnore]
        [XmlAttribute("timestamp")]
        public string? Timestamp { get; set; }

        [XmlIgnore]
        [XmlAttribute("attribution")]
        public string? Attribution { get; set; }

        [XmlIgnore]
        [XmlAttribute("querystring")]
        public string? QueryString { get; set; }

        [XmlIgnore]
        [XmlAttribute("more_url")]
        public string? MoreUrl { get; set; }

        [XmlIgnore]
        [XmlAttribute("exclude_place_ids")]
        public string? Exclude_place_ids { get; set; }

        [XmlElement("place")]
        public Place? Place { get; set; }
    }

    public class Place
    {
        [XmlIgnore]
        [XmlAttribute("exclude_place_ids")]
        public int PlaceId { get; set; }

        [XmlIgnore]
        [XmlAttribute("osm_type")]
        public string? OsmType { get; set; }

        [XmlIgnore]
        [XmlAttribute("osm_id")]
        public string? OsmId { get; set; }

        [XmlAttribute("ref")]
        public string? City { get; set; }

        [XmlAttribute("lat")]
        public string? Latitude { get; set; }

        [XmlAttribute("lon")]
        public string? Longitude { get; set; }

        [XmlIgnore]
        [XmlAttribute("boundingbox")]
        public string? BoundingBox { get; set; }

        [XmlIgnore]
        [XmlAttribute("place_rank")]
        public string? PlaceRank { get; set; }

        [XmlIgnore]
        [XmlAttribute("address_rank")]
        public string? AddressRank { get; set; }

        [XmlIgnore]
        [XmlAttribute("display_name")]
        public string? DisplayName { get; set; }

        [XmlIgnore]
        [XmlAttribute("class")]
        public string? Class { get; set; }

        [XmlIgnore]
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlIgnore]
        [XmlAttribute("importance")]
        public string? Importance { get; set; }
    }
}
