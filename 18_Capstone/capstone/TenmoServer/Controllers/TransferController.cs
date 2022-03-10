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
        public TransferController( ITransferDao transferDao)
        {
            this.transferDao = transferDao;
           
        }
      
        public void ListUsers()
        {
            List<User> users = userDao.GetUsers();
            foreach (User user in users)
            {
                Console.WriteLine($"{user.UserId} {user.Username}");
            }
          
        }
        
       [HttpPost()]
       public ActionResult<bool> SendTransfer(Transfer transfer)
        {

            bool result = transferDao.SendBucks(transfer);
           
            if(result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
