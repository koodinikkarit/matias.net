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

        public EwDatabase Ewdatabase {
        get
            {
                return ewDatabase;
            }
        }

        public string seppoIp { get; set; }
        public int seppoPort { get; set; }

        public Matias(
            string seppoIp,
            int seppoPort,
            string songsDatabasePath,
            string songWordsDatabasePath
        ) {
            ewDatabase = new EwDatabase(songsDatabasePath, songWordsDatabasePath);
            var channel = new Channel(seppoIp + ":" + seppoPort, ChannelCredentials.Insecure);
            seppoClient = new Seppo.SeppoClient(channel);
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
            var songs = ewDatabase.getSongs();

            Console.WriteLine("songsCount " + songs.Count());

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

            seppoClient.syncEwDatabase(request);
        }
    }
}
