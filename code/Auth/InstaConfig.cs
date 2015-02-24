using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InstaAPI.InstaException;

namespace InstaAPI.Auth
{
    [Serializable]
    public class InstaConfig
    {
        private String UriScheme = "https";
        private String ApiUri = "api.instagram.com/v1";
        private String OAuthUri = "api.instagram.com/oauth/authorize";
        private String ClientId = "";
        private String ClientSecret = "";
        private String RedirectUri = "";
        private String Scope = "";
        private String AccessTokenRequestUri = "";
        private List<Scope> ScopesList = null;
        private UriBuilder AuthenticationUri = null;

        /*************************************************************** CONSTRUCTORS ****************************************************************/

        /// <summary>
        ///     <para>default constructor</para>
        /// </summary>
        public InstaConfig()
        {
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <pata>initialise with required parameters</pata>
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="ClientSecret"></param>
        /// <param name="RedirectUri"></param>
        /// <param name="ScopesList"></param>
        public InstaConfig(String ClientId, String ClientSecret, String RedirectUri, List<Scope> ScopesList)
        {
            this.ClientId = ClientId;
            this.ClientSecret = ClientSecret;
            this.RedirectUri = RedirectUri;
            this.ScopesList = ScopesList;
            this.Scope = ScopesToString();
        }
    
        /****************************************************************** SETTERS ******************************************************************/

        /// <summary>
        ///     <para>sets the client id</para>
        /// </summary>
        /// <param name="ClientId"></param>
 
        public void SetClientId(String ClientId)
        {
            this.ClientId = ClientId;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>sets the client secret</para>
        /// </summary>
        /// <param name="ClientSecret"></param>
        public void SetClientSecret(String ClientSecret)
        {
            this.ClientSecret = ClientSecret;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>sets the redirect uri</para>
        /// </summary>
        /// <param name="RedirectUri"></param>
        public void SetRedirectUri(String RedirectUri)
        {
            this.RedirectUri = RedirectUri;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>sets the required scope for the app</para>
        /// </summary>
        /// <param name="ScopesList"></param>
        public void SetScopes(List<Scope> ScopesList)
        {
            this.ScopesList = ScopesList;
            this.Scope = ScopesToString();
        }

        /****************************************************************** GETTERS ******************************************************************/

        /// <summary>
        ///     <para>gets the client id</para>
        /// </summary>
        /// <returns></returns>
        public String GetClientId()
        {
            return ClientId;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the client secret</para>
        /// </summary>
        /// <returns></returns>
        public String GetClientSecret()
        {
            return ClientSecret;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the string representatio of redirect uri</para>
        /// </summary>
        /// <returns></returns>
        public String GetRedirectUriString()
        {
            return RedirectUri;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the uri scheme</para>
        /// </summary>
        /// <returns></returns>
        public String GetUriScheme()
        {
            return UriScheme;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the redirect uri</para>
        /// </summary>
        /// <returns></returns>
        public Uri GetRedirectUri()
        {
            return new Uri(RedirectUri);
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the string representation of the uri to be used for authenticating the user</para>
        /// </summary>
        /// <returns></returns>
        public String GetOAuthUriString()
        {
            return OAuthUri;
        }

        /********************************************************************************************************************************************/
        
        /// <summary>
        ///     <para>gets the uri to be used for authenticating the user</para>
        /// </summary>
        /// <returns></returns>
        public Uri GetOAuthUri()
        {
            return new Uri(OAuthUri);
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the string representation of the uri to be used for making request to instagram</para>
        /// </summary>
        /// <returns></returns>
        public String GetApiUriString()
        {
            return ApiUri;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the uri to be used for making request to instagram</para>
        /// </summary>
        /// <returns></returns>
        public Uri GetApiUri()
        {
            return new Uri(ApiUri);
        }

        /********************************************************************************************************************************************/
        
        /// <summary>
        ///     <para>gets the string representation of the uri to be used by user for authentication</para>
        /// </summary>
        /// <returns></returns>
        public String GetAuthenticationUriString()
        {
            AuthenticationUri = new UriBuilder();

            AuthenticationUri.Scheme = this.UriScheme;
            AuthenticationUri.Host = OAuthUri;
            
            NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(String.Empty);
            QueryString["client_id"] = this.ClientId;
            QueryString["redirect_uri"] = this.RedirectUri;
            QueryString["scope"] = this.Scope;
            QueryString["response_type"] = "code";

            AuthenticationUri.Query = QueryString.ToString();
            AccessTokenRequestUri = AuthenticationUri.Uri.ToString();

            return AccessTokenRequestUri;
        }

        /********************************************************************************************************************************************/

        /// <summary>
        ///     <para>gets the uri to be used by user for authentication</para>
        /// </summary>
        /// <returns></returns>
        public Uri GetAuthenticationUri()
        {
            return new Uri(GetAuthenticationUriString());
        }

        /************************************************************** OTHER METHODS ****************************************************************/

        /// <summary>
        ///     <para>sets up scope in the required format as described in instagram api</para>
        /// </summary>
        /// <returns></returns>
        private String ScopesToString()
        {
            StringBuilder ScopesBuilder = new StringBuilder();

            int len = this.ScopesList.Count;

            /**
             * RAISE EXCEPTION IF NO SCOPE HAS BEEN DEFINED
             */ 
            if (len == 0)
            {
                throw new NoScopeDefined();
            }

            for (int i = 0; i < len; i++)
            {
                InstaAPI.Auth.Scope Current = this.ScopesList[i];
                if (Current == InstaAPI.Auth.Scope.basic)
                {
                    ScopesBuilder.Append("basic");
                }
                else if (Current == InstaAPI.Auth.Scope.comments)
                {
                    ScopesBuilder.Append("comments");
                }
                else if (Current == InstaAPI.Auth.Scope.likes)
                {
                    ScopesBuilder.Append("likes");
                }
                else if (Current == InstaAPI.Auth.Scope.relationships)
                {
                    ScopesBuilder.Append("relationships");
                }

                if (i != len - 1)
                {
                    ScopesBuilder.Append("+");
                }
            }

            return ScopesBuilder.ToString();
        }
    }
}