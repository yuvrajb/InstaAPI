using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Endpoints.OptionalParameters
{
    public class GetUserFollowsParameters
    {
        /// <summary>
        ///     <para>gets or sets the count of the feeds to be fetched</para>
        /// </summary>
        public int Count = 10;
        /// <summary>
        ///     <para>gets or sets the next cursor id for pagination</para>
        /// </summary>
        public String NextCursor = String.Empty;
    }
}
