using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceAndDiceWeb
{

    // Contains the metadata needed to slice the media file
    struct SongInfo
    {
        public string Title;
        public string StartTime;    // hh:mm:ss
        public int StartSeconds;    // total seconds

        public SongInfo (string Title, string StartTime, int StartSeconds)
        {
            this.Title = Title;
            this.StartTime = StartTime;
            this.StartSeconds = StartSeconds;
        }


    }
}
