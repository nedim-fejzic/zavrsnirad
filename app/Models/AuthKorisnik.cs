using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace app.Models
{
    public class AuthKorisnik : FilterAttribute, IAuthenticationFilter
    {

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["logiran_korisnik"] == null)
                filterContext.HttpContext.Response.Redirect("/Home/Login");
            return;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["logiran_korisnik"] == null)
                filterContext.HttpContext.Response.Redirect("/Home/Login");
            return;
        }



    }
}