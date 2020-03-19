using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonitoringAPI.Data;
using MonitoringAPI.Models;

namespace MonitoringAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitorController : ControllerBase
    {

        private MyContext _Context;
        private readonly ILogger<MonitorController> _logger;

        public MonitorController(ILogger<MonitorController> logger, MyContext c)
        {
            _logger = logger;
            _Context = c;
        }

        // Users

        [HttpGet("[action]")]
        public async Task<IEnumerable<User>> GetUser()
        {
            var p =  _Context.Users.ToList();
            return p;  
        }

        [HttpGet("[action]")]
        public async Task<User> GetUserByID(int id)
        {
            var p = _Context.Users.Find(id);
            return p;
        }

        [HttpGet("[action]")]
        public async Task<bool> UserLogin(string username, string pass)
        {
            var p = _Context.Users.Where(x => x.Password == pass && x.UserName == username);
            if (p.Any())
                return true;
            else
                return false;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                _Context.Users.Add(user);
                try
                {
                    await _Context.SaveChangesAsync();
                    return StatusCode(200);
                }
                catch (Exception e)
                {
                    return BadRequest("Error in CreateUser " + e);
                }
            }
            else
                return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = _Context.Users.Where(x => x.Id == id);
                if (user.Any())
                {
                    _Context.Users.Remove(user.FirstOrDefault());
                    await _Context.SaveChangesAsync();
                    return StatusCode(200);
                }

                return BadRequest($"User with id:{id} not found");
            }
            catch (Exception e)
            {
                return BadRequest("Error in DeleteUser " + e);
            }
        }


        // ToDoLists
        [HttpGet("[action]")]
        public async Task<IEnumerable<TodoItem>> GetList()
        {
            var p = _Context.TodoItems.ToList();
            return p;
        }
        [HttpGet("[action]")]
        public async Task<TodoItem> GetListByID(int id)
        {
            var p = _Context.TodoItems.Find(id);
            return p;
        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<TodoItem>> GetListByValue(string name, string msg = "", string auth = "")
        {
            if (msg == "" || auth == "")
            {
                return _Context.TodoItems.Where(x => x.Name.StartsWith(name));
            }
            else if (name == "" || auth == "")
            {
                return _Context.TodoItems.Where(x => x.Description.StartsWith(msg));
            }
            else if (msg == "" || auth == "")
            {
                var user = _Context.Users.Where(x => x.UserName == auth).Select(x => x.Id);
                if (user.Any())
                    return _Context.TodoItems.Where(x => x.UserID == user.FirstOrDefault());
                return new List<TodoItem>();
            }
            else
                return _Context.TodoItems.Where(x => x.Name.StartsWith(name));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateList(TodoItem todo)
        {
            if (ModelState.IsValid)
            {
                _Context.TodoItems.Add(todo);
                try
                {
                    await _Context.SaveChangesAsync();
                    return StatusCode(200);
                }
                catch (Exception e)
                {
                    return BadRequest("Error in CreateList " + e);
                }
            }
            else
                return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteList(int id)
        {
            try
            {
                var todo = _Context.TodoItems.Where(x => x.Id == id);
                if (todo.Any())
                {
                    _Context.TodoItems.Remove(todo.FirstOrDefault());
                    await _Context.SaveChangesAsync();
                    return StatusCode(200);
                }

                return BadRequest($"List with id:{id} not found");
            }
            catch (Exception e)
            {
                return BadRequest("Error in DeleteList " + e);
            }
        }
    }
}
