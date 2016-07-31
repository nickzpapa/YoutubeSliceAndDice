using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;


namespace SliceAndDiceWeb.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeLog()
        {
            return View();
        }

        
        [HttpPost]  // Request for an sliced album
        public ActionResult Index(string url, string tracklist, string artist, string album)
        {           

            if (String.IsNullOrWhiteSpace(url))
                return View();           
            

            // Download album to random directory
            string albumDir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            Directory.CreateDirectory(albumDir);
            string albumLoc = AlbumDownloader.Download(url, albumDir);
            string videoTitle = Path.GetFileNameWithoutExtension(albumLoc);


            //Check for errors
            if(albumLoc.Contains("Error"))
            {
                return Content(albumLoc);
            }


            System.Diagnostics.Debug.WriteLine("Album Location: " + albumLoc);

            // Create Dir for songs
            string songDirParent = Path.Combine( Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            string songDir = Path.Combine(songDirParent, videoTitle);
            Directory.CreateDirectory(songDir);

            // Zip Path for finished files 
            string zipLoc = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()), videoTitle);
            Directory.CreateDirectory(zipLoc);
            zipLoc = Path.Combine(zipLoc, videoTitle + ".zip");

            System.Diagnostics.Debug.WriteLine("Zip Location: " + zipLoc);

            if (!String.IsNullOrWhiteSpace(tracklist))
            {
                string [] tracks = Regex.Split(tracklist, Environment.NewLine);
                List<SongInfo> songList = SongInfoUtility.FromArray(tracks);
                AlbumUtility albumUtility = new AlbumUtility(albumLoc, songDir, songList, artist, album);
                ZipFile.CreateFromDirectory(songDirParent, zipLoc);
                //    Console.Write("All finished. The album should be saved at {0}", Path.GetDirectoryName(songListLoc));
                //    Console.WriteLine();     
            }
            else
            {
                ZipFile.CreateFromDirectory(albumDir, zipLoc);
            }
                   
            // Send finished Zip File
            FileInfo file = new FileInfo(zipLoc);
            try
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Regex.Replace(videoTitle, @"[\/:*?""<>|\s]*", "") + ".zip");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(file.FullName);
                Response.End();
            }
            catch(HttpException ex)
            {
                return null;
            }   return null;
       
        }


        // GET: Main/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Main/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Main/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Main/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Main/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Main/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Main/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
