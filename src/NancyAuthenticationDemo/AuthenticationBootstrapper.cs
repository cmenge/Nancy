namespace NancyAuthenticationDemo
{
    using System.Net;
    using Nancy;
    using Nancy.Responses;

    public class AuthenticationBootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoC.TinyIoCContainer container)
        {
            base.InitialiseInternal(container);

            this.Before += (ctx) =>
            {
                // World's-worse-authentication (TM)
                // Pull the username out of the querystring if it exists
                var username = ctx.Request.Query.username;

                if (username.HasValue)
                {
                    ctx.Items["username"] = username.ToString();
                }

                return null;
            };

            this.After += (ctx) =>
            {
                // If status code comes back as Unauthorized then
                // forward the user to the login page
                if (ctx.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ctx.Response = new RedirectResponse("/login?returnUrl=" + ctx.Request.Uri);
                }
            };
        }
    }
}