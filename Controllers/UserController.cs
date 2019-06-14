using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamingHome.Models;
using DreamingHome.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DreamingHome.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns>用户列表</returns>
        /// <response code="200">返回所有用户列表</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<Response> Get()
        {
            var model = new UserViewModel();
            using (var context = new MainContext())
            {
                var sqlData = new SqlUserData(context);
                model.Users = sqlData.GetAll();
            }

            return new Response(true, "获取所有用户信息", new {Users = model.Users});
        }

        /// <summary>
        /// 测试接口，不用管这个
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        public JsonResult Test()
        {
            return new JsonResult("测试一下");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginUser">用户信息</param>
        /// <returns>登录结果以及Session</returns>
        /// <response code="200">返回登录结果以及Session</response>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        public ActionResult<Response> Login([FromBody] User loginUser)
        {
            using (var context = new MainContext())
            {
                var sqlData = new SqlUserData(context);
                var user = sqlData.GetByPhone(loginUser.Phone);
                var response = new Response();
                if (user.Password == loginUser.Password)
                {
                    var sessionData = new SqlSessionData(context);
                    var session = new Session {User = user};
                    sessionData.Add(session);
                    response.Success = true;
                    response.Message = "登录成功！";
                    response.Data = new {Session = session, User = user};
                }
                else
                {
                    response.Success = false;
                    response.Message = "登录失败，手机号或者密码错误";
                }

                return response;
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>注册结果以及Session</returns>
        /// <response code="200">返回注册结果以及Session</response>
        [HttpPost("signup")]
        [ProducesResponseType(200)]
        public ActionResult<Response> SignUp([FromBody] User user)
        {
            if (user == null)
                return new Response {Message = "没有提供数据啊！", Success = false};
            using (var context = new MainContext())
            {
                var sqlData = new SqlUserData(context);
                var result = sqlData.Add(user);
                var sessionData = new SqlSessionData(context);
                var session = new Session {User = user};
                sessionData.Add(session);
                var data = new {User = user, Session = session};
                return new Response(true, result ? "注册成功" : "注册失败", data);
            }
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>注销结果</returns>
        /// <response code="200">返回注销结果</response>
        [HttpGet("logout")]
        [ProducesResponseType(200)]
        public ActionResult<Response> Logout(string id)
        {
            using (var context = new MainContext())
            {
                var sessionData = new SqlSessionData(context);
                var result = sessionData.DeleteByUserId(id);
                return new Response(result, result ? "注销成功" : "注销失败");
            }
        }
    }

    public class SqlSessionData
    {
        private MainContext _context { get; set; }

        public SqlSessionData(MainContext context)
        {
            _context = context;
        }

        public bool Add(Session session)
        {
            _context.Add(session);
            return _context.SaveChanges() > 0;
        }

        public Session Get(string id)
        {
            return _context.Sessions.FirstOrDefault(s => s.Id == id);
        }

        public Session GetByUser(User user)
        {
            return _context.Sessions.FirstOrDefault(s => s.User == user);
        }

        public bool Delete(string id)
        {
            try
            {
                var sessionModel = _context.Sessions.Find(id);
                _context.Sessions.Remove(sessionModel);
                var flag = _context.SaveChanges();
                return flag > 0;
            }
            catch (Exception)
            {
                Console.WriteLine("删除失败");
                return false;
            }
        }

        public bool DeleteByUserId(string id)
        {
            try
            {
                var sessionModel = _context.Sessions.FirstOrDefault(s => s.User.Id == id);
                _context.Sessions.Remove(sessionModel);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class SqlUserData
    {
        private MainContext _context { get; set; }

        public SqlUserData(MainContext context)
        {
            _context = context;
        }

        public bool Add(User user)
        {
            _context.Add(user);
            return _context.SaveChanges() > 0;
        }

        public User Get(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList<User>();
        }

        public User GetByPhone(string phone)
        {
            return _context.Users.FirstOrDefault(u => u.Phone == phone);
        }

        public bool Delete(string id)
        {
            try
            {
                var userModel = _context.Users.Find(id);
                _context.Users.Remove(userModel);
                var flag = _context.SaveChanges();
                return flag > 0;
            }
            catch (Exception)
            {
                Console.WriteLine("删除失败");
                return false;
            }
        }
    }
}