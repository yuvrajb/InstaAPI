using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class Location
    {
        private InstaConfig Config;
        private AuthUser AuthorisedUser;

        /****************************************************** CONSTRUCTORS *******************************************************/

        /// <summary>
        ///     <para>instantiate with InstaConfig and AuthUser</para>
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="AuthorisedUser"></param>
        public Location(InstaConfig Config, AuthUser AuthorisedUser)
        {
            this.Config = Config;
            this.AuthorisedUser = AuthorisedUser;
        }

        /******************************************************** GETTERS **********************************************************/

        /// <summary>
        ///     <para>gets information about a location</para>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public LocationInfoData GetLocationInformation(String LocationId)
        {
            LocationInfoData LocInfo = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder(Config.GetUriScheme() + "://" + Config.GetApiUriString() + "/locations/" + LocationId);

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                LocInfo = new LocationInfoData();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                LocInfo.Meta = Meta;

                // SET DATA
                LocationData Data = new LocationData();
                Data.Id = ParsedJson.data.id;
                Data.Name = ParsedJson.data.name;
                Data.Latitude = ParsedJson.data.latitude;
                Data.Longitude = ParsedJson.data.longitude;
                LocInfo.Data = Data;
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
                        LocInfo.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return LocInfo;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>gets a list of media objects from a given location</para>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public LocationMedia GetLocationRecentMedia(String LocationId, GetLocationRecentMediaParameters Parameters) 
        {
            LocationMedia Media = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();
                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Host += "/locations/" + LocationId + "/media/recent";

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("min_timestamp", Parameters.MinTimeStamp);
                QueryString.Add("max_timestamp", Parameters.MaxTimeStamp);
                QueryString.Add("min_id", Parameters.MinId);
                QueryString.Add("max_id", Parameters.MaxId);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                Media = new LocationMedia();

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

                // CREATE PAGINATION OBJECT
                PaginationIdData Pagination = new PaginationIdData();
                Pagination.NextUrl = ParsedJson.pagination.next_url;
                Pagination.NextMaxId = ParsedJson.pagination.next_max_id;
                Media.Pagination = Pagination;

                // SET DATA
                Media.Data = ParseFeeds(ParsedJson);
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
        ///     <para>searched for location by geographic coordinate</para>
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public LocationSearch GetLocationSearchResult(String Latitude, String Longitude, GetLocationSearchResultParameters Parameters)
        {
            LocationSearch SearchResult = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder(Config.GetUriScheme() + "://" + Config.GetApiUriString() + "/locations/search");

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("lat", Latitude);
                QueryString.Add("lng", Longitude);
                QueryString.Add("distance", Parameters.Distance);
                QueryString.Add("facebook_places_id", Parameters.FacebookPlacesId);
                QueryString.Add("foursquare_v2_id", Parameters.FoursquareV2Id);

                // SET THE QUERY STRING
                BaseUri.Query = QueryString.ToString();

                Console.WriteLine(BaseUri.Uri.ToString());

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE SEARCH RESULT OBJECT
                SearchResult = new LocationSearch();

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                SearchResult.Meta = Meta;

                // SET DATA FIELD IN SEARCHRESULT
                List<LocationData> Data = new List<LocationData>();
                foreach (dynamic EachLocation in ParsedJson.data)
                {
                    // CREATE NEW LOCATIONDATA OBJECT
                    LocationData Location = new LocationData();
                    Location.Id = EachLocation.id;
                    Location.Name = EachLocation.name;
                    Location.Latitude = EachLocation.latitude;
                    Location.Longitude = EachLocation.longitude;

                    // ADD LOCATION TO THE LIST
                    Data.Add(Location);
                }
                SearchResult.Data = Data;
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
