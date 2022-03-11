using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao 
    {
        Account GetAccountByUserId(int id);
        Account GetAccountByAccountId(int id);


    }
}
