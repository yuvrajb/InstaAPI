using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Endpoints
{
    public class GetTagRecentMediaParameters
    {
        /// <summary>
        ///     <para>gets or sets the count of feeds to fetch</para>
        /// </summary>
        public int Count = 10;
        /// <summary>
        ///     <para>gets or sets the minimum tag id</para>
        /// </summary>
        public String MinTagId = String.Empty;
        /// <summary>
        ///     <para>gets or sets the maximum tag id</para>
        /// </summary>
        public String MaxTagId = String.Empty;
    }
}
