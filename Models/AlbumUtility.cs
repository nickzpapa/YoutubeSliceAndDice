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
        // Slices up an album mp3 file into individual song mp3 files and saves in new sub-directory
        // TODO: audit class
        public AlbumUtility(string filepathIn, string filepathOut, List<SongInfo> songList, string artist="", string album="", bool deleteAlbum=true)
        {
            string dirName = Path.GetFileNameWithoutExtension(filepathIn);
            
            MediaFile inputFile = new MediaFile(filepathIn);
            System.Diagnostics.Debug.WriteLine("Found {0}", filepathIn);
            System.Diagnostics.Debug.WriteLine("Saving songs to {0}", filepathOut);

            using (Engine engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                int albumLength = (Int32)(inputFile.Metadata.Duration.TotalSeconds);
                System.Diagnostics.Debug.WriteLine("Album Length {0}", albumLength);
                for (int i = 0; i < songList.Count; i++)
                {
                    // Get start time and and length of song
                    int timeLength = 0;
                    if (i + 1 < songList.Count)
                    {
                        timeLength = songList[i + 1].StartSeconds - songList[i].StartSeconds;
                    }
                    else {
                        timeLength = albumLength - songList[i].StartSeconds;
                    }

                    // Get the output file for the song
                    string songpath = Path.Combine(filepathOut, songList[i].Title + ".mp3");
                    MediaFile outputFile = new MediaFile(songpath);

                    // Cut the song
                    ConversionOptions options = new ConversionOptions();
                    options.CutMedia(TimeSpan.FromSeconds(songList[i].StartSeconds), TimeSpan.FromSeconds(timeLength));
                    engine.Convert(inputFile, outputFile, options);
                    System.Diagnostics.Debug.WriteLine("{0}-{1} {2}", songList[i].StartSeconds, songList[i].StartSeconds + timeLength, songpath);

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
                System.Diagnostics.Debug.WriteLine("Deleting temporary album file {0}", filepathIn);
                File.Delete(filepathIn);
            }
        }


    }
}
