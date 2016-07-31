using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceAndDiceWeb
{
    class AlbumUtility
    {
        /// <summary>
        /// Slices up an album mp3 file into individual song mp3 files and saves in new sub-directory
        /// </summary>
        /// <param name="filepathIn">The filepath where the album mp3 file is stored</param>
        /// <param name="filepathOut">The directory where songs will be stored</param>
        /// <param name="songList">Info needed to cut each song</param>
        /// <param name="deleteAlbum">Option to delete original youtube file</param>
        public AlbumUtility(string filepathIn, string filepathOut, List<SongInfo> songList, string artist="", string album="", bool deleteAlbum=true)
        {
            string dirName = Path.GetFileNameWithoutExtension(filepathIn);
            
            MediaFile inputFile = new MediaFile(filepathIn);
            Console.WriteLine("Found {0}", filepathIn);
            Console.WriteLine("Saving songs to {0}", filepathOut);

            using (Engine engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                int albumLength = (int)(inputFile.Metadata.Duration.TotalSeconds);
                Console.WriteLine("Album Length {0}", albumLength);
                for (int i = 0; i < songList.Count; i++)
                {
                    // Get start time and and length of song
                    int timeStart, timeLength;
                    timeStart = songList[i].StartTimeInSeconds();
                    if (i + 1 < songList.Count)
                    {
                        timeLength = songList[i + 1].StartTimeInSeconds() - timeStart;
                    }
                    else {
                        timeLength = albumLength - timeStart;
                    }

                    // Get the output file for the song
                    string songpath = Path.Combine(filepathOut, songList[i].Title + ".mp3");
                    MediaFile outputFile = new MediaFile(songpath);

                    // Cut the song
                    ConversionOptions options = new ConversionOptions();
                    options.CutMedia(TimeSpan.FromSeconds(timeStart), TimeSpan.FromSeconds(timeLength));
                    engine.Convert(inputFile, outputFile, options);
                    Console.WriteLine("{0}-{1} {2}", timeStart, timeStart + timeLength, songpath);

                    // Write the song metadata
                    TagLib.File song = TagLib.File.Create(songpath);
                    song.Tag.Title = songList[i].Title;
                    if (album != "")
                        song.Tag.Album = album;
                    if (artist != "")
                    {
                        song.Tag.Artists = new string[] { artist };
                        song.Tag.AlbumArtists = new string[] { artist };
                        song.Tag.Performers = new string[] { artist };
                    }
                    song.Tag.Track = (uint)i + 1;
                    song.Save();
                }
            }

            if(deleteAlbum)
            {
                Console.WriteLine("Deleting temporary album file {0}", filepathIn);
                File.Delete(filepathIn);
            }
        }

        //static void Main(string[] args)
        //{
        //    Console.Write("Enter YouTube URL: ");
        //    string url = Console.ReadLine();
        //    Console.Write("Downloading Album... ");
        //    string filepath = AlbumDownloader.Download(url);
        //    Console.WriteLine("Finished.");
        //    Console.WriteLine();

        //    Console.Write("Enter the C://path/to/tracklist/file.txt: ");
        //    string songListLoc = Console.ReadLine();
        //    Console.Write("Getting Track Info... ");
        //    List<SongInfo> songList = SongInfoUtility.FromFile(songListLoc);
        //    Console.Write("Finished.");
        //    Console.WriteLine();

        //    Console.Write("What\'s the name of the artist?: ");
        //    string artist = Console.ReadLine();
        //    Console.Write("What\'s the name of the album?: ");
        //    string album = Console.ReadLine();

        //    Console.WriteLine("Awesome slicing up the album now...");
        //    AlbumUtility albumUtility = new AlbumUtility(filepath, songListLoc, songList, artist, album);
        //    Console.Write("All finished. The album should be saved at {0}", Path.GetDirectoryName(songListLoc));
        //    Console.WriteLine();

        //    Console.Read();
        //}
    }
}
