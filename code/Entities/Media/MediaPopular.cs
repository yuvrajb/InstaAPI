using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class MediaPopular
    {
        /// <summary>
        ///     <para>gets or sets the response metadata</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gets or sets the popular feeds in the list</para>
        /// </summary>
        public List<FeedData> Data;
    }
}
