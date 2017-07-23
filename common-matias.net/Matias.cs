using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using SeppoService;
using Google.Protobuf.Collections;
using DCSoft.RTF;
using System.Data.SQLite;

namespace common_matias
{
    public class Matias
    {
        EwDatabase ewDatabase;

        SeppoService.Seppo.SeppoClient seppoClient;
        Channel channel;

        public EwDatabase Ewdatabase {
        get
            {
                return ewDatabase;
            }
        }

        private string seppoIp;
        public string SeppoIp {
            get
            {
                return seppoIp;
            }
            set
            {
                seppoIp = value;
                this.createSeppoClient();
            }
        }

        private int seppoPort;
        public int SeppoPort {
            get
            {
                return seppoPort;
            }
            set
            {
                seppoPort = value;
                this.createSeppoClient();
            }
        }

        private void createSeppoClient()
        {
            Console.WriteLine("connection " + seppoIp + ":" + seppoPort);
            channel = new Channel(seppoIp + ":" + seppoPort, ChannelCredentials.Insecure);
            seppoClient = new Seppo.SeppoClient(channel);
        }

        public Matias(
            string seppoIp,
            int seppoPort,
            string songsDatabasePath,
            string songWordsDatabasePath
        ) {
            ewDatabase = new EwDatabase(songsDatabasePath, songWordsDatabasePath);
            this.createSeppoClient();
        }

        public Matias()
        {
            ewDatabase = new EwDatabase();
            seppoIp = "localhost";
            seppoPort = 3214;
            this.createSeppoClient();
        }

        public void updateOrCreate(EwSong song)
        {

        }

        public List<EwSong> getSongs()
        {
            List<EwSong> ewSongs = new List<EwSong>();



            return ewSongs;
        }

        public void SyncEwDatabase()
        {
            this.createSeppoClient();
            var songs = ewDatabase.getSongs();

            RepeatedField<EwSong> ewSongs = new RepeatedField<EwSong>();

            foreach (var song in songs)
            {
                var ewSong = new EwSong
                {
                    Id = (uint)song.id,
                    Title = song.title,
                    Author = song.author,
                    Copyright = song.copyright,
                    Administrator = song.administrator,
                    Description = song.description != null ? song.description : "",
                    Tags = song.tags != null ? song.tags : ""
                };

                if (song.verses != null)
                {
                    foreach (var verse in song.verses)
                    {
                        ewSong.Verses.Add(new EwVerse()
                        {
                            Text = verse.text
                        });
                    }
                }

                ewSongs.Add(ewSong);
            }

            var request = new SyncEwDatabaseRequest()
            {
                EwDatabaseId = 5
            };

            request.EwSongs.Add(ewSongs);

            var res = seppoClient.syncEwDatabase(request);

            channel.ShutdownAsync().Wait();

            Console.WriteLine("SyncEwDatabase " + res.EwSongs.Count);
        }
    }
}
