using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class TagInfo
    {
        /// <summary>
        ///     <para>gets or sets the count of the media</para>
        /// </summary>
        public long MediaCount;
        /// <summary>
        ///     <para>gets or sets the name of the tag</para>
        /// </summary>
        public String Name;
    }
}
