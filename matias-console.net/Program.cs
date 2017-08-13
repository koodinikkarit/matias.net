using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common_matias;
using Grpc.Core;
using System.IO;
//using SeppoService;

namespace matias_console.net
{
    class Program
    {
        static void Main(string[] args)
        {

            Matias matias = new Matias();
            matias.Ewdatabase.DatabasePath = @"D:\ewengineering\ew_makkara\v6.1\Databases";
            matias.MatiasClient.MatiasServerIp = "localhost";
            matias.MatiasClient.MatiasServerPort = 3214;

            //matias.SyncEwDatabase();

            while (true)
            {

            }
        }
    }
}
