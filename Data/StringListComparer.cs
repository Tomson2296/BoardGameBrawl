using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace BoardGameBrawl.Data
{
    public class StringListComparer : ValueComparer<List<string>>
    {
       public StringListComparer()
       : base(
           (c1, c2) => JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
           c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
           c => c.ToList())
        {
        }
    }
}
