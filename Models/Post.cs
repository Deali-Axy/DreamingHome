using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamingHome.Models
{
    /// <summary>
    /// 发布的post
    /// </summary>
    public class Post
    {
        public string Id { get; set; }
        public User Author { get; set; }
        public string Contetnt { get; set; }
        public string[] PicuresUrl { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Place Place { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
