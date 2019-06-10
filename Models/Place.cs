using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamingHome.Models
{
    /// <summary>
    /// 位置，比如：客厅、卧室、厨房、洗手间
    /// </summary>
    public class Place
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
