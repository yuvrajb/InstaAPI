using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class LikesData
    {
        /// <summary>
        ///     <para>gets or sets the total number of likes</para>
        /// </summary>
        public long Count;
        /// <summary>
        ///     <para>gets or sets the list of users</para>
        /// </summary>
        public List<User> Data;
    }
}
