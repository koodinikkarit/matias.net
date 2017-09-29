using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using System.Net.Sockets;
using Google.Protobuf;
using SeppoService;
using Grpc.Core;
using System.IO;

namespace common_matias
{
    public class SeppoClient
    {
        Channel channel;
        SeppoService.Seppo.SeppoClient client;

        private string seppoIp;

        public string SeppoIp
        {
            get {
                return seppoIp;
            }
            set {
                seppoIp = value;
                this.CreateClient();
            }
        }

        private int? seppoPort = null;

        public int? SeppoPort
        {
            get {
                return seppoPort;
            }
            set {
                seppoPort = value;
                this.CreateClient();
            }
        }


        private void CreateClient()
        {
            if (seppoIp != null && seppoPort != null)
            {
                //this.tcpClient = new TcpClient(this.matiasServerIp, (int)this.matiasServerPort);
                //var a = new Grpc.Core.SslCredentials()
                //var channelCredentials = new SslCredentials(File.ReadAllText("ssl/ca.crt"));
                //var channelCredentials = new SslCredentials()
                //var cacert = File.ReadAllText("ssl/ca.crt");
                //var clientcert = File.ReadAllText("ssl/client.crt");
                //var clientkey = File.ReadAllText("ssl/client.key");
                //var ssl = new SslCredentials(cacert, new KeyCertificatePair(clientcert, clientkey));
                channel = new Channel(seppoIp, (int) seppoPort, ChannelCredentials.Insecure);
                //channel = new Channel(matiasServerIp + ":" + matiasServerPort, ssl);
                client = new SeppoService.Seppo.SeppoClient(channel);
            }
        }

        public SeppoClient()
        {

        }

        public void InsertEwSongIds(string ewDatabaseKey, List<VariationIdEwSongId> ids, Dictionary<int, int> links = null)
        {
            var insertEwSongIdsRequest = new SeppoService.InsertEwSongIdsRequest();
            insertEwSongIdsRequest.EwDatabaseKey = ewDatabaseKey;
            foreach(var id in ids)
            {
                insertEwSongIdsRequest.VariationIdEwSongIds.Add(new SeppoService.VariationIdEwSongId
                {
                    EwSongId = id.ewSongId,
                    VariationId = id.variationId
                });
            }

            if (links != null) {
                foreach (var link in links)
                {
                    insertEwSongIdsRequest.NewSongIds.Add(new SeppoService.NewSongId
                    {
                        OldEwSongId = (uint)link.Key,
                        NewEwSongId = (uint)link.Value
                    });
                }
            }

            var res = client.insertEwSongIds(insertEwSongIdsRequest);
        }

        public void ChangeSongIds(string ewDatabaseKey, List<NewSongId> newSongIds)
        {

        }

        public async Task<SyncEwDatabaseResponse> SyncEwDatabase(string EwDatabaseKey, IEnumerable<Song> songs)
        {
            var ewSongs = new RepeatedField<EwSong>();

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
                    Tags = song.tags != null ? song.tags : "",
                    Text = song.text
                };
                ewSongs.Add(ewSong);
            }

            var request = new SyncEwDatabaseRequest()
            {
                EwDatabaseKey = EwDatabaseKey
            };

            request.EwSongs.Add(ewSongs);

            var res = client.syncEwDatabase(request);

            return new common_matias.SyncEwDatabaseResponse
            {
                removeSongIds = res.RemoveEwSongIds,
                songs = res.EwSongs.Select(p => new Song
                {
                    variationId = p.VariationId,
                    id = (int)p.Id,
                    title = p.Title,
                    text = p.Text
                })
            };

            //return res.EwSongs.Select(p => new Song
            //{
            //    variationId = p.VariationId,
            //    id = (int) p.Id,
            //    title = p.Title,
            //    text = p.Text
            //});
        }
    }
}
