using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common_matias;
using Grpc.Core;
using System.IO;
using SharpYaml.Serialization;
//using SeppoService;
using System.Text.RegularExpressions;


namespace matias_console.net
{
    class Program
    {
        static void Main(string[] args)
        {
            string seppoIp = "";
            int seppoPort = 0;
            string ewDatabasePath = "";
            string ewDatabaseKey = "";

            if (File.Exists("config"))
            {
                var text = File.ReadAllText("config");
                foreach(var part in Regex.Split(text, "\n"))
                {
                    var rowParts = Regex.Split(part, "=");
                    switch(rowParts[0])
                    {
                        case "seppoip":
                            seppoIp = rowParts[1].Replace("\r", "");
                            break;
                        case "seppoport":
                            seppoPort = int.Parse(rowParts[1]);
                            break;
                        case "ewdatabasepath":
                            ewDatabasePath = rowParts[1].Replace("\r", "");
                            break;
                        case "ewdatabasekey":
                            ewDatabaseKey = rowParts[1].Replace("\r", "");
                            break;
                    }
                }
                Console.WriteLine(text + "\n");
            }

            if (
                seppoIp != "" && 
                seppoPort != 0 && 
                ewDatabasePath != "" && 
                ewDatabaseKey != "")
            {
                Matias matias = new Matias();
                matias.DatabasePath = ewDatabasePath;
                matias.SeppoIp = seppoIp;
                matias.Seppoport = seppoPort;
                matias.EwDatabaseKey = ewDatabaseKey;

                matias.SyncAfterDatabaseUnlocked = true;

                if (matias.DatabaseLocked == false)
                {
                    Console.WriteLine("Database not locked syncing now...");
                    matias.SyncEwDatabase();
                }

                while (true)
                {

                }
            }
            else
            {
                Console.WriteLine("Config ei loytynyt tai on virheellinen...");
                Console.WriteLine("Aseta se kansioon " + Directory.GetCurrentDirectory());
                Console.ReadKey();
            }
        }
    }
}
