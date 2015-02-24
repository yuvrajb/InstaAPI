using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;

using InstaAPI.Entities;
using InstaAPI.Auth;
using InstaAPI.Endpoints;
using InstaAPI.Endpoints.OptionalParameters;
using Newtonsoft.Json;

namespace InstaAPI.Endpoints.Authenticated
{
    [Serializable]
    public class Media
    {
        private InstaConfig Config;
        private AuthUser AuthorisedUser;

        /****************************************************** CONSTRUCTORS *******************************************************/

        /// <summary>
        ///     <para>instantiate with InstaConfig and AuthUser</para>
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="AuthorisedUser"></param>
        public Media(InstaConfig Config, AuthUser AuthorisedUser)
        {
            this.Config = Config;
            this.AuthorisedUser = AuthorisedUser;
        }

        /******************************************************** GETTERS **********************************************************/

        /// <summary>
        ///     <para>gets the media information using media id</para>
        /// </summary>
        /// <param name="MediaId"></param>
        /// <returns></returns>
        public MediaData GetMediaInformation(String MediaId)
        {
            MediaData Media = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();
                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Host += "/media/" + MediaId;

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                Media = new MediaData();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Media.Meta = Meta;

                // SET FEED DATA
                Media.Feed = ParseFeed(ParsedJson);
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
                        Media.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Media;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the media information using shortcode</para>
        /// </summary>
        /// <param name="ShortCode"></param>
        /// <returns></returns>
        public MediaData GetMediaInformationShotcode(String ShortCode)
        {
            MediaData Media = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder(Config.GetUriScheme() + "://" + Config.GetApiUriString() + "/media/shortcode/" + ShortCode);
                //BaseUri.Scheme = Config.GetUriScheme();
                //BaseUri.Host = Config.GetApiUriString();
                //BaseUri.Host += "/media/shortcode/" + ShortCode;

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                Media = new MediaData();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Media.Meta = Meta;

                // SET FEED DATA
                Media.Feed = ParseFeed(ParsedJson);
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
                        Media.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Media;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>searched for media in a given area</para>
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public MediaSearch GetMediaSearchResult(String Latitude, String Longitude, GetMediaSearchResultParameters Parameters)
        {
            MediaSearch Media = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();
                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Host += "/media/search";

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("lat", Latitude);
                QueryString.Add("lng", Longitude);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("count", Parameters.Count.ToString());
                QueryString.Add("min_timestamp", Parameters.MinTimestamp);
                QueryString.Add("max_timestamp", Parameters.Maxtimestamp);
                QueryString.Add("distance", Parameters.Distance);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                Media = new MediaSearch();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Media.Meta = Meta;

                // SET FEEDS DATA
                Media.Data = ParseMediaSearchFeeds(ParsedJson);
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
                        Media.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Media;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the popular posts at the moment</para>
        /// </summary>
        /// <returns></returns>
        public MediaPopular GetMediaPopular()
        {
            MediaPopular Popular = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();
                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Host += "/media/popular";

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                Popular = new MediaPopular();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Popular.Meta = Meta;

                // SET FEEDS DATA
                Popular.Data = ParseMediaSearchFeeds(ParsedJson);
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
                        Popular.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Popular;
        }

        /**************************************************** OTHER METHODS ******************************************************/

        /// <summary>
        ///     <para>parses and returns the FeedData</para>
        /// </summary>
        /// <param name="ParsedJson"></param>
        /// <returns></returns>
        private FeedData ParseFeed(dynamic ParsedJson)
        {
            // SET VARIABLES
            dynamic Post = ParsedJson.data;

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

            return Feed;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>parses and returns the </para>
        /// </summary>
        /// <param name="ParsedJson"></param>
        /// <returns></returns>
        private List<FeedData> ParseMediaSearchFeeds(dynamic ParsedJson)
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
