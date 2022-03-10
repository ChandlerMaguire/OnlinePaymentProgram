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
        public TransferController(ITransferDao transferDao, IAccountDao accountDao, IUserDao userDao)
        {
            this.transferDao = transferDao;
            this.accountDao = accountDao;
            this.userDao = userDao;
        }
        [HttpGet]
        public ActionResult<List<Transfer>> GetTransfers()
        {
            List<Transfer> transfers = transferDao.GetTransfers();

            if (transfers.Count > 0)
            {
                return Ok(transfers);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Transfer> GetTransferById(int id)
        {
            Transfer transfer = transferDao.GetTransferById(id);

            if (transfer != null)
            {
                return Ok(transfer);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost()]
        public ActionResult<bool> SendTransfer(Transfer transfer)
        {
            string userId = User.FindFirst("sub").Value;
            int id = Int32.Parse(userId);

            Account account = accountDao.GetAccountByUserId(id);
            try
            {

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
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;

        }
    }
}
