using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using DCSoft.RTF;
using System.IO;

namespace common_matias
{
    public class EwDatabase
    {
        SQLiteConnection songsConnection;
        SQLiteConnection songWordsConnection;

        public event EventHandler<DatabaseLockStateChangedEventArgs> databaseLockStateChanged;

        private FileSystemWatcher watcher;

        private string databasePath;

        public string DatabasePath
        {
            get { return databasePath; }
            set {
                Console.WriteLine("Songspath" + value + "\\Data\\Songs.db");
                databasePath = value;
                songsConnection = new SQLiteConnection("Data Source=" + value + "\\Data\\Songs.db;Version=3;");
                songWordsConnection = new SQLiteConnection("Data Source=" + value + "\\Data\\SongWords.db;Version=3;");
                songsConnection.Open();
                songWordsConnection.Open();

                songsConnection.Update += SongsConnection_Update;
                watcher.Path = value + "/Locks";
                watcher.EnableRaisingEvents = true;
            }
        }

        private void SongsConnection_Update(object sender, UpdateEventArgs e)
        {
            Console.WriteLine("Update songs");
        }

        public EwDatabase()
        {
            watcher = new FileSystemWatcher();
            watcher.Filter = "*.ulck";
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.FileName
                                    | NotifyFilters.DirectoryName;
            watcher.Created += Watcher_Created;
            watcher.Deleted += Watcher_Deleted;
        }

