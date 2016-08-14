using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace SliceAndDiceWeb
{    
    // Provides utility to translate string data into song attributes
    class TrackListUtility
    {
        // Matches a time (hh:mm:SS)
        public const string TimeRegex = @"(([0-9]?[0-9]:)?([0-9]?[0-9]?):([0-9]?[0-9]))";


        // Takes a tracklist as a multiline string and retrieves song metadata list
        // TODO: Error checking tracklist input (hints: exactly time per line, etc)
        public static List<SongInfo> FromString(string tracksList)
        {
            List<SongInfo> songList = new List<SongInfo>();
            string[] tracks = Regex.Split(tracksList, Environment.NewLine);           

            for (int i = 0; i < tracks.Length; i++)
            {
                string line = tracks[i];

                // Save and remove start time from line
                string startTime = Regex.Match(line, TimeRegex).Value;
                int startSeconds = toSeconds(startTime);
                line = Regex.Replace(line, TimeRegex, "");

                // Clean up the rest and save as Title
                string title = Regex.Replace(line, @"[\/:*?""<>|]", "");
                title = title.Trim(new char[] { ' ', '-', '(', ')', '{', '}' });

                System.Diagnostics.Debug.WriteLine("{0} - {1}", startTime, title);

                // Add song info to song list
                songList.Add(new SongInfo(title, startTime, startSeconds));
            }
            return songList;        
        }


        // Parses a text file for a list of SongInfo. Used for testing and such
        // TODO: refactored, not sure if works
        public static List<SongInfo> FromFile(string filePath)
        {
            using (StreamReader fin = new StreamReader(filePath))
            {
                string trackList = fin.ReadToEnd();
                return FromString(trackList);
            }
        }


        // Converts StartTime to time in seconds
        private static int toSeconds(string startTime)
        {
            int seconds = 0;
            string[] time = startTime.Split(':');
            for (int i = 0; i < time.Length; i++)
            {
                int t = Int32.Parse(time[i]);
                seconds += t * (int)Math.Pow(60, time.Length - 1 - i);
            }
            return seconds;
        }




    }
}
