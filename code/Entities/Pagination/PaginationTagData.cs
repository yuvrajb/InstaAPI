using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class PaginationTagData
    {
        /// <summary>
        ///     <para>gets or set the next page url</para>
        /// </summary>
        public String NextUrl = String.Empty;
        /// <summary>
        ///     <para>gets or sets the next max tag id</para>
        /// </summary>
        public String NextMaxTagId = String.Empty;
    }
}
