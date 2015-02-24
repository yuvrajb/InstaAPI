using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class LocationMedia
    {
        /// <summary>
        ///     <para>gets or sets the response meta data</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gets or sets the pagination</para>
        /// </summary>
        public PaginationIdData Pagination;
        /// <summary>
        ///     <para>gets or sets posts in the list</para>
        /// </summary>
        public List<FeedData> Data;
    }
}
