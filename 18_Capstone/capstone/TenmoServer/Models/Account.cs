namespace TenmoServer.Models
{
    public class Account
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public int Account_Id { get; set; }
        
    }

   
    //public class ReturnUser
    //{
    //    public int UserId { get; set; }
    //    public string Username { get; set; }
    //    //public string Role { get; set; }
    //    public string Token { get; set; }
    //}

    ///// <summary>
    ///// Model to accept login parameters
    ///// </summary>
    //public class LoginUser
    //{
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //}
}
