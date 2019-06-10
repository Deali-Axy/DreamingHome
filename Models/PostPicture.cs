using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamingHome.Models
{
    public class PostPicture
    {
        public string Id { get; set; }
        public Post Post { get; set; }
        public string PictureUrl { get; set; }
    }
}