using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamingHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace DreamingHome.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public JsonResult Get()
        {
            var model = new UserViewModel();
            using (var context = new UserContext())
            {
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

        [HttpGet("login")]
        public ActionResult<string> Login()
        {
            return "login";
        }

        public ActionResult<string> SignIn()
        {
            return "";
        }
    }

    public class SQLUserData
    {
        private UserContext _context { get; set; }

        public SQLUserData(UserContext context)
        {
            _context = context;
        }
        public void Add(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }
        public User Get(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public List<User> GetAll()
        {
            return _context.Users.ToList<User>();
        }
    }
}