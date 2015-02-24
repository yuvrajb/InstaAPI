using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Endpoints.OptionalParameters
{
    public class GetUserPostsParameters
    {
        /// <summary>
        ///     <para>gets or sets the count of the feeds to be fetched</para>
        /// </summary>
        public int Count = 10;
        /// <summary>
        ///     <para>gets or sets the maximum timestamp upto which posts need to be fetched</para>
        /// </summary>
        public ulong MaxTimeStamp = ulong.MinValue;
        /// <summary>
        ///     <para>gets or sets the minimum timestamp from where posts need to be fetched</para>
        /// </summary>
        public ulong MinTimeStamp = ulong.MinValue;
        /// <summary>
        ///     <para>gets or sets the minimum id up from where the posts need to be fetched</para>
        /// </summary>
        public String MinId = String.Empty;
        /// <summary>
        ///     <para>gets or set the maximum id upto which posts need to be fetched</para>
        /// </summary>
        public String MaxId = String.Empty;
    }
}
