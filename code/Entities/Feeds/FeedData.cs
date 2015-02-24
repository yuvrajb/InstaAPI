using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class FeedData
    {
        /// <summary>
        ///     <para>gets or sets the attribution</para>
        /// </summary>
        public AttributionData Attribution;
        /// <summary>
        ///     <para>gets or sets the list of tags with media</para>
        /// </summary>
        public List<String> Tags;
        /// <summary>
        ///     <para>gets or sets the type of media</para>
        /// </summary>
        public String Type;
        /// <summary>
        ///     <para>gets or sets the location data of the media</para>
        /// </summary>
        public LocationData Location;
        /// <summary>
        ///     <para>gets or sets the comment data of the media</para>
        /// </summary>
        public CommentData Comments;
        /// <summary>
        ///     <para>gets or sets the filter of the media</para>
        /// </summary>
        public String Filter;
        /// <summary>
        ///     <para>gets or sets the created time of the media</para>
        /// </summary>
        public DateTime CreatedTime;
        /// <summary>
        ///     <para>gets or sets the instagram link to the media</para>
        /// </summary>
        public String Link;
        /// <summary>
        ///     <para>gets or sets the likes data of the media</para>
        /// </summary>
        public LikesData Likes;
        /// <summary>
        ///     <para>gets or sets the videos data of the media if applicable</para>
        /// </summary>
        public VideosData Videos;
        /// <summary>
        ///     <para>gets or sets the images data of the media</para>
        /// </summary>
        public ImagesData Images;
        /// <summary>
        ///     <para>gets or sets the caption data of the media</para>
        /// </summary>
        public CaptionData Caption;
        /// <summary>
        ///     <para>gets or sets whether authorised user has liked the media</para>
        /// </summary>
        public Boolean UserHasLiked;
        /// <summary>
        ///     <para>gets or sets the list of tagged users in the media</para>
        /// </summary>
        public List<TaggedUser> UsersInPhoto;
        /// <summary>
        ///     <para>gets or sets the id of the media</para>
        /// </summary>
        public String Id;
        /// <summary>
        ///     <para>gets or sets the user who owns the media</para>
        /// </summary>
        public User User;
    }
}
