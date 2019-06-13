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
        [HttpGet]
        public JsonResult Get()
        {
            var model = new PostViewModel();
            using (var context = new MainContext())
            {
                var sqlData = new SqlPostData(context);
                model.Posts = sqlData.GetAll();
            }

            return new JsonResult(model.Posts);
        }

        [HttpPost("add")]
        public ActionResult<Response> Add([FromBody] Post post)
        {
            if (post == null)
                return new Response {Message = "没提供内容啊！", Success = false};
            using (var context = new MainContext())
            {
                var sqlData = new SqlPostData(context);
                var result = sqlData.Add(post);
                var data = new {Post = post};
                return new Response(true, result ? "发表成功！" : "发表失败！", data);
            }
        }
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

    public class SqlPictureData
    {
        private MainContext _context { get; set; }

        public SqlPictureData(MainContext context)
        {
            _context = context;
        }

        public bool Add(PostPicture picture)
        {
            _context.Add(picture);
            return _context.SaveChanges() > 0;
        }
    }
}