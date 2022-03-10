namespace TenmoClient.Models
{
    public class ApiTransfer
    {
        public int TransferId { get; set; }
        public int TransferTypeId { get; set; }
        public int TransferStatusId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

        public ApiTransfer(int accountTo, int accountFrom, decimal amount,int transferTypeId, int transferStatusId)
        {
            AccountFrom = accountFrom;
            AccountTo = accountTo;
            Amount = amount;
            TransferStatusId = transferStatusId;
            TransferTypeId = transferTypeId;
        }
        public ApiTransfer()
        {

        }
    }
}