        public bool Lockstate { get; set; }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.Name == "Client (1).ulck")
            {
                Lockstate = true;
                if (databaseLockStateChanged != null)
                {
                    databaseLockStateChanged(this, new DatabaseLockStateChangedEventArgs
                    {
                        lockState = true
                    });
                }
            }
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (e.Name == "Client (1).ulck")
            {
                Lockstate = false;
                if (databaseLockStateChanged != null)
                {
                    databaseLockStateChanged(this, new DatabaseLockStateChangedEventArgs
                    {
                        lockState = false
                    });
                }
            }
        }

        public IEnumerable<Song> getSongs()
        {
            IEnumerable<Song> songs = new List<Song>();

            if (songsConnection.State == System.Data.ConnectionState.Open && songWordsConnection.State == System.Data.ConnectionState.Open)
            {
                var fetchSongsCommand = new SQLiteCommand("select * from song", songsConnection);
                var fetchSongWordsCommand = new SQLiteCommand("select * from word", songWordsConnection);

                Dictionary<int, Song> dSongs = new Dictionary<int, Song>();

                SQLiteDataReader songsReader = fetchSongsCommand.ExecuteReader();
                SQLiteDataReader songWordsReader = fetchSongWordsCommand.ExecuteReader();

                while (songsReader.Read())
                {
                    
                    var song = new Song()
                    {
                        id = int.Parse(songsReader["rowid"].ToString()),
                        title = songsReader["title"] != System.DBNull.Value ? (string)songsReader["title"] : "",
                        author = songsReader["author"] != System.DBNull.Value ? (string)songsReader["author"] : "",
                        copyright = songsReader["copyright"] != System.DBNull.Value ? (string)songsReader["copyright"] : "",
                        administrator = songsReader["administrator"] != System.DBNull.Value ? (string)songsReader["administrator"] : "",
                        description = songsReader["description"] != System.DBNull.Value ? (string)songsReader["description"] : "",
                        tags = songsReader["tags"] != System.DBNull.Value ? (string)songsReader["tags"] : ""
                    };

                    dSongs[(int)song.id] = song;
                }

                while (songWordsReader.Read())
                {
                    Song song;

                    var id = int.Parse(songWordsReader["song_id"].ToString());

                    if (dSongs.ContainsKey(id)) song = dSongs[id];
                    else
                    {
                        song = new Song()
                        {
                            id = id
                        };
                        dSongs[id] = song;
                    }

                    song.text = songWordsReader["words"] != System.DBNull.Value ? RichTextStripper.StripRichTextFormat((string)songWordsReader["words"]) : "";
                }
                songs = dSongs.Select(p => p.Value);
            }
            return songs;
        }

        public Song getSong(uint id)
        {
            Song song = null;
            if (songsConnection.State == System.Data.ConnectionState.Open && songWordsConnection.State == System.Data.ConnectionState.Open)
            {
                song = new Song();
                var fetchSongCommand = new SQLiteCommand("select * from song", songsConnection);
                var fetchSongWordsCommand = new SQLiteCommand("select * from word", songWordsConnection);

                SQLiteDataReader songReader = fetchSongCommand.ExecuteReader();
                SQLiteDataReader songWordsReader = fetchSongWordsCommand.ExecuteReader();

                while(songReader.Read())
                {
                    song = new Song()
                    {
                        id = (int)songReader["rowid"],
                        title = songReader["title"] != System.DBNull.Value ? (string)songReader["title"] : "",
                        author = songReader["author"] != System.DBNull.Value ? (string)songReader["author"] : "",
                        copyright = songReader["copyright"] != System.DBNull.Value ? (string)songReader["copyright"] : "",
                        administrator = songReader["administrator"] != System.DBNull.Value ? (string)songReader["administrator"] : "",
                        description = songReader["description"] != System.DBNull.Value ? (string)songReader["description"] : "",
                        tags = songReader["tags"] != System.DBNull.Value ? (string)songReader["tags"] : ""
                    };
                }
                if (song != null)
                {
                    while (songWordsReader.Read())
                    {
                        song.text = songWordsReader["words"] != System.DBNull.Value ? RichTextStripper.StripRichTextFormat((string)songWordsReader["words"]) : "";
                    }
                }
            }
            return song;
        }

        public bool songExists(int id)
        {
            if (songsConnection.State == System.Data.ConnectionState.Open)
            {
                var fetchSongCommand = new SQLiteCommand("select * from song where rowid = " + id, songsConnection);

                var songReader = fetchSongCommand.ExecuteReader();

                bool found = false;

                while(songReader.Read())
                {
                    found = true;
                }
                return found;
            }
            return false;
        }

        public VariationIdEwSongId updateOrCreateSong(Song song)
        {
            if (songsConnection.State == System.Data.ConnectionState.Open && songWordsConnection.State == System.Data.ConnectionState.Open)
            {
                if (song.id != null && songExists((int) song.id))
                {
                    var updateSongCommand = new SQLiteCommand("update song set title = ?, author = ?, copyright =  ?, administrator = ?, description = ?, tags = ? where rowid = ?", songsConnection);
                    updateSongCommand.Parameters.Add(new SQLiteParameter("@title", song.title));
                    updateSongCommand.Parameters.Add(new SQLiteParameter("@author", song.author));
                    updateSongCommand.Parameters.Add(new SQLiteParameter("@copyright", song.copyright));
                    updateSongCommand.Parameters.Add(new SQLiteParameter("@administrator", song.administrator));
                    updateSongCommand.Parameters.Add(new SQLiteParameter("@description", song.description));
                    updateSongCommand.Parameters.Add(new SQLiteParameter("@tags", song.tags));
                    updateSongCommand.Parameters.Add(new SQLiteParameter("@rowid", song.id.ToString()));

                    updateSongCommand.ExecuteNonQuery();

                    string words = @"{\rtf1{\pard " + song.text.Replace("\n", @"\par ") + "}}";

                    var updateSongWordsCommand = new SQLiteCommand("update word set words = ? where song_id = ?", songWordsConnection);

                    updateSongWordsCommand.Parameters.Add(new SQLiteParameter("@words", words));
                    updateSongWordsCommand.Parameters.Add(new SQLiteParameter("@song_id", song.id));

                    updateSongWordsCommand.ExecuteNonQuery();
                } else
                {
                    var songUid = Guid.NewGuid();
                    var insertSongCommand = new SQLiteCommand("insert into song (song_item_uid, song_uid, title, author, copyright, administrator, description, tags) values(@song_item_uid, @song_uid, @title, @author, @copyright, @administrator, @description, @tags)", songsConnection);
                    insertSongCommand.Parameters.Add(new SQLiteParameter("@song_item_uid", songUid));
                    insertSongCommand.Parameters.Add(new SQLiteParameter("@song_uid", songUid));
                    insertSongCommand.Parameters.Add(new SQLiteParameter("@title", song.title));
                    insertSongCommand.Parameters.Add(new SQLiteParameter("@author", song.author));
                    insertSongCommand.Parameters.Add(new SQLiteParameter("@copyright", song.copyright));
                    insertSongCommand.Parameters.Add(new SQLiteParameter("@administrator", song.administrator));
                    insertSongCommand.Parameters.Add(new SQLiteParameter("@description", song.description));
                    insertSongCommand.Parameters.Add(new SQLiteParameter("@tags", song.tags));

                    try
                    {
                        insertSongCommand.ExecuteNonQuery(System.Data.CommandBehavior.SingleRow);
                    } catch(Exception e)
                    {
                        fixDatabase();
                        insertSongCommand.ExecuteNonQuery(System.Data.CommandBehavior.SingleRow);
                    }

                    var songId = songsConnection.LastInsertRowId;

                    string words = @"{\rtf1{\pard " + song.text.Replace("\n", @"\par ") + "}}";

                    var insertSongWordsCommand = new SQLiteCommand("insert into word (song_id, words) values (@song_id, @words)", songWordsConnection);
                    insertSongWordsCommand.Parameters.Add(new SQLiteParameter("@song_id", songId.ToString()));
                    insertSongWordsCommand.Parameters.Add(new SQLiteParameter("@words", words));

                    insertSongWordsCommand.ExecuteNonQuery();

                    return new VariationIdEwSongId
                    {
                        variationId = (UInt32) song.variationId,
                        ewSongId = (UInt32) songId
                    };
                }
            }
            return null;
        }

        public void removeEwSong(UInt32 id)
        {
            if (songsConnection.State == System.Data.ConnectionState.Open && songWordsConnection.State == System.Data.ConnectionState.Open)
            {
                var removeSongCommand = new SQLiteCommand("delete from song where rowid = @rowid", songsConnection);
                removeSongCommand.Parameters.Add(new SQLiteParameter("@rowid", id));
                try
                {
                    removeSongCommand.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    fixDatabase();
                    removeSongCommand.ExecuteNonQuery();
                }

                var removeSongWordsCommand = new SQLiteCommand("delete from word where song_id = @song_id", songWordsConnection);
                removeSongWordsCommand.Parameters.Add(new SQLiteParameter("@song_id", id));
                removeSongWordsCommand.ExecuteNonQuery();
            }
        }

        public void fixDatabase()
        {
            if (songsConnection.State == System.Data.ConnectionState.Open && songWordsConnection.State == System.Data.ConnectionState.Open)
            {
                var songs = getSongs();

                var removeSongWords = new SQLiteCommand("delete from word", songWordsConnection);
                removeSongWords.ExecuteNonQuery();

                var removeSongTableCommand = new SQLiteCommand("DROP TABLE song", songsConnection);
                removeSongTableCommand.ExecuteNonQuery();
                var createSongTableCommand = new SQLiteCommand("CREATE TABLE song (rowid integer PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, song_item_uid text UNIQUE, song_rev_uid text, song_uid text, title text NOT NULL, author text, copyright text, administrator text, description text, tags text, reference_number text, vendor_id integer, presentation_id integer, layout_revision integer DEFAULT 1, revision integer DEFAULT 1 )", songsConnection);
                createSongTableCommand.ExecuteNonQuery();                

                foreach(var song in songs)
                {
                    song.id = 0;
                    updateOrCreateSong(song);
                }
            }
        }
    }
}
