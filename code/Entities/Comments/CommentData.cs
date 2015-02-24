using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class CommentData
    {
        /// <summary>
        ///     <para>gets or sets the total number of comments on the media</para>
        /// </summary>
        public long Count;
        /// <summary>
        ///     <para>gets or sets the list of comments</para>
        /// </summary>
        public List<Comment> Data;
    }
}
