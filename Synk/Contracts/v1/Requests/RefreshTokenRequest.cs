using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synk.Contracts.v1.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
