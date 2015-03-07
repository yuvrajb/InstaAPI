using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using InstaAPI.Auth;
using InstaAPI.Entities;
using InstaAPI.Endpoints.OptionalParameters;
using InstaAPI.InstaException;
using Newtonsoft.Json;

namespace InstaAPI.Endpoints.Authenticated
{
    [Serializable]
    public class Users
    {
        private InstaConfig Config;
        private AuthUser AuthorisedUser;

        /****************************************************** CONSTRUCTORS *******************************************************/

        /// <summary>
        ///     <para>instantiate with InstaConfig and AuthUser</para>
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="AuthorisedUser"></param>
        public Users(InstaConfig Config, AuthUser AuthorisedUser)
        {
            this.Config = Config;
            this.AuthorisedUser = AuthorisedUser;
        }

        /******************************************************** GETTERS *********************************************************/

        /// <summary>
        ///     <para>gets the basic user information - identified by UserId</para>
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public User GetUserInformation(String UserId){
            User RequestedUser = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "users/" + UserId + "/";

                // SET UP QUERY String
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);

                // SET THE QUERY StringS
                BaseUri.Query = QueryString.ToString();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE NEW USER OBJECT AND FILL IN DATA
                RequestedUser = new User();
                RequestedUser.Id = ParsedJson.id;
                RequestedUser.UserName = ParsedJson.data.username;
                RequestedUser.FullName = ParsedJson.data.full_name;
                RequestedUser.Bio = ParsedJson.data.bio;
                RequestedUser.Website = ParsedJson.data.website;
                RequestedUser.ProfilePicture = ParsedJson.data.profile_picture;
                RequestedUser.MediaCount = ParsedJson.data.counts.media;
                RequestedUser.FollowsCount = ParsedJson.data.counts.follows;
                RequestedUser.FollowedByCount = ParsedJson.data.counts.followed_by;

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                RequestedUser.Meta = Meta;
            }
            catch (WebException WEx)
            {
                // FETCHES ANY ERROR THROWN BY INSTAGRAM API
                Stream ResponseStream = WEx.Response.GetResponseStream();
                if (ResponseStream != null)
                {
                    StreamReader ResponseReader = new StreamReader(ResponseStream);
                    if (ResponseReader != null)
                    {
                        // PARSE JSON   
                        dynamic ParsedJson = JsonConvert.DeserializeObject(ResponseReader.ReadToEnd());

                        // CREATE NEW USER OBJECT AND FILL IN DATA
                        RequestedUser = new User();

                        // CREATE META OBJECT   
                        MetaData Meta = new MetaData();
                        Meta.Code = ParsedJson.meta.code;
                        Meta.ErrorType = ParsedJson.meta.error_type;
                        Meta.ErrorMessage = ParsedJson.meta.error_message;
                        RequestedUser.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return RequestedUser;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the logged in user's feed</para>
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public Feeds GetUserFeeds(GetUserFeedsParameters Parameters)
        {
            Feeds UserFeeds = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();
                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Host += "/users/self/feed";

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("count", Parameters.Count.ToString());
                QueryString.Add("min_id", Parameters.MinId);
                QueryString.Add("max_id", Parameters.MaxId);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                UserFeeds = new Feeds();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);
                
                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                UserFeeds.Meta = Meta;

                // CREATE PAGINATION OBJECT
                PaginationIdData Pagination = new PaginationIdData();
                Pagination.NextUrl = ParsedJson.pagination.next_url;
                Pagination.NextMaxId = ParsedJson.pagination.next_max_id;
                UserFeeds.Pagination = Pagination;

                // SET FEEDS DATA
                UserFeeds.Data = ParseFeeds(ParsedJson);
            }
            catch (WebException WEx)
            {
                // FETCHES ANY ERROR THROWN BY INSTAGRAM API
                Stream ResponseStream = WEx.Response.GetResponseStream();
                if (ResponseStream != null)
                {
                    StreamReader ResponseReader = new StreamReader(ResponseStream);
                    if (ResponseReader != null)
                    {
                        // PARSE JSON
                        dynamic ParsedJson = JsonConvert.DeserializeObject(ResponseReader.ReadToEnd());

                        // CREATE NEW META OBJECT AND FILL IN DATA
                        MetaData Meta = new MetaData();
                        Meta.Code = ParsedJson.meta.code;
                        Meta.ErrorType = ParsedJson.meta.error_type;
                        Meta.ErrorMessage = ParsedJson.meta.error_message;
                        UserFeeds.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return UserFeeds;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>gets all the posts of the user - identified by UserId</para>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public Feeds GetUserPosts(String UserId, GetUserPostsParameters Parameters)
        {
            Feeds UserPosts = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Host += "/users/" + UserId + "/media/recent";

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("count", Parameters.Count.ToString());
                QueryString.Add("max_timestamp", Parameters.MaxTimeStamp.ToString());
                QueryString.Add("min_timestamp", Parameters.MinTimeStamp.ToString());
                QueryString.Add("min_id", Parameters.MinId);
                QueryString.Add("max_id", Parameters.MaxId);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                UserPosts = new Feeds();

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                UserPosts.Meta = Meta;

                // CREATE PAGINATION OBJECT
                PaginationIdData Pagination = new PaginationIdData();
                Pagination.NextUrl = ParsedJson.pagination.next_url;
                Pagination.NextMaxId = ParsedJson.pagination.next_max_id;
                UserPosts.Pagination = Pagination;

                // SET FEEDS DATA
                UserPosts.Data = ParseFeeds(ParsedJson);
            }
            catch (WebException WEx)
            {
                // FETCHES ANY ERROR THROWN BY INSTAGRAM API
                Stream ResponseStream = WEx.Response.GetResponseStream();
                if (ResponseStream != null)
                {
                    StreamReader ResponseReader = new StreamReader(ResponseStream);
                    if (ResponseReader != null)
                    {
                        // PARSE JSON
                        dynamic ParsedJson = JsonConvert.DeserializeObject(ResponseReader.ReadToEnd());

                        // CREATE NEW META OBJECT AND FILL IN DATA
                        MetaData Meta = new MetaData();
                        Meta.Code = ParsedJson.meta.code;
                        Meta.ErrorType = ParsedJson.meta.error_type;
                        Meta.ErrorMessage = ParsedJson.meta.error_message;
                        UserPosts.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return UserPosts;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>get the logged in user's likes</para>
        /// </summary>
        /// <param name="Count"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public Feeds GetUserLikes(GetUserLikesParameters Parameters)
        {
            Feeds UserLikes = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Host += "/users/self/media/liked";

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("count", Parameters.Count.ToString());
                QueryString.Add("max_like_id", Parameters.MaxLikeId);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                UserLikes = new Feeds();

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                UserLikes.Meta = Meta;

                // CREATE PAGINATION OBJECT
                PaginationIdData Pagination = new PaginationIdData();
                Pagination.NextUrl = ParsedJson.pagination.next_url;
                Pagination.NextMaxId = ParsedJson.pagination.next_max_id;
                UserLikes.Pagination = Pagination;

                // SET FEEDS DATA
                UserLikes.Data = ParseFeeds(ParsedJson);
            }
            catch (WebException WEx)
            {
                // FETCHES ANY ERROR THROWN BY INSTAGRAM API
                Stream ResponseStream = WEx.Response.GetResponseStream();
                if (ResponseStream != null)
                {
                    StreamReader ResponseReader = new StreamReader(ResponseStream);
                    if (ResponseReader != null)
                    {
                        // PARSE JSON
                        dynamic ParsedJson = JsonConvert.DeserializeObject(ResponseReader.ReadToEnd());

                        // CREATE NEW META OBJECT AND FILL IN DATA
                        MetaData Meta = new MetaData();
                        Meta.Code = ParsedJson.meta.code;
                        Meta.ErrorType = ParsedJson.meta.error_type;
                        Meta.ErrorMessage = ParsedJson.meta.error_message;
                        UserLikes.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return UserLikes;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the search result</para>
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public UserSearch GetUserSearchResult(GetUserSearchResultParameters Parameters)
        {
            UserSearch SearchResult = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Host += "/users/search";

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("q", Parameters.Query);
                QueryString.Add("count", Parameters.Count.ToString());

                // SET THE QUERY STRING
                BaseUri.Query = QueryString.ToString();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE SEARCH RESULT OBJECT
                SearchResult = new UserSearch();

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                SearchResult.Meta = Meta;

                // SET DATA FIELD IN SEARCHRESULT
                List<User> SearchedUsers = new List<User>();
                foreach (dynamic EachUser in ParsedJson.data)
                {
                    // CREATE NEW USER OBJECT
                    User EUser = new User();
                    EUser.UserName = EachUser.username;
                    EUser.Bio = EachUser.bio;
                    EUser.Website = EachUser.website;
                    EUser.ProfilePicture = EachUser.profile_picture;
                    EUser.FullName = EachUser.full_name;
                    EUser.Id = EachUser.id;

                    // ADD EUSER TO THE LIST
                    SearchedUsers.Add(EUser);
                }
                SearchResult.Data = SearchedUsers;
            }
            catch (WebException WEx)
            {
                // FETCHES ANY ERROR THROWN BY INSTAGRAM API
                Stream ResponseStream = WEx.Response.GetResponseStream();
                if (ResponseStream != null)
                {
                    StreamReader ResponseReader = new StreamReader(ResponseStream);
                    if (ResponseReader != null)
                    {
                        // PARSE JSON
                        dynamic ParsedJson = JsonConvert.DeserializeObject(ResponseReader.ReadToEnd());

                        // CREATE NEW META OBJECT AND FILL IN DATA
                        MetaData Meta = new MetaData();
                        Meta.Code = ParsedJson.meta.code;
                        Meta.ErrorType = ParsedJson.meta.error_type;
                        Meta.ErrorMessage = ParsedJson.meta.error_message;
                        SearchResult.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return SearchResult;
        }

        /**************************************************** OTHER METHODS ******************************************************/

        /// <summary>
        ///     <para>parses and returns the FeedData in the form of list</para>
        /// </summary>
        /// <param name="ParsedJson"></param>
        /// <returns></returns>
        private List<FeedData> ParseFeeds(dynamic ParsedJson)
        {
            // CREATE FEEDDATA LIST
            List<FeedData> Data = new List<FeedData>();

            foreach (dynamic Post in ParsedJson.data)
            {
                // CREATE FEEDDATA OBJECT
                FeedData Feed = new FeedData();

                // CREATE ATTRIBUTION OBJECT;
                if (Post.attribution == null)
                {
                    Feed.Attribution = null;
                }
                else
                {
                    AttributionData Attribution = new AttributionData();
                    Attribution.Website = Post.Attribution.website;
                    Attribution.ItunesUrl = Post.Attribution.itunes_url;
                    Attribution.Name = Post.Attribution.name;
                    Feed.Attribution = Attribution;
                }

                // SET TAGS
                List<String> Tags = new List<String>();
                foreach (dynamic Tag in Post.tags)
                {
                    Tags.Add(Tag.ToString());
                }
                Feed.Tags = Tags;

                // SET TYPE
                Feed.Type = Post.type;

                // SET LOCATION
                if (Post.location == null)
                {
                    Feed.Location = null;
                }
                else
                {
                    LocationData Location = new LocationData();
                    Location.Id = Post.location.id;
                    Location.Latitude = Post.location.latitude;
                    Location.Longitude = Post.location.longitude;
                    Location.Name = Post.location.name;
                    Feed.Location = Location;
                }

                // SET COMMENTS
                CommentData Comments = new CommentData();
                Comments.Count = Post.comments.count;
                List<Comment> CommentData = new List<Comment>();
                foreach (dynamic EachComment in Post.comments.data)
                {
                    // CREATE COMMENT OBJECT
                    Comment Comment = new Comment();
                    Comment.CreatedTime = new DateTime(long.Parse(EachComment.created_time.ToString()));
                    Comment.Id = EachComment.id;
                    Comment.Text = EachComment.text;

                    // CREATE USER OBJECT
                    User CommentedBy = new User();
                    CommentedBy.UserName = EachComment.from.username;
                    CommentedBy.ProfilePicture = EachComment.from.profile_pciture;
                    CommentedBy.Id = EachComment.from.id;
                    CommentedBy.FullName = EachComment.from.full_name;

                    // ASSOCIATE COMMENT WITH USER
                    Comment.From = CommentedBy;

                    // ADD COMMENT TO THE LIST
                    CommentData.Add(Comment);
                }
                Comments.Data = CommentData;
                Feed.Comments = Comments;

                // SET FILTER
                Feed.Filter = Post.filter;

                // SET CREATED TIME
                Feed.CreatedTime = new DateTime(long.Parse(Post.created_time.ToString()));

                // SET LINK 
                Feed.Link = Post.link;

                // SET LIKES
                LikesData Likes = new LikesData();
                Likes.Count = Post.likes.count;
                List<User> LikedByUsers = new List<User>();
                foreach (dynamic EachLike in Post.likes.data)
                {
                    // CREATE USER OBJECT
                    User LikedBy = new User();
                    LikedBy.UserName = EachLike.username;
                    LikedBy.ProfilePicture = EachLike.profile_picture;
                    LikedBy.Id = EachLike.id;
                    LikedBy.FullName = EachLike.full_name;

                    // ADD USER TO THE LIST
                    LikedByUsers.Add(LikedBy);
                }
                Likes.Data = LikedByUsers;
                Feed.Likes = Likes;

                // SET VIDEO
                if (Feed.Type.Equals("video"))
                {
                    VideosData VideoData = new VideosData();
                    LowResolutionVideo LRVideo = new LowResolutionVideo();
                    LRVideo.url = Post.videos.low_resolution.url;
                    LRVideo.width = Post.videos.low_resolution.width;
                    LRVideo.height = Post.videos.low_resolution.height;
                    VideoData.LowResolution = LRVideo;
                    StandardResolutionVideo SRVideo = new StandardResolutionVideo();
                    SRVideo.url = Post.videos.standard_resolution.url;
                    SRVideo.width = Post.videos.standard_resolution.width;
                    SRVideo.height = Post.videos.standard_resolution.height;
                    VideoData.StandardResolution = SRVideo;

                    Feed.Videos = VideoData;
                }
                else
                {
                    Feed.Videos = null;
                }

                // SET IMAGES
                ImagesData Images = new ImagesData();
                StandardResolutionImage SRImage = new StandardResolutionImage();
                SRImage.url = Post.images.standard_resolution.url;
                SRImage.width = Post.images.standard_resolution.width;
                SRImage.height = Post.images.standard_resolution.height;
                Images.StandardResolution = SRImage;
                ThumbnailImage TImage = new ThumbnailImage();
                TImage.url = Post.images.thumbnail.url;
                TImage.width = Post.images.thumbnail.width;
                TImage.height = Post.images.thumbnail.height;
                Images.Thumbnail = TImage;
                LowResolutionImage LRImage = new LowResolutionImage();
                LRImage.url = Post.images.low_resolution.url;
                LRImage.width = Post.images.low_resolution.width;
                LRImage.height = Post.images.low_resolution.height;
                Images.LowResolution = LRImage;
                Feed.Images = Images;

                // SET CAPTIONS
                CaptionData Caption = new CaptionData();
                if (Post.caption != null)
                {
                    Caption.CreratedTime = new DateTime(long.Parse(Post.caption.created_time.ToString()));
                    Caption.Text = Post.caption.text;
                    Caption.Id = Post.caption.id;
                    User CaptionedBy = new User();
                    CaptionedBy.UserName = Post.caption.from.username;
                    CaptionedBy.ProfilePicture = Post.caption.from.profile_pciture;
                    CaptionedBy.Id = Post.caption.from.id;
                    CaptionedBy.FullName = Post.caption.from.full_name;
                    Caption.From = CaptionedBy;
                }
                Feed.Caption = Caption;

                // SET TAGGED USER
                List<TaggedUser> UserInPhotos = new List<TaggedUser>();
                if (Post.users_in_photo != null)
                {
                    foreach (dynamic UserTag in Post.users_in_photo)
                    {
                        // CREATE TAGGED USER OBJECT
                        TaggedUser TUser = new TaggedUser();

                        // SET USER
                        User TaggedUser = new User();
                        TaggedUser.UserName = UserTag.user.username;
                        TaggedUser.FullName = UserTag.user.full_name;
                        TaggedUser.Id = UserTag.user.id;
                        TaggedUser.ProfilePicture = UserTag.user.profile_picture;
                        TUser.User = TaggedUser;

                        // SET POSITION
                        Position TagPosition = new Position();
                        TagPosition.x = float.Parse(UserTag.position.x.ToString());
                        TagPosition.y = float.Parse(UserTag.position.y.ToString());
                        TUser.Position = TagPosition;

                        // ADD TO LIST
                        UserInPhotos.Add(TUser);
                    }
                }
                Feed.UsersInPhoto = UserInPhotos;

                // SET USER LIKE
                Feed.UserHasLiked = Post.user_has_liked;

                // SET ID
                Feed.Id = Post.id;

                // SET USER
                User FeedBy = new User();
                FeedBy.UserName = Post.user.username;
                FeedBy.Website = Post.user.webste;
                FeedBy.ProfilePicture = Post.user.profile_picture;
                FeedBy.FullName = Post.user.full_name;
                FeedBy.Bio = Post.user.bio;
                FeedBy.Id = Post.user.id;
                Feed.User = FeedBy;

                // ADD FEED TO LIST
                Data.Add(Feed);
            }

            return Data;
        }
    }
}
