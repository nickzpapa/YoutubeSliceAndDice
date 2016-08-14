using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using YoutubeExtractor;
using System.Linq;
using System.Text.RegularExpressions;

namespace SliceAndDiceWeb
{
    class AlbumDownloader
    {        

        // Downloads album as video and returns file location as a string
        public static string Download(string url, string filepath="./")
        {
            try {
                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);
                VideoInfo videoInfo;

                foreach(VideoInfo video in videoInfos)
                    System.Diagnostics.Debug.WriteLine(video.ToString());

                //Get the best quality
                if (videoInfos.Count() > 0)
                {
                    videoInfo = videoInfos
                        .OrderByDescending(vi => vi.AudioBitrate)
                        .First<VideoInfo>();
                }
                else
                {
                    //TODO: No Videos found!!!!
                    return "Didn't Download!";
                }

                if (videoInfo.RequiresDecryption)
                {
                    DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                }

                // Download and convert to mp3
                string title = Regex.Replace(videoInfo.Title, @"[\/:*?""<>|]*", "");
                string savePath = Path.Combine(filepath, title + videoInfo.VideoExtension);
                VideoDownloader videoDownloader = new VideoDownloader(videoInfo, savePath);
                videoDownloader.Execute();

                return savePath;
            }
            catch(System.ArgumentException e)
            {
                return "Error: URL Provided does not point to YouTube";
            }   
            catch(WebException e)
            {
                return "Error: YouTube is not cooperating... Please try again later.";
            }
        }
    }
}
