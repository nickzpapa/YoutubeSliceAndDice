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
        // Slices up an album file into individual song mp3 files and saves in new sub-directory
        // TODO: audit class
        public AlbumUtility(Payload payload, List<SongInfo> songList=null, string artist="", string album="", bool deleteAlbum=false)
        {
            
            string dirName      = Path.GetFileNameWithoutExtension(payload.VideoFileLocation);            
            MediaFile inputFile = new MediaFile(payload.VideoFileLocation);
            

            System.Diagnostics.Debug.WriteLine("Found {0}", payload.VideoFileLocation);
            System.Diagnostics.Debug.WriteLine("Saving songs to {0}", payload.VideoFileLocation);


            using (Engine engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                int albumLength = (Int32)(inputFile.Metadata.Duration.TotalSeconds);

                // Case where there is no tracklist - So just convert whole video to one mp3
                if (songList == null)
                {
                    string songpath      = Path.Combine(payload.SongsDir, payload.VideoTitle + ".mp3");
                    MediaFile outputFile = new MediaFile(songpath);

                    // Convert the video
                    engine.Convert(inputFile, outputFile);

                    // Write the song metadata
                    TagLib.File song = TagLib.File.Create(songpath);
                    song.Tag.Title = payload.VideoTitle;
                    if (album != "")
                        song.Tag.Album = album;
                    if (artist != "")
                    {
                        song.Tag.Artists    = new string[] { artist };
                        song.Tag.Performers = new string[] { artist };
                    }
                    song.Save();
                }

                // Otherwise slice up album
                else
                {
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
                        DBug.Echo("Get the output file for the song");
                        DBug.Echo(payload.SongsDir);
                        DBug.Echo(payload.SongsDir + songList[i].Title + ".mp3");

                        string songpath = Path.Combine(payload.SongsDir, songList[i].Title + ".mp3");
                        DBug.Echo(songpath);

                        MediaFile outputFile = new MediaFile(songpath);

                        // Slice and convert media file type to audio 
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
                            song.Tag.Performers = new string[] { artist };
                        }
                        song.Tag.Track = (uint)i + 1;
                        song.Save();
                    }
                }
            }

            if(deleteAlbum)
            {
                System.Diagnostics.Debug.WriteLine("Deleting temporary video file {0}", payload.VideoFileLocation);
                File.Delete(payload.VideoFileLocation);
            }
        }


    }
}
