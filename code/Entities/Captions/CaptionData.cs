using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class CaptionData
    {
        /// <summary>
        ///     <para>gets or sets the creation time of the caption</para>
        /// </summary>
        public DateTime CreratedTime;
        /// <summary>
        ///     <para>gets or sets the text of the caption</para>
        /// </summary>
        public String Text;
        /// <summary>
        ///     <para>gets or sets the user who owns the caption</para>
        /// </summary>
        public User From;
        /// <summary>
        ///     <para>gets or sets the id of the caption</para>
        /// </summary>
        public String Id;
    }
}
