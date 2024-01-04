using System.Collections.Generic;

namespace Hive.Library.Models
{
    public class SessionCollection
    {
        public string Id { get; set; }
        public List<int> Players { get; set; } = new List<int>();
    }
}