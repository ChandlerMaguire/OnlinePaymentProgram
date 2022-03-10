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
    public class TransferController : ControllerBase
    {
        private ITransferDao transferDao;
        private IUserDao userDao;
        private IAccountDao accountDao;
        public TransferController(ITransferDao transferDao, IAccountDao accountDao)
        {
            this.transferDao = transferDao;
            this.accountDao = accountDao;
        }
        [HttpGet]
        public void ListUsers()
        {
            List<User> users = userDao.GetUsers();
            foreach (User user in users)
            {
                Console.WriteLine($"{user.UserId} {user.Username}");
            }
        }
        [HttpPost()]
        [AllowAnonymous]
        public ActionResult<bool> SendTransfer(Transfer transfer)
        {
            string userId = User.FindFirst("sub").Value;
            int id = Int32.Parse(userId);

            Account account = accountDao.GetAccountByUserId(id);
            bool result = transferDao.SendBucks(transfer);
            if (result)
            {

                if (transfer.AccountFrom == id)
                {
                    return BadRequest("Both accounts can't be the same.");
                }
                else if (transfer.Amount <= 0)
                {
                    return BadRequest("Amount must be greater than 0.");
                }
                else if (transfer.Amount > account.Balance)
                {
                    return BadRequest("Insufficient funds.");
                }
            }
            return result = true;
        }
    }
}
