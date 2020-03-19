using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public List<TodoItem> TodoItems { get; set; }
    }
}
