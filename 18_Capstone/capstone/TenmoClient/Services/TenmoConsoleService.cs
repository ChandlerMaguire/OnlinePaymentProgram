using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {
        
        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }
       

        // Add application-specific UI methods here...
        public int PromptForTransferAccountTo()
        {
            Console.WriteLine("Please input user ID you would like to transfer to");
            string userInput = Console.ReadLine();
            int X;
            while (!Int32.TryParse(userInput, out X))
            {
                Console.WriteLine("Not a valid input, try again.");

                userInput = Console.ReadLine();
            }
            int accountTo = Int32.Parse(userInput);
            accountTo += 1000;
            return accountTo;
        }
      
        public decimal PromptForTransferAmount()
        {
            Console.WriteLine("Please input amount to transfer(0 to cancel)");
            string userInput = Console.ReadLine();
            int X;
            while (!Int32.TryParse(userInput, out X))
            {
                Console.WriteLine("Not a valid input, try again.");

                userInput = Console.ReadLine();
            }
            decimal amount = Decimal.Parse(userInput);
            return amount;
        }
       
    }
}
