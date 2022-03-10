using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetTransferById(int id);
        List<Transfer> GetTransfers();
        bool SendBucks(Transfer transfer);
    }
}
