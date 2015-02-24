using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class TagMedia
    {
        /// <summary>
        ///     <para>gets or sets the pagination data</para>
        /// </summary>
        public PaginationTagData Pagination;
        /// <summary>
        ///     <para>gets or sets the response metadata</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gets or sets the feeds in the list</para>
        /// </summary>
        public List<FeedData> Data;
    }
}
