using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.Collections;
using DCSoft.RTF;
using System.Data.SQLite;

namespace common_matias
{
    public class Matias
    {
        public event EventHandler<DatabaseLockStateChangedEventArgs> DatabaseLockStateChanged;

        SeppoClient seppoClient;

        EwDatabase ewDatabase;

        private string databasePath;

        public string DatabasePath
        {
            get {
                return ewDatabase.DatabasePath;
            }
            set {
                ewDatabase.DatabasePath = value;
            }
        }

        private string seppoIp;

        public string SeppoIp
        {
            get {
                return seppoClient.SeppoIp;
            }
            set {
                seppoClient.SeppoIp = value;
            }
        }

        private int seppoPort;

        public int? Seppoport
        {
            get {
                return seppoClient.SeppoPort;
            }
            set {
                seppoClient.SeppoPort = value;
            }
        }


        private string ewDatabaseKey;

        public string EwDatabaseKey
        {
            get {
                return ewDatabaseKey;
            }
            set {
                ewDatabaseKey = value;
            }
        }

        public bool DatabaseLocked
        {
            get {
                return ewDatabase.Lockstate;
            }
        }

        private bool syncAfterDatabaseUnlocked;

        public bool SyncAfterDatabaseUnlocked
        {
            get {
                return syncAfterDatabaseUnlocked;
            }
            set {
                syncAfterDatabaseUnlocked = value;
            }
        }

        public Matias()
        {
            seppoClient = new SeppoClient();
            ewDatabase = new EwDatabase();
            ewDatabase.DatabaseLockStateChanged += EwDatabase_databaseLockStateChanged;
            ewDatabase.SongIdsChanged += EwDatabase_SongIdsChanged;
        }

        private void EwDatabase_SongIdsChanged(object sender, SongIdsChangedEventArgs e)
        {
            
        }

        private void EwDatabase_databaseLockStateChanged(object sender, DatabaseLockStateChangedEventArgs e)
        {
            if (DatabaseLockStateChanged != null)
            {
                DatabaseLockStateChanged(this, e);
            }

            if (e.lockState == true)
            {

            } else if (e.lockState == false)
            {
                if (syncAfterDatabaseUnlocked)
                {
                    SyncEwDatabase();
                }
            }
        }

        private List<VariationIdEwSongId> UpdateOrCreateSongs(IEnumerable<Song> songs, IEnumerable<uint> removeSongIds, Dictionary<int, int> links = null)
        {
            var variationIdEwSongIds = new List<VariationIdEwSongId>();
            if (songs.Count() > 0)
            {
                Console.WriteLine("Creating or updating " + songs.Count() + " laulua.");
                int i = 1;
                foreach (var ewSong in songs)
                {
                    Console.WriteLine("Creating or updating song " + ewSong.title + " " + i + " / " + songs.Count());

                    if (ewSong.id != null && links != null && links.ContainsKey((int)ewSong.id))
                    {
                        ewSong.id = links[(int)ewSong.id];
                    }

                    var variationIdEwSongId = ewDatabase.updateOrCreateSong(ewSong);
                    if (variationIdEwSongId != null)
                    {
                        variationIdEwSongIds.Add(variationIdEwSongId);
                    }
                    i++;
                }
            }
            if (removeSongIds.Count() > 0)
            {
                Console.WriteLine("Removing " + removeSongIds.Count() + " songs");
                foreach (var removeId in removeSongIds)
                {
                    ewDatabase.removeEwSong(removeId);
                }
            }
            return variationIdEwSongIds;
        }

        public void SyncEwDatabase()
        {
            Console.WriteLine("Syncronizing databases...");
            var songs = ewDatabase.getSongs();
            Console.WriteLine("Sending " + songs.Count() + " songs to database with " + ewDatabaseKey + " ewdatabasekey");
            var response = seppoClient.SyncEwDatabase(ewDatabaseKey, songs.Where(p => p.id > 0));
            try
            {
                response.Wait();
            } catch (Exception e)
            {
                Console.WriteLine("Sending syncronization to server failed " + e.InnerException.Message);
            }
            var results = response.Result;
            Console.WriteLine("Results from server");

            if (results.songs.Count() == 0 && results.removeSongIds.Count() == 0)
            {
                Console.WriteLine("Ei poistettavia tai lisättäviä lauluja");
            }
            else
            {
                try
                {
                    var variationIdEwSongIds = UpdateOrCreateSongs(results.songs, results.removeSongIds);
                    seppoClient.InsertEwSongIds(ewDatabaseKey, variationIdEwSongIds);
                }
                catch (Exception e)
                {
                    var links = ewDatabase.fixDatabase();
                    var variationIdEwSongIds = UpdateOrCreateSongs(results.songs, results.removeSongIds, links);
                    seppoClient.InsertEwSongIds(ewDatabaseKey, variationIdEwSongIds);
                }
            }                
        }
    }
}
