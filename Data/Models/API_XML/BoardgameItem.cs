#nullable disable
using System.Xml.Serialization;

namespace BoardGameBrawl.Data.Models.API_XML
{
    [XmlRoot("items")]
    public class BoardgameItem
    {
        [XmlIgnore]
        [XmlAttribute("termsofuse")]
        public string TermsOfUse { get; set; }

        [XmlElement("item")]
        public GameInfo GameInfo { get; set; }
    }

    public class GameInfo
    {
        [XmlIgnore]
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("thumbnail")]
        public string Thumbnail { get; set; }

        [XmlElement("image")]
        public string Image { get; set; }

        [XmlElement("name")]
        public GameInfo_Name Name { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("yearpublished")]
        public YearPublished YearPublished { get; set; }

        [XmlElement("minplayers")]
        public MinPlayers MinPlayers { get; set; }

        [XmlElement("maxplayers")]
        public MaxPlayers MaxPlayers { get; set; }

        [XmlIgnore]
        [XmlElement("poll")]
        public Poll Poll { get; set; }

        [XmlElement("playingtime")]
        public PlayingTime PlayingTime { get; set; }

        [XmlElement("minplaytime")]
        public MinPlayTime MinPlayTime { get; set; }

        [XmlElement("maxplaytime")]
        public MaxPlayTime MaxPlayTime { get; set; }

        [XmlElement("minage")]
        public MinAge MinAge { get; set; }

        [XmlElement("link")]
        public Link[] Link { get; set; }

        [XmlElement("statistics")]
        public Statistics Statistics { get; set; }

        [XmlElement("videos")]
        public Videos Videos { get; set; }
    }

    public class GameInfo_Name
    {
        [XmlIgnore]
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlIgnore]
        [XmlAttribute("sortindex")]
        public int SortIndex { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class YearPublished
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class MinPlayers
    {
        [XmlAttribute("value")]
        public byte Value { get; set; }
    }

    public class MaxPlayers
    {
        [XmlAttribute("value")]
        public byte Value { get; set; }
    }

    public class Poll
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("totalvotes")]
        public int TotalVotes { get; set; }

        [XmlElement("results")]
        public Results[] Results { get; set; }
    }

    public class Results
    {
        [XmlAttribute("numplayers")]
        public byte NumOfPlayers { get; set; }

        [XmlElement("result")]
        public Result[] Result { get; set; }
    }

    public class Result
    {
        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("numvotes")]
        public int NumOfVotes { get; set; }

        [XmlIgnore]
        [XmlAttribute("level")]
        public int Level { get; set; }
    }

    public class PlayingTime
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class MinPlayTime
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class MaxPlayTime
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class MinAge
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class Link
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class Statistics
    {
        [XmlIgnore]
        [XmlAttribute("page")]
        public int Page { get; set; }

        [XmlElement("ratings")]
        public Ratings Ratings { get; set; }
    }

    public class Ratings
    {
        [XmlElement("usersrated")]
        public Usersrated Usersrated { get; set; }

        [XmlElement("average")]
        public Average Average { get; set; }

        [XmlIgnore]
        [XmlElement("bayesaverage")]
        public Bayesaverage Bayesaverage { get; set; }

        [XmlElement("ranks")]
        public Ranks Ranks { get; set; }

        [XmlIgnore]
        [XmlElement("stddev")]
        public Stddev Stddev { get; set; }

        [XmlIgnore]
        [XmlElement("median")]
        public Median Median { get; set; }

        [XmlIgnore]
        [XmlElement("owned")]
        public Owned Owned { get; set; }

        [XmlIgnore]
        [XmlElement("trading")]
        public Trading Trading { get; set; }

        [XmlIgnore]
        [XmlElement("wanting")]
        public Wanting Wanting { get; set; }

        [XmlIgnore]
        [XmlElement("wishing")]
        public Wishing Wishing { get; set; }

        [XmlIgnore]
        [XmlElement("numcomments")]
        public NumComments NumComments { get; set; }

        [XmlIgnore]
        [XmlElement("numweights")]
        public Numweights Numweights { get; set; }

        [XmlElement("averageweight")]
        public AverageWeight AverageWeight { get; set; }

        [XmlElement("videos")]
        public Videos Videos { get; set; }
    }

    public class Usersrated
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class Average
    {
        [XmlAttribute("value")]
        public float Value { get; set; }
    }

    public class Bayesaverage
    {
        [XmlAttribute("value")]
        public float Value { get; set; }
    }

    public class Ranks
    {
        [XmlElement("rank")]
        public Rank[] Rank { get; set; }
    }

    public class Rank
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("friendlyname")]
        public string FriendlyName { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlAttribute("bayesaverage")]
        public float Bayesaverage { get; set; }
    }

    public class Stddev
    {
        [XmlAttribute("value")]
        public float Value { get; set; }
    }

    public class Median
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class Owned
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class Trading
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class Wanting
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class Wishing
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class NumComments
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class Numweights
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class AverageWeight
    {
        [XmlAttribute("value")]
        public float Value { get; set; }
    }

    public class Videos 
    {
        [XmlAttribute("total")]
        public int Total { get; set; }

        [XmlElement("video")]
        public Video[] VideoList { get; set; }
    }

    public class Video
    {
        [XmlAttribute("id")]
        public int Total { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("category")]
        public string Category { get; set; }

        [XmlAttribute("language")]
        public string Language { get; set; }

        [XmlAttribute("link")]
        public string Link { get; set; }

        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlAttribute("userid")]
        public int UserId { get; set; }

        [XmlAttribute("postdate")]
        public DateTime PostDate { get; set; }
    }
}