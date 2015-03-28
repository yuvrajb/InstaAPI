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
    public class Likes
    {
        private InstaConfig Config;
        private AuthUser AuthorisedUser;

        /****************************************************** CONSTRUCTORS *******************************************************/

        /// <summary>
        ///     <para>instantiate with InstaConfig and AuthUser</para>
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="AuthorisedUser"></param>
        public Likes(InstaConfig Config, AuthUser AuthorisedUser)
        {
            this.Config = Config;
            this.AuthorisedUser = AuthorisedUser;
        }

        /******************************************************** GETTERS **********************************************************/

        /// <summary>
        ///     <para>gets the list of recent likes on a media object</para>
        /// </summary>
        /// <param name="MediaId"></param>
        /// <returns></returns>
        public MediaLikes GetMediaRecentLikes(String MediaId)
        {
            MediaLikes Likes = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder(Config.GetUriScheme() + "://" + Config.GetApiUriString() + "/media/" + MediaId + "/likes");

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                Likes = new MediaLikes();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Likes.Meta = Meta;

                // SET FEED DATA
                List<User> Data = new List<User>();
                foreach (dynamic EachUser in ParsedJson.data)
                {
                    User LikedBy = new User();
                    LikedBy.Id = EachUser.id;
                    LikedBy.UserName = EachUser.username;
                    LikedBy.ProfilePicture = EachUser.profile_picture;
                    LikedBy.FullName = EachUser.full_name;
                    LikedBy.Bio = EachUser.bio;
                    LikedBy.Website = EachUser.website;

                    Data.Add(LikedBy);
                }
                Likes.Data = Data;
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
                        Likes.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Likes;
        }

        /******************************************************** POSTERS **********************************************************/

        /// <summary>
        ///     <para>sets like on a media by authorised user</para>
        /// </summary>
        /// <param name="MediaId"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public MetaData PostMediaLike(String MediaId)
        {
            MetaData Meta = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();
                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "media/" + MediaId + "/likes";

                // SET UP QUERY String
                NameValueCollection PostStrings = System.Web.HttpUtility.ParseQueryString(String.Empty);
                PostStrings.Add("access_token", AuthorisedUser.AccessToken);

                // CREATE NEW USER FOLLOWS OBJECT
                Meta = new MetaData();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.UploadValues(BaseUri.Uri, PostStrings);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                Meta.Code = ParsedJson.meta.code;
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
                        Meta.Code = ParsedJson.meta.code;
                        Meta.ErrorType = ParsedJson.meta.error_type;
                        Meta.ErrorMessage = ParsedJson.meta.error_message;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Meta;
        }

        /******************************************************** DELETERS *********************************************************/

        /// <summary>
        ///     <para>removes a like on a media</para>
        /// </summary>
        /// <param name="MediaId"></param>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        public MetaData DeleteMediaLike(String MediaId)
        {
            MetaData Meta = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();
                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "media/" + MediaId + "/likes";

                // SET UP QUERY String
                NameValueCollection QueryStrings = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryStrings.Add("access_token", AuthorisedUser.AccessToken);
                BaseUri.Query = QueryStrings.ToString();

                // CREATE NEW USER FOLLOWS OBJECT
                Meta = new MetaData();

                // SEND REQUEST
                WebClient Client = new WebClient();
                String Response = Client.UploadString(BaseUri.Uri, "DELETE", String.Empty);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                Meta.Code = ParsedJson.meta.code;
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
                        if (ResponseReader.Peek() == 60)
                        {
                            Meta.Code = 500;
                            Meta.ErrorMessage = "No data received";
                            Meta.ErrorType = "Scope error";
                        }
                        else
                        {
                            // PARSE JSON
                            dynamic ParsedJson = JsonConvert.DeserializeObject(ResponseReader.ReadToEnd());

                            // CREATE NEW META OBJECT AND FILL IN DATA
                            Meta.Code = ParsedJson.meta.code;
                            Meta.ErrorType = ParsedJson.meta.error_type;
                            Meta.ErrorMessage = ParsedJson.meta.error_message;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
            }

            return Meta;
        }
    }
}
