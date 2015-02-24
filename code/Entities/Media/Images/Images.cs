using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class Images
    {
        /// <summary>
        ///     <para>gets or sets the image url</para>
        /// </summary>
        public String url;
        /// <summary>
        ///     <para>gets or sets the media width</para>
        /// </summary>
        public int width;
        /// <summary>
        ///     <para>gets or sets the media height</para>
        /// </summary>
        public int height;
    }
}
