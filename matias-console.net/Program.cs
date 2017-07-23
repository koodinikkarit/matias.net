using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common_matias;
using Grpc.Core;
using SeppoService;

namespace matias_console.net
{
    class Program
    {
        static void Main(string[] args)
        {
            ////var matias = new Matias("localhost", 3214, @"D:\ewengineering\ew_makkara\v6.1\Databases\Data\Songs.db", @"D:\ewengineering\ew_makkara\v6.1\Databases\Data\SongWords.db");

            ////matias.SyncEwDatabase();

            //var channel = new Channel("127.0.0.1:3214", ChannelCredentials.Insecure);

            //var client = new SeppoService.Seppo.SeppoClient(channel);

            //var reply = client.syncEwDatabase(new SyncEwDatabaseRequest
            //{
            //    EwDatabaseId = 5
            //});

            //Console.WriteLine("Got: ");

            //channel.ShutdownAsync().Wait();

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            Matias matias = new Matias();
            matias.Ewdatabase.DatabasePath = @"D:\ewengineering\ew_makkara\v6.1\Databases";
            matias.SeppoIp = "10.70.0.50";
            matias.SeppoPort = 3214;

            matias.SyncEwDatabase();
        }
    }
}
