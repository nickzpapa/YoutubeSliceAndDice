using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceAndDiceWeb
{
    /// <summary>
    /// This class is used to save song info obtained from youtube as a Title and StartTime
    /// </summary>
    class SongInfo
    {

        public string Title { get; set; }
        public string StartTime { get; set; }

        public SongInfo (string Title, string StartTime)
        {
            this.Title = Title;
            this.StartTime = StartTime;
        }

        /// <summary>
        /// Converts StartTime to time in seconds
        /// </summary>
        /// <returns>The time in seconds of StartTime</returns>
        public int StartTimeInSeconds ()
        {
            int seconds = 0;
            string [] time = StartTime.Split(':');
            for(int i=0; i<time.Length; i++)
            {
                int t = Int32.Parse(time[i]);
                seconds += t * (int)Math.Pow(60, time.Length - 1 - i);
            }
            return seconds;
        }
    }
}
