using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Endpoints
{
    public class GetLocationRecentMediaParameters
    {
        /// <summary>
        ///     <para>gets or sets the minimum time stamp</para>
        /// </summary>
        public String MinTimeStamp = String.Empty;
        /// <summary>
        ///     <para>gets or sers the maximum time stamp</para>
        /// </summary>
        public String MaxTimeStamp = String.Empty;
        /// <summary>
        ///     <para>gets or sets the minimum post id</para>
        /// </summary>
        public String MinId = String.Empty;
        /// <summary>
        ///     <para>gets or sets the maximum post id</para>
        /// </summary>
        public String MaxId = String.Empty;
    }
}
