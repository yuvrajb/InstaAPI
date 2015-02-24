using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InstaAPI.Endpoints;

namespace InstaAPI.Entities
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        ///     <para>gets or sets the user id</para>
        /// </summary>
        public String Id;
        /// <summary>
        ///     <para>gets or sets the user name</para>
        /// </summary>
        public String UserName;
        /// <summary>
        ///     <para>gets or sets the user's full name</para>
        /// </summary>
        public String FullName;
        /// <summary>
        ///     <para>gets or sets the user's profile picture link</para>
        /// </summary>
        public String ProfilePicture;
        /// <summary>
        ///     <para>gets or sets the user's bio</para>
        /// </summary>
        public String Bio = String.Empty;
        /// <summary>
        ///     <para>gets or sets the user's website</para>
        /// </summary>
        public String Website = String.Empty;
        /// <summary>
        ///     <para>gets or sets the user's media count</para>
        /// </summary>
        public long MediaCount = 0;
        /// <summary>
        ///     <para>gets or sets the count of people user follows</para>
        /// </summary>
        public long FollowsCount = 0;
        /// <summary>
        ///     <para>gets or sets the count of people following user</para>
        /// </summary>
        public long FollowedByCount = 0;
        /// <summary>
        ///     <para>gets or sets the api response meta data</para>
        /// </summary>
        public MetaData Meta;
    }
}
