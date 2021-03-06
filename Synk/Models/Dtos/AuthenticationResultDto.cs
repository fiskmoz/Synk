﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synk.Domain
{
    public class AuthenticationResultDto
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
        public string RefreshToken { get; internal set; }
    }
}
