# InstaAPI
InstaAPI is a CSharp(#) wrapper for Instagram API build in DotNET Framework 4. It is a simple, small library that uses Instagram's <b>server-side</b> flow for fetching access tokens.
# Quick Note
Started working on it when I couldn't find a simple C# API for Instagram.<br/>
The API may contain some bugs; feel free to raise issues regarding the same. You can also contact me via <br/>
<ul>
	<li>email: yuvrajbabrah@live.com</li>
	<li>facebook: <b>fb.com/yuvraj.babrah</b></li>
	<li>twitter: <b>@yuvrajb</b></li>
</ul>
Yuvraj Singh Babrah, 2015
# Important
In order to post or delete something on Instagram using any API, you first need to get your application approved from Instagram. In case if you try to mention anything besides basic in your scopes you'll encounter an error on the authentication page. More on this: [Instagram Contact](https://help.instagram.com/contact/185819881608116)

You won't face any hiccup while fetching any type of data; be it media, likes or comment.
# Requirements
<ul>
	<li>DotNet Framework 4 or above</li>
	<li>Newtonsoft json library</li>
</ul>
# Quick Walkthrough
	Refer the following quick-guide for using the API
  - <b>Creating InstaConfig Object</b> (to be used for both authenticated and unauthenticated requests)
  ```
  List<Scope> scopes = new List<Scope>() { Scope.basic, Scope.relationships, Scope.likes };
  InstaConfig config = new InstaConfig("CLIENT_ID", "CLIENT_SECRET", "REDIRECT_URI", scopes);
  
  // use this to redirect user for authenticating your application
  String AuthenticationUriString = config.GetAuthenticationUriString();  
   ```
  
  - <b>Creating OAuth Object</b> (to be used for authenticated requests)
  ```
  OAuth oauth = new OAuth(config, "CODE_ATTACHED_WITH_REDIRECTED_URI_AFTER_SUCCESSFUL_AUTHENTICATION");
  AuthUser user = oauth.GetAuhtorisedUser();

  Console.WriteLine(user.AccessToken);
  Console.WriteLine(user.UserId);
  Console.WriteLine(user.UserName);
  Console.WriteLine(user.FullName);
  ```
  <b>You can serialize and save the AuthUser object. This is how you can save the access for the authorized user; hence, there will be no need to re-authorize the application.</b>
  
  - <b>Fetching User Feeds</b>
  ```
  Users CurrentUser = new Users(config, user);
  Feeds feeds = CurrentUser.GetUserPosts("1118571892", new GetUserPostsParameters());
  foreach (var fd in feeds.Data)
  {
      Console.WriteLine(fd.Caption.Text);
      Console.WriteLine(fd.Images.StandardResolution.url);
      Console.WriteLine(fd.User.UserName);
  }
  ```
  
  - <b>Fetching Comments</b>
  ```
  Comments comm = new Comments(config, user);
  MediaComments mc = comm.GetMediaRecentComments("662323776542578144_1118571892");
  foreach (var comment in mc.Data)
  {
      Console.WriteLine(comment.From.UserName);
      Console.WriteLine(comment.Text);
  }
  ```
  
  - <b>Posting a Commment</b>
  ```
  Comments comm = new Comments(config, user);
  MetaData meta = comm.PostMediaComment("662323776542578144_1118571892", "i am a comment from github docs");
  Console.WriteLine(meta.Code);
  ```
  
  I'm sorry for not having a proper html type documentation. Wish Visual Studio could have some easy mechanism as provided in Netbeans. In the mean time, happy coding!
