using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SliceAndDiceWeb
{
    public class SliceService
    {

        // Download a youtube, slice if needed, save it to a zipfile, return zipfile location 
        public static string DoSlice(string url, string tracklist, string artist, string album)
        {            
            // Create a temp folder
            string albumDir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            Directory.CreateDirectory(albumDir);

            // Download and save album in temp folder
            string albumLoc = AlbumDownloader.Download(url, albumDir);
            string videoTitle = Path.GetFileNameWithoutExtension(albumLoc);


            // Check for errors
            // TODO: seriously i dont even
            if (albumLoc.Contains("Error"))
            {
                return "error";
            }
            System.Diagnostics.Debug.WriteLine("Album Location: " + albumLoc);


            // Create song directory in temp directory
            string songDirParent = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            string songDir = Path.Combine(songDirParent, videoTitle);
            Directory.CreateDirectory(songDir);

            // Create zip file for requested data
            string zipLoc = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()), videoTitle);
            Directory.CreateDirectory(zipLoc);
            zipLoc = Path.Combine(zipLoc, videoTitle + ".zip");                        


            // Slice up the tracklist
            if (!String.IsNullOrWhiteSpace(tracklist))
            {
                List<SongInfo> songList = TrackListUtility.FromString(tracklist);
                AlbumUtility albumUtility = new AlbumUtility(albumLoc, songDir, songList, artist, album);
                ZipFile.CreateFromDirectory(songDirParent, zipLoc);
            }          
            // TODO: why is this here ???
            else
            {
                ZipFile.CreateFromDirectory(albumDir, zipLoc);
            }


            System.Diagnostics.Debug.WriteLine("Zip Location: " + zipLoc);
            return zipLoc;
        }
    }
}