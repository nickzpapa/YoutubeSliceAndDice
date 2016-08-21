using System;

using System.IO;

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
            return View();
        }

        [HttpPost]  // Request for an sliced album
        public ActionResult Index(string url, string tracklist, string artist, string album)
        {           
            if (String.IsNullOrWhiteSpace(url))
                return View();


            // Download video from youtube, slice it up, save it to a zip file
            string zipLoc = SliceService.DoSlice(url, tracklist, artist, album);
            FileInfo file = new FileInfo(zipLoc);

            // Send back zip file
            try
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Regex.Replace(zipLoc, @"[\/:*?""<>|\s]*", "") + ".zip");
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

    }
}
