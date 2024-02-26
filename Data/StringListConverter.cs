#nullable disable
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace BoardGameBrawl.Data
{
    public class StringListConverter : ValueConverter<List<string>, string>
    {
        public StringListConverter() : base(
        v => JsonConvert.SerializeObject(v),
        v => JsonConvert.DeserializeObject<List<string>>(v))
        { }
    }
}
