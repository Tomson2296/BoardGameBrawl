#nullable disable
using System.Security.Policy;
using System.Xml.Serialization;

namespace BoardGameBrawl.Data.Models.API_XML
{
    [XmlRoot("items")]
    public class BoardGameCollection
    {
        [XmlAttribute("totalitems")]
        public int TotalItems { get; set; }

        [XmlIgnore]
        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; set; }

        [XmlIgnore]
        [XmlAttribute("pubdate")]
        public DateTime PubDate { get; set; }

        [XmlElement("item")]
        public List<Game> Games { get; set; }
    }

    public class Game
    {
        [XmlIgnore]
        [XmlAttribute("objecttype")]
        public string ObjectType { get; set; }

        [XmlAttribute("objectid")]
        public int ObjectId { get; set; }

        [XmlIgnore]
        [XmlAttribute("subtype")]
        public string SubType { get; set; }

        [XmlIgnore]
        [XmlAttribute("collid")]
        public int CollId { get; set; }

        [XmlElement("name")]
        public Name Name { get; set; }

        [XmlElement("yearpublished")]
        public int YearPublished { get; set; }

        [XmlElement("image")]
        public string Image { get; set; }

        [XmlElement("thumbnail")]
        public string Thumbnail { get; set; }

        [XmlIgnore]
        [XmlElement("status")]
        public Status Status { get; set; }

        [XmlElement("numplays")]
        public int NumPlays { get; set; }
    }

    public class Name 
    {
        [XmlIgnore]
        [XmlAttribute("sortindex")]
        public int SortIndex { get; set; }

        [XmlText]
        public string GameName { get; set; }
    }

    public class Status
    {
        [XmlAttribute("own")]
        public int Own { get; set; }

        [XmlAttribute("prevowned")]
        public int PrevOwned { get; set; }

        [XmlAttribute("fortrade")]
        public int ForTrade { get; set; }

        [XmlAttribute("want")]
        public int Want { get; set; }

        [XmlAttribute("wanttoplay")]
        public int WantToPlay { get; set; }

        [XmlAttribute("wanttobuy")]
        public int WantToBuy { get; set; }

        [XmlAttribute("wishlist")]
        public int Wishlist { get; set; }

        [XmlAttribute("preordered")]
        public int Preordered { get; set; }

        [XmlAttribute("lastmodified")]
        public DateTime LastModified { get; set; }
    }
}