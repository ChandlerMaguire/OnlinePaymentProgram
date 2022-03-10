using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccountById(int id)
        {
            
            Account account = accountDao.GetAccountByUserId(id);

            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

    }
}
