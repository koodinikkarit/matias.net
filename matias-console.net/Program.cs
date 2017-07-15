using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common_matias;

namespace matias_console.net
{
    class Program
    {
        static void Main(string[] args)
        {
            var matias = new Matias("localhost", 3214, @"D:\Data\Songs.db", @"D:\Data\SongWords.db");

            matias.SyncEwDatabase();
        }
    }
}
