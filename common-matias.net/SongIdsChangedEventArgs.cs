using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_matias
{
    public class SongIdsChangedEventArgs: EventArgs
    {
        public List<NewSongId> NewSongIds { get; set; }
    }
}
