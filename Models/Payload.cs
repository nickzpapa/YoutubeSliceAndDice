using System;
using System.IO;

namespace SliceAndDiceWeb
{
    public class Payload
    {
        public string Key,
            RootDir,
            VideoTitle,
            VideoFileName,
            VideoFileLocation,
            SongsDir,
            ZipDir;

        public Payload ()
        {
            this.Key = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            this.RootDir = GetStorageDir(this.Key);
            Directory.CreateDirectory(this.RootDir);
        }

        public static string GetStorageDir(string key="", string filename="")
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Stuff", key, filename);
        }

        public void SetDirectories(string videoFileName)
        {
            // Save downloaded video file info
            this.VideoTitle = Path.GetFileNameWithoutExtension(videoFileName);
            this.VideoFileName = videoFileName;
            this.VideoFileLocation = Path.Combine(this.RootDir, this.VideoFileName);       

            // Create song directory
            this.SongsDir = Path.Combine(RootDir, "songs", this.VideoTitle);
            Directory.CreateDirectory(SongsDir);

            // Create Zip output dir
            this.ZipDir = Path.Combine(RootDir, this.VideoTitle) + ".zip";
            Directory.CreateDirectory(SongsDir);
        }
    }
}