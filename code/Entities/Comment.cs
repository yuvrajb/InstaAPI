using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class Comment
    {
        /// <summary>
        ///     <para>gets or sets the creation time of the comment</para>
        /// </summary>
        public DateTime CreatedTime;
        /// <summary>
        ///     <para>gets or sets the comment text</para>
        /// </summary>
        public String Text;
        /// <summary>
        ///     <para>gets or sets the user who owns the comment</para>
        /// </summary>
        public User From;
        /// <summary>
        ///     <para>gets or sets the id of the comment</para>
        /// </summary>
        public String Id;
    }
}
