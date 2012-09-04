Library for Authenticating with Facebook OAuth and populating the Profile object with information from the user's open graph. 
It is designed to work with FormsAuthentication, it will create users and populate their profile when they log in & authorise your application.

This is built with IoC in mind from the beginning. The `IFacebookAuthenticationProvider` is the mockable inteface that you can 
inject into your application, and if you're using Castle.Windsor the library has a `IWindsorInstaller` so if you include the VineMedia.Facebook assembly in your container installer the authentication provider will be available automatically.

There are two projects in this solution
 1. VineMedia.Facebook - This is the library for using Facebook
 2. VineMedia.Facebook.Web - This is a demo MVC4 application showing the authentication working.

## Usage ##

There are two parts to the authentication process:
1. If the current user isn't authenticated, redirecting them to Facebook
2. Handling the OAuth callback and turning that into an authenticated user with a profile.

On your login page, do this:

```c#	
	FacebookAuthProvider.Authenticate();
```

This will bounce the user to Facebook's OAuth page if they aren't authenticated.  Facebook will then redirect them back to `facebookouth.axd` on your domain with an authentication code.

To handle this callback there is a `HttpHandler` that will pickup this request and convert it into an authenticated user, creating the `Membership` & `Profile` data as needed.  Register this handler in your web.config:

```xml
<system.webServer>
	<handlers>
		<add name="FaceBookOAuth" path="facebookouth.axd" verb="POST,GET,HEAD" type="VineMedia.Facebook.FacebookOAuthHandler, VineMedia.Facebook" preCondition="integratedMode" />
	</handlers>
</system.webServer>
```

## Project References ##

1. facebook-csharp-sdk/facebook-csharp-sdk