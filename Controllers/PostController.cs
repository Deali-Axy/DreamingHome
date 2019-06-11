using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamingHome.Models;
using DreamingHome.Utils;
using Microsoft.AspNetCore.Mvc;


namespace DreamingHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
    }

    public class SqlPostData
    {
        private MainContext _context { get; set; }

        public SqlPostData(MainContext context)
        {
            _context = context;
        }

        public bool Add(Post post)
        {
            _context.Add(post);
            return _context.SaveChanges() > 0;
        }

        public Post Get(string id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == id);
        }

        public List<Post> GetAll()
        {
            return _context.Posts.ToList<Post>();
        }
    }
}