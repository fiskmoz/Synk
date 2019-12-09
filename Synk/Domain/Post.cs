using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Synk.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime PublishDate { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public string Likes { get; set; }
    }
}
