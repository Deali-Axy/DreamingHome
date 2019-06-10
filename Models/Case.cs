using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamingHome.Models
{
    /// <summary>
    /// 整屋装修案例
    /// </summary>
    public class Case
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Designer { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public string HouseType { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 花费
        /// </summary>
        public string Cost { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }
        public DateTime Time { get; set; }
    }
}
