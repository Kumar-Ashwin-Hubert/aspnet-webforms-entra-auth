# aspnet-webforms-entra-auth

This project demonstrates how to integrate Microsoft Entra ID (formerly Azure AD) authentication into an ASP.NET Web Forms application.

## NuGet Packages Used

The following NuGet packages are required:

- `Microsoft.Owin.Host.SystemWeb` v4.2.2
- `Microsoft.Owin.Security.OpenIdConnect` v4.2.2
- `Microsoft.Owin.Security.Cookies` v4.2.2
- `Microsoft.Identity.Client` v4.71.1

## Key Features

- Microsoft Entra ID authentication for secure user sign-in
- Token caching and management using MSAL.NET
- Cookie handling for SameSite compatibility
- Configuration via `Web.config`

## Key Files and Methods

- [`AspNetWebFormsEntraAuth/Startup.cs`](AspNetWebFormsEntraAuth/AspNetWebFormsEntraAuth/Startup.cs):  
  Configures OWIN middleware and Entra authentication pipeline.

- [`AspNetWebFormsEntraAuth/Utils/MsalAppBuilder.cs`](AspNetWebFormsEntraAuth/AspNetWebFormsEntraAuth/Utils/MsalAppBuilder.cs):  
  Contains the `BuildConfidentialClientApplication()` method to initialize MSAL client for acquiring tokens.

- [`AspNetWebFormsEntraAuth/Utils/MSALPerUserMemoryTokenCache.cs`](AspNetWebFormsEntraAuth/AspNetWebFormsEntraAuth/Utils/MSALPerUserMemoryTokenCache.cs):  
  Implements per-user in-memory token caching.

- [`AspNetWebFormsEntraAuth/Utils/SameSiteCookieManager.cs`](AspNetWebFormsEntraAuth/AspNetWebFormsEntraAuth/Utils/SameSiteCookieManager.cs):  
  Handles SameSite cookie compatibility for authentication cookies.

- [`AspNetWebFormsEntraAuth/Web.config`](AspNetWebFormsEntraAuth/AspNetWebFormsEntraAuth/Web.config):  
  Stores Entra configuration values (ClientId, TenantId, etc.).

## Key Methods

- `Startup.ConfigureAuth(IAppBuilder app)`  
  Sets up OWIN authentication middleware.

- `MsalAppBuilder.BuildConfidentialClientApplication()`  
  Builds and configures the MSAL confidential client.

- `MSALPerUserMemoryTokenCache.Load()` / `Save()`  
  Loads and saves tokens for the current user session.

- `SameSiteCookieManager.SetCookie()`  
  Ensures cookies are set with the correct SameSite attribute.

## Key Code Snippets

**Set NameClaimType to "preferred_username" in OWIN Startup:**
```csharp
// In Startup.Auth.cs or Startup.cs
TokenValidationParameters = new TokenValidationParameters
{
    // This ensures that User.Identity.Name will return the user's email address (preferred_username)
    NameClaimType = "preferred_username"
};
```

> **Why?**
> <br/>By setting `NameClaimType = "preferred_username"`, the application will use the user's email address from Entra ID as the value for `User.Identity.Name`.

**Get the signed-in user's name (which will be the email address from Entra ID in this case):**
```csharp
// Returns the value of the "preferred_username" claim, typically the user's email address
System.Web.HttpContext.Current.User.Identity.Name
```

**Require authentication on any page (redirect if not authenticated):**
```csharp
if (!Request.IsAuthenticated)
{
    HttpContext.Current.GetOwinContext().Authentication.Challenge(
        new AuthenticationProperties
        {
            RedirectUri = "/"
        },
        OpenIdConnectAuthenticationDefaults.AuthenticationType);
}
```

> **How to use:**
> <br/>Add this code to the `Page_Load` event of any page you want to protect. If the user is not authenticated, they will be redirected to sign in.

**Logout and clear user token cache:**
```csharp
string logoutUrl = Globals.PostLogoutRedirectUri;

// Clear the user token cache
await MsalAppBuilder.ClearUserTokenCache();

HttpContext.Current.GetOwinContext().Authentication.SignOut(
    new AuthenticationProperties
    {
        RedirectUri = logoutUrl
    },
    OpenIdConnectAuthenticationDefaults.AuthenticationType,
    CookieAuthenticationDefaults.AuthenticationType);
```

> **How to use:**
> <br/>Call this code when you want to log the user out and clear their token cache.

## How to Run

1. Update `Web.config` with your Entra (Azure AD) app registration values.
2. Build and run the solution.
3. Navigate to the app and sign in using your Entra credentials.

## References

- [Microsoft Identity Platform Documentation](https://learn.microsoft.com/azure/active-directory/develop/)
- [MSAL.NET Documentation](https://learn.microsoft.com/azure/active-directory/develop/msal-overview)

---

For more details, see the code in the referenced files above.