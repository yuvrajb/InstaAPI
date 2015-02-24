using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Endpoints.OptionalParameters
{
    public class GetUserFeedsParameters
    {
        /// <summary>
        ///     <para>gets or sets the count of the feeds to be fetched</para>
        /// </summary>
        public int Count = 10;
        /// <summary>
        ///     <para>gets or sets the minimum id up from where the posts need to be fetched</para>
        /// </summary>
        public String MinId = String.Empty;
        /// <summary>
        ///     <para>gets or set he maximum id upto which the posts need to be fetched</para>
        /// </summary>
        public String MaxId = String.Empty;
    }
}
