using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_matias
{
    public class Song
    {
        public int? id { get; set; }
        public UInt32 variationId { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string copyright { get; set; }
        public string administrator { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
        public string text { get; set; }
        public List<Verse> verses { get; set; }
    }
}
