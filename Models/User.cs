using System;
using System.Collections.Generic;

namespace DreamingHome.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public bool IsDesigner { get; set; }
    }

    public class UserViewModel
    {
        public List<User> Users { get; set; }
    }
}