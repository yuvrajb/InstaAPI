using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class TagData
    {
        /// <summary>
        ///     <para>gets or sets the response metadata</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gets or sets the list of tag information</para>
        /// </summary>
        public TagInfo Data;
    }
}
