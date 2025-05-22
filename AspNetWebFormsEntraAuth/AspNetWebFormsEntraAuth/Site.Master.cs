using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Owin.Security.Cookies;
using AspNetWebFormsEntraAuth.Utils;
using System.Threading.Tasks;

namespace AspNetWebFormsEntraAuth
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            string logoutUrl = Globals.PostLogoutRedirectUri;

            // Clear the user token cache
            MsalAppBuilder.ClearUserTokenCache().GetAwaiter().GetResult();

            HttpContext.Current.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = logoutUrl
                },
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = "/"
                    },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }
    }
}