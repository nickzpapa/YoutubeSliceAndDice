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
            // Download and save album in temp folder
            Payload payload = new Payload();
            string title = AlbumDownloader.Download(url, payload.RootDir);
            payload.SetDirectories(title);

            // Convert to mp3
            List<SongInfo> songList = (!String.IsNullOrWhiteSpace(tracklist)) ? TrackListUtility.FromString(tracklist) : null;
            AlbumUtility albumUtility = new AlbumUtility(payload, songList, artist, album);
            ZipFile.CreateFromDirectory(payload.SongsDir, payload.ZipDir);

            System.Diagnostics.Debug.WriteLine("Zip Location: " + payload.ZipDir);
            return payload.ZipDir;
        }
    }
}