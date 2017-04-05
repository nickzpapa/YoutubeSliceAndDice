using SliceAndDiceWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;



namespace SliceAndDiceWeb
{
    public class MainController : Controller
    {

        // the page
        public ActionResult Index()
        {
            Session["state"] = "Initialized";
            return View();
        }


        [HttpPost]  // Slice an album an store on server
        public ActionResult Index(string url, string tracklist, string artist, string album)
        {
            try {
                if (String.IsNullOrWhiteSpace(url) || Session["state"] != "Initialized")
                    return View();

                // Download video from youtube, slice it up, save it to a zip file
                Payload payload = new Payload();
                string title = AlbumDownloader.Download(url, payload.RootDir);
                payload.SetDirectories(title);

                // Convert to mp3
                List<SongInfo> songList = (!String.IsNullOrWhiteSpace(tracklist)) ? TrackListUtility.FromString(tracklist) : null;
                AlbumUtility albumUtility = new AlbumUtility(payload, songList, artist, album);
                ZipFile.CreateFromDirectory(payload.SongsDir, payload.ZipDir);

                System.Diagnostics.Debug.WriteLine("Zip Location: " + payload.ZipDir);
                FileInfo file = new FileInfo(payload.ZipDir);

                // Set headers
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Regex.Replace(payload.ZipDir, @"[\/:*?""<>|\s]*", "") + ".zip");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/json";

                return Json(new
                {
                    Loc = payload.Key + "/" + payload.VideoTitle,
                    Error = "none"
                });

            }
            catch (SliceException e)
            {
                return Json(new { Error = e.Message });
            }
            //catch (Exception e)
            //{
            //    return Json(new { Error = "unknown" });
            //}

        }


        // Download the requested album
        public ActionResult Download (string key, string filename)
        {            
            FileInfo file = new FileInfo(Payload.GetStorageDir(key, filename) + ".zip");

            System.Diagnostics.Debug.WriteLine("Payload.GetStorageDir(key) {0}" + Payload.GetStorageDir(key));
            // Send back zip file
            try
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(file.FullName);
                Response.End();
            }
            catch (HttpException ex)
            {
                return Content(ex.Message);
            }
            return null;
        }

    }
}
