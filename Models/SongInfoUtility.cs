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
    /// <summary>
    /// This class provides methods to obtain a <paramref name="List<<paramref name="SongInfo"/>>"/>
    /// </summary>
    class SongInfoUtility
    {

        /// <summary>
        /// Matches a time (hh:mm:SS)
        /// </summary>
        public const string TimeRegex = @"(([0-9]?[0-9]:)?([0-9]?[0-9]:)([0-9][0-9]))";


        public static List<SongInfo> FromArray(string[] tracks)
        {
            List<SongInfo> songInfoList = new List<SongInfo>();
            for (int i = 0; i < tracks.Length; i++)
            {
                string line = tracks[i];

                // Find and remove StartTime
                string startTime = Regex.Match(line, TimeRegex).Value;
                line = Regex.Replace(line, TimeRegex, "");

                // Clean up the rest and save as Title
                string title = Regex.Replace(line, @"[\/:*?""<>|]", "");
                title = title.Trim(new char[] { ' ', '-', '(', ')', '{', '}' });

                Console.WriteLine("{0} - {1}", startTime, title);

                // Add song info to song list
                songInfoList.Add(new SongInfo(title, startTime));
            }
            return songInfoList;        
        }



        /// <summary>
        /// Parses a text file for a list of SongInfo
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>List of track start times and titles</returns>
        public static List<SongInfo> FromFile(string filePath)
        {
            using (StreamReader fin = new StreamReader(filePath))
            {
                List<SongInfo> songInfoList = new List<SongInfo>();
                while (!fin.EndOfStream)
                {
                    string line = fin.ReadLine();

                    // Find and remove StartTime
                    string startTime = Regex.Match(line, TimeRegex).Value;
                    line = Regex.Replace(line, TimeRegex, "");

                    // Clean up the rest and save as Title
                    string title = Regex.Replace(line, @"[\/:*?""<>|]", "");
                    title = title.Trim(new char[] { ' ', '-', '(', ')', '{', '}' });

                    Console.WriteLine("{0} - {1}", startTime, title);

                    // Add song info to song list
                    songInfoList.Add(new SongInfo(title, startTime));
                }
                return songInfoList;
            }
        }


        /* Junk code below  (Not Used) */

        /// <summary>
        /// Downloads HTML code from a webpage
        /// </summary>
        /// <param name="url">Website URL</param>
        /// <returns>HTML code</returns>
        private static string GetHtml (string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                string htmlCode = client.DownloadString(url);

                using (StreamWriter test = new StreamWriter(Path.Combine(@"C:\Users\Nick\Desktop\test", "HTML_OUTPUT.TXT")))
                {
                    test.Write(htmlCode);
                }                    

                return htmlCode;
            }            
        }


        /// <summary>
        /// Parses a Youtube webpage for song info
        /// Note: Bad implentation. Revise or remove
        /// </summary>
        /// <param name="url"></param>
        /// <returns>List of track start times and titles</returns>
        public static List<SongInfo> Download (string url)
        {
            // Get the chunk of html text to start with (Each line containing song title and start time)
            string html = GetHtml(url);                        
            MatchCollection matches 
                = Regex.Matches(html, TimeRegex + @"(<\/a>)(\s)*([(\p{L})\w\s\.\,\'\""\/\#\!\?\$\%\^\&\*\;\:\{\}\=\-\+_\`\~\(\)\[\]])+(<br\s*\/>)", RegexOptions.CultureInvariant);

            if (matches == null)
                Console.WriteLine("Didnt find anything");

            List<SongInfo> songInfoList = new List<SongInfo>();
            using (StreamWriter test = new StreamWriter(Path.Combine(@"C:\Users\Nick\Desktop\test", "SONGLIST_OUTPUT.TXT")))
            {
                foreach (Match match in matches)
                {
                    // Get the start time first
                    string startTime = Regex.Match(match.Value, TimeRegex).Value;

                    // Cut off everything else that's not part of the title
                    string title = Regex.Replace(match.Value, TimeRegex + @"(<\/a>)(\s)*", "");
                    title = Regex.Replace(title, @"(<br\s*\/>)", "");

                    test.WriteLine("{0} - {1}", startTime, title);
                    Console.WriteLine("{0} - {1}", startTime, title);

                    // Add song info to song list
                    songInfoList.Add(new SongInfo(title, startTime));
                
                }
            }
            return songInfoList;
        }

    }
}
