using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using YoutubeExtractor;
using System.Linq;
using System.Text.RegularExpressions;
using SliceAndDiceWeb.Models;

namespace SliceAndDiceWeb
{
    class AlbumDownloader
    {        

        // Downloads album as video and returns file name as a string
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
                    throw new System.Exception("Could not download from YouTube");;
                }

                if (videoInfo.RequiresDecryption)
                {
                    DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                }

                // Download and convert to mp3
                string title = Regex.Replace(videoInfo.Title, @"[\/:*?""<>|]*", "");
                string fileName = title + videoInfo.VideoExtension;
                string savePath = Path.Combine(filepath, fileName);
                VideoDownloader videoDownloader = new VideoDownloader(videoInfo, savePath);
                videoDownloader.Execute();

                return fileName;
            }
            catch(System.ArgumentException e)
            {
                throw new SliceException("URL Provided does not point to YouTube");
            }   
            catch(WebException e)
            {
                throw new System.Exception("YouTube is not cooperating with this particular video. Please try another");
            }
        }
    }
}
