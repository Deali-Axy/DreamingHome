using System;
using System.Collections.Generic;
using System.Linq;
using DreamingHome.Models;
using DreamingHome.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DreamingHome.Controllers
{
    /// <summary>
    /// 整屋案例
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : Controller
    {
        /// <summary>
        /// 获取所有整屋案例文章
        /// </summary>
        /// <returns>整屋案例文章列表</returns>
        /// <response code="200">返回整屋案例文章列表</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<Response> Get()
        {
            var model = new CaseViewModel();
            using (var context = new MainContext())
            {
                var sqlData = new SqlCaseData(context);
                model.Cases = sqlData.GetAll();
            }


            return new Response(true, "获取所有整屋案例", new {Cases = model.Cases});
        }

        /// <summary>
        /// 获取指定id的整屋案例
        /// </summary>
        /// <param name="id">整屋案例的id</param>
        /// <returns>整屋案例文章</returns>
        /// <response code="200">返回整屋案例文章</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public ActionResult<Response> Get(string id)
        {
            using (var context = new MainContext())
            {
                var sqlData = new SqlCaseData(context);
                return new Response(true, "获取整屋案例", new {Case = sqlData.Get(id)});
            }
        }

        /// <summary>
        /// 添加新的整屋案例
        /// </summary>
        /// <param name="cCase">整屋案例对象</param>
        /// <returns>添加结果</returns>
        /// <response code="200">返回添加结果</response>
        [HttpPost("new")]
        [ProducesResponseType(200)]
        public ActionResult<Response> New([FromBody] Case cCase)
        {
            if (cCase == null)
                return new Response {Message = "没提供内容啊！", Success = false};
            using (var context = new MainContext())
            {
                var sqlData = new SqlCaseData(context);
                var result = sqlData.Add(cCase);
                var data = new {Case = cCase};
                return new Response(true, result ? "发表成功！" : "发表失败！", data);
            }
        }
    }

    public class SqlCaseData
    {
        private MainContext _context { get; set; }

        public SqlCaseData(MainContext context)
        {
            _context = context;
        }

        public bool Add(Case cCase)
        {
            _context.Add(cCase);
            return _context.SaveChanges() > 0;
        }

        public Case Get(string id)
        {
            return _context.Cases.FirstOrDefault(c => c.Id == id);
        }

        public List<Case> GetAll()
        {
            return _context.Cases.ToList<Case>();
        }
    }
}