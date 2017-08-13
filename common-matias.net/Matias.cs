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
        SeppoClient seppoClient;
        public SeppoClient MatiasClient {
            get
            {
                return seppoClient;
            }
        }

        EwDatabase ewDatabase;
        public EwDatabase Ewdatabase {
        get
            {
                return ewDatabase;
            }
        }

        public Matias()
        {
            seppoClient = new SeppoClient();
            ewDatabase = new EwDatabase();
        }

        public void SyncEwDatabase()
        {
            var songs = ewDatabase.getSongs();
            var response = seppoClient.SyncEwDatabase(1, songs);
            response.Wait();

            var variationIdEwSongIds = new List<VariationIdEwSongId>();

            var results = response.Result;

            foreach(var ewSong in results.songs)
            {
                var variationIdEwSongId = ewDatabase.updateOrCreateSong(ewSong);
                if (variationIdEwSongId != null)
                {
                    variationIdEwSongIds.Add(variationIdEwSongId);
                }
            }

            foreach(var removeId in results.removeSongIds)
            {
                ewDatabase.removeEwSong(removeId);
            }

            seppoClient.InsertEwSongIds(1, variationIdEwSongIds);     
        }
    }
}
