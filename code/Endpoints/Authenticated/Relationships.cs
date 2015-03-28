using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;

using InstaAPI.Auth;
using InstaAPI.Entities;
using InstaAPI.Endpoints.OptionalParameters;
using InstaAPI.InstaException;
using Newtonsoft.Json;

namespace InstaAPI.Endpoints.Authenticated
{
    [Serializable]
    public class Relationships
    {
        private InstaConfig Config;
        private AuthUser AuthorisedUser;

        /****************************************************** CONSTRUCTORS *******************************************************/

        /// <summary>
        ///     <para>instantiate with InstaConfig and AuthUser</para>
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="AuthorisedUser"></param>
        public Relationships(InstaConfig Config, AuthUser AuthorisedUser)
        {
            this.Config = Config;
            this.AuthorisedUser = AuthorisedUser;
        }

        /******************************************************** GETTERS *********************************************************/

        /// <summary>
        ///     <para>gets the list of users that a user follows</para>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public UserFollows GetUserFollows(String UserId, GetUserFollowsParameters Parameters)
        {
            UserFollows Follows = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "users/" + UserId + "/follows";

                // SET UP QUERY String
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("count", Parameters.Count.ToString());
                QueryString.Add("cursor", Parameters.NextCursor);

                // SET THE QUERY StringS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW USER FOLLOWS OBJECT
                Follows = new UserFollows();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Follows.Meta = Meta;

                // CREATE PAGINATION OBJECT
                PaginationCursorData Pagination = new PaginationCursorData();
                Pagination.NextUrl = ParsedJson.pagination.next_url;
                Pagination.NextCursor = ParsedJson.pagination.next_cursor;
                Follows.Pagination = Pagination;

                // CREATE DATA LIST
                List<User> Data = new List<User>();
                foreach (dynamic EachUser in ParsedJson.data)
                {
                    // CREATE AND FILL USER OBJECT
                    User User = new User();
                    User.UserName = EachUser.username;
                    User.Bio = EachUser.bio;
                    User.Website = EachUser.website;
                    User.ProfilePicture = EachUser.profile_picture;
                    User.FullName = EachUser.full_name;
                    User.Id = EachUser.id;

                    // ADD USER TO THE LIST
                    Data.Add(User);
                }
                Follows.Data = Data;
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
                        Follows.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Follows;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para1>gets the list of users who follow the specified user</para1>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public UserFollowedBy GetUserFollowedBy(String UserId, GetUserFollowedByParameters Parameters)
        {
            UserFollowedBy FollowedBy = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "users/" + UserId + "/followed-by";

                // SET UP QUERY String
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("count", Parameters.Count.ToString());
                QueryString.Add("cursor", Parameters.NextCursor);

                // SET THE QUERY StringS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW USER FOLLOWS OBJECT
                FollowedBy = new UserFollowedBy();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                FollowedBy.Meta = Meta;

                // CREATE PAGINATION OBJECT
                PaginationCursorData Pagination = new PaginationCursorData();
                Pagination.NextUrl = ParsedJson.pagination.next_url;
                Pagination.NextCursor = ParsedJson.pagination.next_cursor;
                FollowedBy.Pagination = Pagination;

                // CREATE DATA LIST
                List<User> Data = new List<User>();
                foreach (dynamic EachUser in ParsedJson.data)
                {
                    // CREATE AND FILL USER OBJECT
                    User User = new User();
                    User.UserName = EachUser.username;
                    User.Bio = EachUser.bio;
                    User.Website = EachUser.website;
                    User.ProfilePicture = EachUser.profile_picture;
                    User.FullName = EachUser.full_name;
                    User.Id = EachUser.id;

                    // ADD USER TO THE LIST
                    Data.Add(User);
                }
                FollowedBy.Data = Data;
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
                        FollowedBy.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return FollowedBy;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the list of users who have requested to follow</para>
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public UserRequestedBy GetUserRequestedBy(GetUserRequestedByParameters Parameters)
        {
            UserRequestedBy Requests = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "users/self/requested-by";

                // SET UP QUERY String
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);
                QueryString.Add("count", Parameters.Count.ToString());
                QueryString.Add("cursor", Parameters.NextCursor);

                // SET THE QUERY StringS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW USER FOLLOWS OBJECT
                Requests = new UserRequestedBy();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Requests.Meta = Meta;

                if (ParsedJson.pagination == null)
                {
                    Requests.Pagination = null;
                }
                else
                {
                    // CREATE PAGINATION OBJECT
                    PaginationCursorData Pagination = new PaginationCursorData();
                    Pagination.NextUrl = ParsedJson.pagination.next_url;
                    Pagination.NextCursor = ParsedJson.pagination.next_cursor;
                    Requests.Pagination = Pagination;
                }

                // CREATE DATA LIST
                List<User> Data = new List<User>();
                foreach (dynamic EachUser in ParsedJson.data)
                {
                    // CREATE AND FILL USER OBJECT
                    User User = new User();
                    User.UserName = EachUser.username;
                    User.ProfilePicture = EachUser.profile_picture;
                    User.Id = EachUser.id;

                    // ADD USER TO THE LIST
                    Data.Add(User);
                }
                Requests.Data = Data;
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
                        Requests.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Requests;
        }

        /**************************************************************************************************************************/

        /// <summary>
        ///     <para>gets information about relationship to another user</para>
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public UserRelationship GetUserRelationship(String UserId)
        {
            UserRelationship Relationship = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "users/"+ UserId +"/relationship";

                // SET UP QUERY String
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);

                // SET THE QUERY StringS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW USER FOLLOWS OBJECT
                Relationship = new UserRelationship();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Relationship.Meta = Meta;

                // CREATE DATA OBJECT
                RelationshipData Data = new RelationshipData();
                Data.OutgoingStatus = ParsedJson.data.outgoing_status;
                Data.IncomingStatus = ParsedJson.data.incoming_status;
                Data.TargetUserIsPrivate = ParsedJson.data.target_user_is_private;
                Relationship.Data = Data;
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
                        Relationship.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Relationship;
        }

        /******************************************************** POSTERS *********************************************************/

        /// <summary>
        ///     <post>pushes the new relationship data to the api</post>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public UserRelationship PostUserRelationship(String UserId, RelationshipTypes Type)
        {
            UserRelationship Relationship = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();

                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "users/" + UserId + "/relationship";

                // SET UP QUERY String
                NameValueCollection PostStrings = System.Web.HttpUtility.ParseQueryString(String.Empty);
                PostStrings.Add("access_token", AuthorisedUser.AccessToken);
                PostStrings.Add("action", Type.ToString());

                // CREATE NEW USER FOLLOWS OBJECT
                Relationship = new UserRelationship();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.UploadValues(BaseUri.Uri, PostStrings);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Relationship.Meta = Meta;

                // CREATE DATA OBJECT
                RelationshipData Data = new RelationshipData();
                Data.OutgoingStatus = ParsedJson.data.outgoing_status;
                Data.IncomingStatus = String.Empty;
                Data.TargetUserIsPrivate = ParsedJson.data.target_user_is_private;
                Relationship.Data = Data;
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
                        Relationship.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Relationship;
        }
    }
}
