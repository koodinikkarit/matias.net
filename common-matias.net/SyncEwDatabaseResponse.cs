using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_matias
{
    public class SyncEwDatabaseResponse
    {
        public IEnumerable<Song> songs { get; set; }
        public IEnumerable<UInt32> removeSongIds { get; set; }
    }
}
