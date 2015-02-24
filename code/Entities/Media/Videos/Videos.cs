using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class Videos
    {
        /// <summary>
        ///     <para>gets or sets the video url</para>
        /// </summary>
        public String url;
        /// <summary>
        ///     <para>sets or gets the media width</para>
        /// </summary>        
        public int width;
        /// <summary>
        ///     <para>sets or gets the media height</para>
        /// </summary>
        public int height;
    }
}
