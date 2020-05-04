using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synk.Contracts.v1
{
    public static class EndpointNames
    {
        public static class Posts
        {
            public const string GetAll = "GetAllPosts";
            public const string Create = "CreatePost";
            public const string Get = "GetSinglePost";
            public const string Update = "UpdateSinglePost";
            public const string Delete = "DeleteSinglePost";
        }

        public static class Identity
        {
            public const string Login = "Login";

            public const string Register = "Register";

            public const string Refresh = "Refresh";
        }
    }
}
