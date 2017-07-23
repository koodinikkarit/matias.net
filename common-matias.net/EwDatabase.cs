using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using DCSoft.RTF;

namespace common_matias
{
    public class EwDatabase
    {
        SQLiteConnection songsConnection;
        SQLiteConnection songWordsConnection;


        string songsDatabasePath;
        string songWordsDatabasePath;

        public string SongsDatabasePath {
            get
            {
                return songsDatabasePath;
            }
            set
            {
                Console.WriteLine("songsPath " + value);
                songsDatabasePath = value;
                songsConnection = new SQLiteConnection("Data Source=" + value + ";Version=3;");
                songsConnection.Open();
            }
        }
        public string SongWordsDatabasePath
        {
            get
            {
                return songWordsDatabasePath;
            }
            set
            {
                songWordsDatabasePath = value;
                songWordsConnection = new SQLiteConnection("Data Source=" + value + ";Version=3;");
                songWordsConnection.Open();
            }
        }

        private string databasePath;

        public string DatabasePath
        {
            get { return databasePath; }
            set {
                databasePath = value;
                songsConnection = new SQLiteConnection("Data Source=" + value + "/Data/Songs.db;Version=3;");
                songWordsConnection = new SQLiteConnection("Data Source=" + value + "/Data/SongWords.db;Version=3;");
                songsConnection.Open();
                songWordsConnection.Open();
            }
        }


        public EwDatabase(
            string songsDatabasePath,
            string songsWordsDatabasePath
        )
        {
            SongsDatabasePath = songsDatabasePath;
            SongWordsDatabasePath = songsWordsDatabasePath;
        }

        public EwDatabase()
        {

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
                        title = songsReader["title"] != null ? (string)songsReader["title"] : "",
                        author = songsReader["author"] != null ? (string)songsReader["author"] : "",
                        copyright = songsReader["copyright"] != null ? (string)songsReader["copyright"] : "",
                        administrator = songsReader["administrator"] != null ? (string)songsReader["administrator"] : "",
                        //description = songsReader["description"] != null ? (string)songsReader["description"] : "",
                        //tags = songsReader["tags"] != null ? (string)songsReader["tags"] : ""
                        verses = new List<Verse>()
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

                    var verses = (string)songWordsReader["words"];

                    var versesPlainText = RichTextStripper.StripRichTextFormat(verses);

                    foreach (var verse in versesPlainText.Split(new string[] { "\n\r\n" }, StringSplitOptions.None))
                    {
                        song.verses.Add(new Verse
                        {
                            text = verse
                        });
                    }
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
                        title = (string)songReader["title"],
                        author = (string)songReader["author"],
                        copyright = (string)songReader["copyright"],
                        administrator = (string)songReader["administrator"],
                        description = (string)songReader["description"],
                        tags = (string)songReader["tags"]
                    };
                }
                if (song != null)
                {
                    DCSoft.RTF.RTFDomDocument doc = new RTFDomDocument();
                    while (songWordsReader.Read())
                    {
                        var verses = (string)songWordsReader["words"];

                        try
                        {
                            doc.LoadRTFText(verses);
                            foreach (var verse in doc.InnerText.Split(new string[] { "\n\r\n" }, StringSplitOptions.None))
                            {
                                song.verses.Add(new Verse
                                {
                                    text = verse
                                });
                            }
                        }
                        catch
                        {

                        }
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

        public void updateOrCreateSong(Song song)
        {
            if (songsConnection.State == System.Data.ConnectionState.Open && songWordsConnection.State == System.Data.ConnectionState.Open)
            {
                if (song.id != null && songExists((int) song.id))
                {
                    var updateSongCommand = new SQLiteCommand("update song set title = ?, author = ?, copyright =  ?, administrator = ?, description = ?, tags = ? where rowid = ?", songsConnection);
                    updateSongCommand.Parameters.Add(song.title);
                    updateSongCommand.Parameters.Add(song.author);
                    updateSongCommand.Parameters.Add(song.copyright);
                    updateSongCommand.Parameters.Add(song.administrator);
                    updateSongCommand.Parameters.Add(song.description);
                    updateSongCommand.Parameters.Add(song.tags);
                    updateSongCommand.Parameters.Add(song.id);

                    updateSongCommand.ExecuteNonQuery();

                    string words = @"{\rtf1{\pard" + String.Join(@"\par\par", song.verses.Select(p => p.text)) + "}}";

                    var updateSongWordsCommand = new SQLiteCommand("update word set words = ? where song_id = ?", songWordsConnection);

                    updateSongWordsCommand.Parameters.Add(words);

                    updateSongWordsCommand.ExecuteNonQuery();


                } else
                {
                    var insertSongCommand = new SQLiteCommand("insert into song (title, author, copyright, administrator, description, tags) values(?, ?, ?, ?, ?, ?)", songsConnection);
                    insertSongCommand.Parameters.Add(song.title);
                    insertSongCommand.Parameters.Add(song.author);
                    insertSongCommand.Parameters.Add(song.copyright);
                    insertSongCommand.Parameters.Add(song.administrator);
                    insertSongCommand.Parameters.Add(song.description);
                    insertSongCommand.Parameters.Add(song.tags);

                    insertSongCommand.ExecuteNonQuery();

                    var songId = songsConnection.LastInsertRowId;

                    string words = @"{\rtf1{\pard" + String.Join(@"\par\par", song.verses.Select(p => p.text)) + "}}";

                    var insertSongWordsCommand = new SQLiteCommand("insert into word (song_id, words) values (?, ?)", songWordsConnection);
                    insertSongWordsCommand.Parameters.Add(songId);
                    insertSongWordsCommand.Parameters.Add(words);

                    insertSongWordsCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
