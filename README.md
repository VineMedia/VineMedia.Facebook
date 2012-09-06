Library for Authenticating with Facebook OAuth and populating the Profile object with information from the user's open graph. 
It is designed to work with FormsAuthentication, it will create users and populate their profile when they log in & authorise your application.

There are two projects in this solution
 1. VineMedia.Facebook - This is the library for using Facebook
 2. VineMedia.Facebook.Web - This is a demo MVC4 application showing the authentication working.

## Usage ##

There are two parts to the authentication process:
 1. If the current user isn't authenticated, redirecting them to Facebook
 2. Handling the OAuth callback and turning that into an authenticated user with a profile.

First off you need to have a [Facebook application](https://developers.facebook.com/apps) to authenticate with.  From there you will need the App ID/API Key & App secret values. These need to be provided to the authentication service.

The core of the authentication process is the `FacebookAuthenticationProvider` class. This needs to be instantiated in your project. There is a mockable `IFacebookAuthenticationProvider` interface that you can use for dependency injection, and if you're using Castle.Windsor, if you call

 ```c#
 container.Install(FromAssembly.Containing<IFacebookAuthenticationProvider>())
 ```

 it will make the `FacebookAuthenticationProvider` available to your container.  
 
 To pass the Facebook App ID and Secret to the provider you can either set these application settings in your web.config

 ```xml
<applicationSettings>
    <VineMedia.Facebook.Properties.Settings>
        <setting name="FacebookAppId" serializeAs="String">
            <value />
        </setting>
        <setting name="FacebookAppSecret" serializeAs="String">
            <value />
        </setting>
    </VineMedia.Facebook.Properties.Settings>
</applicationSettings>
```

or again if you're using IoC you can instantiate the `FacebookConfig` class and add it to your container. There is an example of pulling this data out of a DB & injecting it with Castle [here](https://github.com/VineMedia/VineMedia.Facebook/blob/master/VineMedia.Facebook.Web/FacebookConfigInstaller.cs)

Ok, assuming you have an instance of the `FacebookAuthenticationProvider` available called `FacebookAuthProvider`, on your login page, do this:

```c#	
	FacebookAuthProvider.Authenticate();
```

This will bounce the user to Facebook's OAuth page if they aren't authenticated.  Facebook will then redirect them back to `facebookouth.axd` on your domain with an authentication code.

To handle this callback there is a `HttpHandler` that will pickup this request and convert it into an authenticated user, creating the `Membership` & `Profile` data as needed.  Register this handler in your web.config:

```xml
<system.webServer>
	<handlers>
		<add name="FaceBookOAuth" path="facebookoauth.axd" verb="POST,GET,HEAD" type="VineMedia.Facebook.FacebookOAuthHandler, VineMedia.Facebook" preCondition="integratedMode" />
	</handlers>
</system.webServer>
```

The `facebookoauth.axd` can be customised by setting the `OAuthCallbackPath` on the `FacebookConfig` class (or setting the value in the application settings in web.config) but unless it's conflicting with something there should be no need to change it.

## Project References ##

1. [Facebook C# SDK](https://github.com/facebook-csharp-sdk/facebook-csharp-sdk)
2. [Dapper](https://github.com/SamSaffron/dapper-dot-net)
3. [Castle Windsor](https://github.com/castleproject/Castle.Windsor-READONLY)