using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class PaginationIdData
    {
        /// <summary>
        ///     <para>gets or set the next page url</para>
        /// </summary>
        public String NextUrl = String.Empty;
        /// <summary>
        ///     <para>gets or sets the next max id</para>
        /// </summary>
        public String NextMaxId = String.Empty;
    }
}
