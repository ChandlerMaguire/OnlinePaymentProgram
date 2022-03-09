using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace PetInfoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountDao accountDao;

        public AccountController(IAccountDao accountDao)
        {
            this.accountDao = accountDao;
        }
        [HttpGet]
        public ActionResult<Account> GetAccount()
        {
            string userId = User.FindFirst("sub").Value;
            int id = Int32.Parse(userId);
            Account a = accountDao.GetAccountByUserId(id);

            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
