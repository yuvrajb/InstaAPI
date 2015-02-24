using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Endpoints.OptionalParameters
{
    public class GetUserLikesParameters
    {
        /// <summary>
        ///     <para>gets or sets the count of the feeds to be fetched</para>
        /// </summary>
        public int Count = 10;
        /// <summary>
        ///     <para>gets or sets the maximum id upto which likes need to be fetched</para>
        /// </summary>
        public String MaxLikeId = String.Empty;
    }
}
