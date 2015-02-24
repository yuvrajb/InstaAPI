using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Endpoints.OptionalParameters
{
    public class GetMediaSearchResultParameters
    {
        /// <summary>
        ///     <para>gets or sets the count of media to be searched</para>
        /// </summary>
        public int Count = 10;
        /// <summary>
        ///     <para>gets or sets the minimum timestamp of posts</para>
        /// </summary>
        public String MinTimestamp = String.Empty;
        /// <summary>
        ///     <para>gets or sets the maximum timestamp of posts</para>
        /// </summary>
        public String Maxtimestamp = String.Empty;
        /// <summary>
        ///     <para>gets or sets the maximum distance</para>
        /// </summary>
        public String Distance = "1km";
    }
}
