using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                // View your current balance
                GetBalance();
            }

            if (menuSelection == 2)
            {
                // View your past transfers
                GetTransfers();
            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
                // Send TE bucks
                SendBucks();
            }

            if (menuSelection == 5)
            {
                // Request TE bucks
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }
        public void GetBalance()
        {
            ApiAccount account = tenmoApiService.GetAccount();
            Console.WriteLine($"Your current abbout balance is: {account.Balance.ToString("C")}");
            console.Pause();
        }
        public void ListUsers()
        {
            List<ApiUser> users = tenmoApiService.GetUsers();
            foreach (ApiUser user in users)
            {
                Console.WriteLine($"{user.UserId} {user.Username}");
            }
        }
        public ApiTransfer GetTransferById(int id)
        {
            ApiTransfer transfer = null;
            try
            {
                transfer = tenmoApiService.GetTransferById(id);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid transfer Id");
            }
            return transfer;
        }
        public void SendBucks()
        {
            try
            {
                List<ApiUser> users = tenmoApiService.GetUsers();
                string id = "Id";
                string username = "Username";
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("|----------Users----------|");
                Console.WriteLine($"| {id.PadLeft(5)} | {username.PadRight(15)} |");
                Console.WriteLine("|-------------------------|");
                foreach (ApiUser user in users)
                {
                    Console.WriteLine($"| {user.UserId.ToString().PadLeft(5)} | {user.Username.ToString().PadRight(15)} |");
                }
                Console.WriteLine("|-------------------------|");
            }
            catch (Exception ex)
            {

                Console.WriteLine();
                Console.WriteLine("Unable to list users: " + ex.Message);
            }
            ApiUser currentUser = tenmoApiService.GetCurrentUser();
            int accountFromId = currentUser.UserId += 1000;

            bool done = false;
            int accountToId = 0;

            while (!done)
            {
                accountToId = console.PromptForTransferAccountTo();
                if (accountToId == 0)
                {
                    return;
                }
                if (accountFromId == accountToId)
                {
                    Console.WriteLine("Account ID cannot be your own.");
                }
                else
                {
                    done = true;
                }
            }

            done = false;
            decimal amount = 0;

            while (!done)
            {
                amount = console.PromptForTransferAmount();
                ApiUser user = tenmoApiService.GetCurrentUser();
                int accountId = user.UserId += 1000;
                ApiAccount account = tenmoApiService.GetAccountById(accountId);

                if (amount == 0)
                {
                    done = true;
                    return;
                }
                else if (amount < 0)
                {
                    Console.WriteLine("Amount cannot be negative.");
                    console.Pause();
                    continue;
                }
                else if (amount > account.Balance)
                {
                    Console.WriteLine("Amount cannot be greater than account balance.");
                    continue;
                }
                done = true;
            }

            int transferStatusId = 2;
            int transferTypeId = 2;

            ApiTransfer transfer = new ApiTransfer(accountToId, accountFromId, amount, transferTypeId, transferStatusId);
            tenmoApiService.AddTransfer(transfer);

            Console.WriteLine("Transfer successful.");
            console.Pause();
        }
        public void GetTransfers()
        {
            List<ApiTransfer> transfers = tenmoApiService.GetTransfers();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Transfers");
            Console.WriteLine($"ID" + " " + "From/To".PadLeft(15) + " " + "Amount".PadLeft(15));
            Console.WriteLine("------------------------------------");

            foreach (ApiTransfer transfer in transfers)
            {
                int userFromId = transfer.AccountFrom -= 1000;
                ApiUser userFrom = tenmoApiService.GetUserById(userFromId);

                int userToId = transfer.AccountTo -= 1000;
                ApiUser userTo = tenmoApiService.GetUserById(userToId);

                string from = "From:";
                string to = "To:";

                Console.WriteLine($"{transfer.TransferId} {from.PadLeft(11)} {userFrom.Username.PadLeft(2)} {transfer.Amount.ToString("C").PadLeft(11)}");
                Console.WriteLine($"{transfer.TransferId} {to.PadLeft(9).PadRight(2)} {userTo.Username.PadLeft(6)} {transfer.Amount.ToString("C").PadLeft(11)}");

            }
            Console.WriteLine("------------------------------------");
            Console.Write("Input ID to see transfer details (0 to cancel): ");


            string userInput = Console.ReadLine();
            int transferId = Int32.Parse(userInput);

            if (transferId != 0)
            {

                ApiTransfer transfer = GetTransferById(transferId);
                if(transfer == null)
                {
                    console.Pause();
                    return;
                }
                int userFromId = transfer.AccountFrom -= 1000;
                ApiUser userFrom = tenmoApiService.GetUserById(userFromId);

                int userToId = transfer.AccountTo -= 1000;
                ApiUser userTo = tenmoApiService.GetUserById(userToId);

                string transferType = null;
                string transferStatus = null;

                if (transfer.TransferTypeId == 2)
                {
                    transferType = "Send";
                }
                else if (transfer.TransferTypeId == 1)
                {
                    transferType = "Request";
                }

                if (transfer.TransferStatusId == 2)
                {
                    transferStatus = "Approved";
                }
                else if (transfer.TransferStatusId == 1)
                {
                    transferStatus = "Pending";
                }
                else if (transfer.TransferStatusId == 3)
                {
                    transferStatus = "Rejected";
                }
                Console.WriteLine();
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Transfer Details");
                Console.WriteLine("------------------------------------");
                Console.WriteLine($"Id: {transfer.TransferId}");
                Console.WriteLine($"From: {userFrom.Username}");
                Console.WriteLine($"To: {userTo.Username}");
                Console.WriteLine($"Type: {transferType}");
                Console.WriteLine($"Status: {transferStatus}");
                Console.WriteLine($"Amount: {transfer.Amount.ToString("C")}");
                Console.WriteLine();
                console.Pause();
            }
            return;
        }
    }
}



