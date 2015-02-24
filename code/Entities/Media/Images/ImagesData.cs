using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class ImagesData
    {
        /// <summary>
        ///     <para>gets or sets the object of LowResolutionImage</para>
        /// </summary>
        public LowResolutionImage LowResolution;
        /// <summary>
        ///     <para>gets or sets the object of ThumbnailImage</para>
        /// </summary>
        public ThumbnailImage Thumbnail;
        /// <summary>
        ///     <para>gets or sets the object of StandardResolutionImage</para>
        /// </summary>
        public StandardResolutionImage StandardResolution;
    }
}
