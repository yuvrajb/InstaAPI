using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InstaAPI.Endpoints;

namespace InstaAPI.Entities
{
    [Serializable]
    public class Feeds
    {
        /// <summary>
        ///     <para>gets or sets the pagination</para>
        /// </summary>
        public PaginationIdData Pagination;
        /// <summary>
        ///     <para>gets or sets the response metadata</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gers or sets the list of feeds</para>
        /// </summary>
        public List<FeedData> Data;
    }
}
