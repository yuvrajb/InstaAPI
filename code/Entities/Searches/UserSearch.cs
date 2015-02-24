using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{ 
    [Serializable]
    public class UserSearch
    {
        /// <summary>
        ///     <para>gets or sets the response meta data</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gets or sets the list of users</para>
        /// </summary>
        public List<User> Data;
    }
}
