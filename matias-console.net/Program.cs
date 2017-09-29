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
        static private Matias matias;

        static void Main(string[] args)
        {
            matias = new Matias();

            string seppoIp = "";
            int seppoPort = 0;
            string ewDatabasePath = "";
            string ewDatabaseKey = "";

            if (File.Exists(Directory.GetCurrentDirectory() + "config"))
            {
                Console.WriteLine("Config tiedosto löydetty");
                var text = File.ReadAllText(Directory.GetCurrentDirectory() + "config");
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
                Console.WriteLine("Haluatko luoda config tiedoston (y/n)");
                string c = Console.ReadLine();
                if (c == "y")
                {
                    Console.Write("Seppoip: ");
                    seppoIp = Console.ReadLine();
                    matias.SeppoIp = seppoIp;
                    Console.Write("\nSeppoport: ");
                    seppoPort = int.Parse(Console.ReadLine());
                    matias.Seppoport = seppoPort;
                    Console.Write("\nEwdatabasepath: ");
                    ewDatabasePath = Console.ReadLine();
                    matias.DatabasePath = ewDatabasePath;
                    Console.Write("\nEwdatabasekey: ");
                    ewDatabaseKey = Console.ReadLine();
                    matias.EwDatabaseKey = ewDatabaseKey;

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + "config"))
                    {
                        file.WriteLine("seppoip=" + seppoIp);
                        file.WriteLine("seppoport=" + seppoPort);
                        file.WriteLine("ewdatabasepath=" + ewDatabasePath);
                        file.WriteLine("ewdatabasekey=" + ewDatabaseKey);
                    }

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
                else if (c == "n")
                {
                    Console.WriteLine("Luo config tiedosto ja aseta se kansioon " + Directory.GetCurrentDirectory());
                }
                Console.ReadKey();
            }
        }
    }
}
