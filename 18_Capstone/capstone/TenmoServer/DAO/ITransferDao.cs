using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetTransferById(int id);
        bool SendBucks(Transfer transfer);
    }
}
