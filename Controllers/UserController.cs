using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamingHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DreamingHome.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        [HttpGet]
        public JsonResult Get()
        {
            var model = new UserViewModel();
            using (var context = new MainContext()) {
                SQLUserData sqlData = new SQLUserData(context);
                model.Users = sqlData.GetAll();
            }

            return new JsonResult(model.Users);
        }

        [HttpPost]
        public JsonResult Post()
        {
            return new JsonResult("测试一下");
        }

        [HttpPost]
        public ActionResult<string> Login([FromBody] User user)
        {
            using (var context = new MainContext()) {
                var sqlData = new SQLUserData(context);
                var result = sqlData.Add(user);
            }

            return "login";
        }

        [HttpPost]
        public object SignUp([FromBody] User user)
        {
            using (var context = new MainContext()) {
                var sqlData = new SQLUserData(context);
                var result = sqlData.Add(user);
            }

            return new {
                Message = result ? "注册成功" : "注册失败",
                User = user,
            };
        }
    }

    public class SQLUserData {
        private MainContext _context { get; set; }

        public SQLUserData(MainContext context)
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

        public bool Delete(string id)
        {
            try {
                var userModel = _context.Users.Find(id);
                _context.Users.Remove(userModel);
                var flag = _context.SaveChanges();
                return flag > 0;
            }
            catch (Exception) {
                Console.WriteLine("删除失败");
                return false;
            }
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList<User>();
        }
    }
}