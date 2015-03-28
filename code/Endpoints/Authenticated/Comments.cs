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
    public class Comments
    {
        private InstaConfig Config;
        private AuthUser AuthorisedUser;

         /****************************************************** CONSTRUCTORS *******************************************************/

        /// <summary>
        ///     <para>instantiate with InstaConfig and AuthUser</para>
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="AuthorisedUser"></param>
        public Comments(InstaConfig Config, AuthUser AuthorisedUser)
        {
            this.Config = Config;
            this.AuthorisedUser = AuthorisedUser;
        }

        /******************************************************** GETTERS **********************************************************/

        /// <summary>
        ///     <para>gets the list of recent comments on a media object</para>
        /// </summary>
        /// <param name="MediaId"></param>
        /// <returns></returns>
        public MediaComments GetMediaRecentComments(String MediaId)
        {
            MediaComments Comments = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder(Config.GetUriScheme() + "://" + Config.GetApiUriString() + "/media/" + MediaId + "/comments");

                // SET UP QUERY STRING
                NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryString.Add("access_token", AuthorisedUser.AccessToken);

                // SET THE QUERY STRINGS
                BaseUri.Query = QueryString.ToString();

                // CREATE NEW FEEDS OBJECT AND FILL IN DATA
                Comments = new MediaComments();

                // SEND REQUEST 
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.DownloadData(BaseUri.Uri);
                String Response = Encoding.UTF8.GetString(ResponseData);

                // PARSE JSON
                dynamic ParsedJson = JsonConvert.DeserializeObject(Response);

                // CREATE META OBJECT
                MetaData Meta = new MetaData();
                Meta.Code = ParsedJson.meta.code;
                Comments.Meta = Meta;

                // SET FEED DATA
                List<Comment> Data = new List<Comment>();
                foreach (dynamic EachComment in ParsedJson.data)
                {
                    Comment Comment = new Comment();
                    Comment.CreatedTime = new DateTime(long.Parse(EachComment.created_time.ToString()));
                    Comment.Id = EachComment.id;
                    Comment.Text = EachComment.text;
                    User CommentedBy = new User();
                    CommentedBy.Id = EachComment.from.id;
                    CommentedBy.UserName = EachComment.from.username;
                    CommentedBy.ProfilePicture = EachComment.from.profile_picture;
                    CommentedBy.FullName = EachComment.from.full_name;
                    Comment.From = CommentedBy;

                    Data.Add(Comment);
                }
                Comments.Data = Data;
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
                        Comments.Meta = Meta;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
            }

            return Comments;
        }

        /******************************************************** POSTERS **********************************************************/

        /// <summary>
        ///     <para>creates a comment on a media object</para>
        ///     <para>restricted to specific applications</para>
        ///     <para>refer https://instagram.com/developer/endpoints/comments/#post_media_comments</para>
        /// </summary>
        /// <param name="MediaId"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public MetaData PostMediaComment(String MediaId, String Text)
        {
            MetaData Meta = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder();
                BaseUri.Scheme = Config.GetUriScheme();
                BaseUri.Host = Config.GetApiUriString();
                BaseUri.Path = "media/" + MediaId + "/comments";

                // SET UP QUERY String
                NameValueCollection PostStrings = System.Web.HttpUtility.ParseQueryString(String.Empty);
                PostStrings.Add("access_token", AuthorisedUser.AccessToken);
                PostStrings.Add("text", Text);

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
        ///     <para>remove a comment</para>
        /// </summary>
        /// <param name="MediaId"></param>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        public MetaData DeleteMediaComment(String MediaId, String CommentId)
        {
            MetaData Meta = null;

            try
            {
                // SET UP REQUEST URI
                UriBuilder BaseUri = new UriBuilder(Config.GetUriScheme() + "://" + Config.GetApiUriString() + "/media/" + MediaId + "/comments/" + CommentId);

                // SET UP QUERY String
                NameValueCollection QueryStrings = System.Web.HttpUtility.ParseQueryString(String.Empty);
                QueryStrings.Add("access_token", AuthorisedUser.AccessToken);
                BaseUri.Query = QueryStrings.ToString();

                // CREATE NEW USER FOLLOWS OBJECT
                Meta = new MetaData();

                // SEND REQUEST
                WebClient Client = new WebClient();
                byte[] ResponseData = Client.UploadData(BaseUri.Uri, "DELETE", new byte[] {0});
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
