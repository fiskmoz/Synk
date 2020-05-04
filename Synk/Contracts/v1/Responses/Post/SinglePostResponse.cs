using Synk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synk.Contracts.v1.Responses
{
    public class SinglePostResponse
    { 
        public Post Post { get; set; }
        public string locationUri { get; set; }
    }
}
