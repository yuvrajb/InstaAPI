using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class TagSearch
    {
        /// <summary>
        ///     <para>gets or sets the response metada</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gets or sets the tag info in a list</para>
        /// </summary>
        public List<TagInfo> Data;
    }
}
