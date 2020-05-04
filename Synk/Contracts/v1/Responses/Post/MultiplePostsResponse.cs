using Synk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synk.Contracts.v1.Responses
{
    public class MultiplePostsResponse
    {
        public List<Post> posts { get; set; }
    }
}
