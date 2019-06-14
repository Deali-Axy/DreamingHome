using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DreamingHome.Models;
using DreamingHome.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace DreamingHome.Controllers
{
    /// <summary>
    /// 发布动态
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 依赖注入：获取项目的根目录地址
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public PostController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// 获取所有动态列表
        /// </summary>
        /// <returns>所有动态列表</returns>
        /// <response code="200">返回所有动态列表</response>
        [HttpGet]
        [ProducesResponseType(200)]
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

        /// <summary>
        /// 添加新动态
        /// </summary>
        /// <param name="post">动态内容</param>
        /// <returns>添加结果</returns>
        /// <response code="200">返回添加结果</response>
        [HttpPost("new")]
        [ProducesResponseType(200)]
        public ActionResult<Response> New([FromBody] Post post)
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

        /// <summary>
        /// 上传照片
        /// </summary>
        /// <returns>上传照片结果</returns>
        /// <response code="200">返回上传照片结果</response>
        [HttpPost("add-picture")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Response>> AddPicture()
        {
            var files = Request.Form.Files;
            var size = files.Sum(f => f.Length);
            var webRootPath = _hostingEnvironment.WebRootPath;
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pictures = new List<PostPicture>();
            foreach (var file in files)
            {
                if (file.Length <= 0) continue;

                //文件扩展名，不含“.”
                var fileExt = GetFileExt(file.FileName);
                //获得文件大小，以字节为单位
                var fileSize = file.Length;
                //随机生成新的文件名
                var newFileName = Guid.NewGuid().ToString() + "." + fileExt;
                var filePath = webRootPath + "/upload/" + newFileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                pictures.Add(new PostPicture
                {
                    Id = Guid.NewGuid().ToString(),
                    PictureUrl = filePath,
                });
            }

            return new Response(true, "添加图片", new {Pictures = pictures});
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileName">原始文件名</param>
        /// <returns>扩展名</returns>
        private string GetFileExt(string fileName)
        {
            return fileName.Substring(0, fileName.IndexOf(".", StringComparison.Ordinal));
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