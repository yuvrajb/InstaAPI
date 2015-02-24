using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class AuthUser
    {
        /// <summary>
        ///     <para>gets or sets the authorised access token</para>
        /// </summary>
        public String AccessToken;
        /// <summary>
        ///     <para>gets or sets the id of the user</para>
        /// </summary>
        public String UserId;
        /// <summary>
        ///     <para>gets or set the username of the user</para>
        /// </summary>
        public String UserName;
        /// <summary>
        ///     <para>gets or sets the full name of the user</para>
        /// </summary>
        public String FullName;
        /// <summary>
        ///     <para>gers or sets the profile picture link of the user</para>
        /// </summary>
        public String ProfilePicture;
        /// <summary>
        ///     <para>gets or sets the bio of the authorised user</para>
        /// </summary>
        public String Bio;
        /// <summary>
        ///     <para>gets or sets the website of the authorised user</para>
        /// </summary>
        public String Website;
    }
}
