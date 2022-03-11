using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserDao userDao;
        public UserController(IUserDao userDao)
        {
            this.userDao = userDao;
        }
        [HttpGet]
        public ActionResult<List<User>> ListUsers()
        {
            List<User> users = userDao.GetUsers();

            if (users.Count > 0)
            {
                return Ok(users);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            User user = userDao.GetUserById(id);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("currentuser")]
        public ActionResult<User> GetCurrentUsers()
        {
            string userId = User.FindFirst("sub").Value;
            int id = Int32.Parse(userId);

            User user = userDao.GetCurrentUser(id);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
