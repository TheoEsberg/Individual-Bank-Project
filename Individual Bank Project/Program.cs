using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Transactions;

namespace Individual_Bank_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Init ref values
            int currentAccountIndex = 0;
            string[][] users = new string[5][];

            // Load all the users to ref users 
            LoadUsers(ref users);

            // Start the program
            StartProgram(ref currentAccountIndex, ref users);

        }

        private static void StartProgram(ref int currentAccountIndex, ref string[][] users)
        {
            if (Login(ref currentAccountIndex, ref users))
            {
                Menu(ref currentAccountIndex, ref users);
            }
        }

        private static void WithdrawMoney(ref int currentAccountIndex, ref string[][] users)
        {
            Console.Clear();
            Console.WriteLine("Dina konton.\n");

            // For loop that shows all of the active users accounts
            for (int i = 2; i < users[currentAccountIndex].Length; i += 2)
            {
                Console.WriteLine((i / 2) + ". " + users[currentAccountIndex][i] + ": " + users[currentAccountIndex][i + 1] + "kr");
            }

            // Ask which account to withdraw from
            Console.WriteLine("\nVilket konto vill du ta ut ifrån?");
            int withdrawFrom = Int32.Parse(Console.ReadLine()) * 2 + 1;

            // Ask what amount to withdraw
            Console.WriteLine("\nHur mycket vill du ta ut? (använd , för ören!)");
            double withdrawAmount = Convert.ToDouble(Console.ReadLine());

            // Check if there is enough balance to transfer from that account
            if (Convert.ToDouble(users[currentAccountIndex][withdrawFrom]) >= withdrawAmount)
            {
                // Create a temporary double to act as an middleman in the withdrawal since 
                // you cant remove a double amount from a string
                double temp = Convert.ToDouble(users[currentAccountIndex][withdrawFrom]);
                temp -= withdrawAmount;
                users[currentAccountIndex][withdrawFrom] = Convert.ToString(temp);

                // Tells the user that the transfer was successful
                Console.WriteLine("\nUttaget lyckades!\nTryck enter för att återgå till menyn.");
            } else
            {
                Console.WriteLine("\nUttaget misslyckades! Du har inte tillräckligt med pengar.\nTryck enter för att återgå till menyn.");
            }

            // Go back to the menu
            Console.ReadKey();
            Menu(ref currentAccountIndex, ref users);
        }

        private static void TransferMoney(ref int currentAccountIndex, ref string[][] users)
        {
            Console.Clear();
            Console.WriteLine("Dina konton.\n");

            // For loop that shows all of the active users accounts
            for (int i = 2; i < users[currentAccountIndex].Length; i += 2)
            {
                Console.WriteLine((i/2) + ". " + users[currentAccountIndex][i] + ": " + users[currentAccountIndex][i + 1] + "kr");
            }

            // Ask which account to transfer from
            Console.WriteLine("\nVilket konto vill du överför ifrån?");
            int transferFrom = Int32.Parse(Console.ReadLine()) * 2 + 1;

            // Ask which account to transfer to
            Console.WriteLine("\nVilket konto vill du överför till?");
            int transferTo = Int32.Parse(Console.ReadLine()) * 2 + 1;

            // Ask what amount to transfer
            Console.WriteLine("\nHur mycket vill du överföra? (använd , för ören!)");
            double transferAmount = Convert.ToDouble(Console.ReadLine());

            // Check if there is enough balance to transfer from that account
            if (Convert.ToDouble(users[currentAccountIndex][transferFrom]) >= transferAmount)
            {
                // Create a temporary double to act as an middleman in the transfer since 
                // you cant remove a double amount from a string
                double temp = Convert.ToDouble(users[currentAccountIndex][transferFrom]);
                temp -= transferAmount;
                users[currentAccountIndex][transferFrom] = Convert.ToString(temp);

                // Create a temporary double to act as an middleman in the transfer since 
                // you cant remove a double amount from a string
                temp = Convert.ToDouble(users[currentAccountIndex][transferTo]);
                temp += transferAmount;
                users[currentAccountIndex][transferTo] = Convert.ToString(temp);

                // Tells the user that the transfer was successful
                Console.WriteLine("\nÖverföringen lyckades!\nTryck enter för att återgå till menyn.");
            }

            // Go back to the menu
            Console.ReadKey();
            Menu(ref currentAccountIndex, ref users);
        }

        private static void CheckAccountBalance(ref int currentAccountIndex, ref string[][] users)
        {
            Console.Clear();
            Console.WriteLine("Här är dina konton.\n");

            for (int i = 2; i < users[currentAccountIndex].Length; i+=2) {
                Console.WriteLine(users[currentAccountIndex][i] + ": " + users[currentAccountIndex][i+1] + "kr");
            }

            Console.WriteLine("\nTryck enter för att återgå till menyn.");

            Console.ReadKey();
            Menu(ref currentAccountIndex, ref users);

        }

        private static void Menu(ref int currentAccountIndex, ref string[][] users)
        {
            Console.Clear();
            Console.WriteLine("1. Se över dina konton och saldon");
            Console.WriteLine("2. Överföring mellan konton");
            Console.WriteLine("3. Ta ut pengar");
            Console.WriteLine("4. Logga ut");

            string choise = Console.ReadLine();

            switch (choise)
            {
                case "1":
                    CheckAccountBalance(ref currentAccountIndex, ref users);
                    break;
                case "2":
                    TransferMoney(ref currentAccountIndex, ref users);
                    break;
                case "3":
                    WithdrawMoney(ref currentAccountIndex, ref users);
                    break;
                case "4":
                    StartProgram(ref currentAccountIndex, ref users);
                    break;
                default:
                    Console.WriteLine("\nOgiltigt val.");
                    Thread.Sleep(1000);
                    Menu(ref currentAccountIndex, ref users);
                    break;
            }
        }

        private static bool Login(ref int currentAccountIndex, ref string[][] users)
        {
            // Declare attempts
            int attempts = 0;

            // Wellcome message
            Console.WriteLine("Wellcome to Esbergs Bank\n");

            // A do while loop
            do
            {
                // Get username and password
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                // Go through each user inside of users
                for (int i = 0; i < users.Length; i++)
                {
                    // Check if username is equal to a username inside of users
                    // Aswell as if the password is equal to that users password
                    if (users[i][0] == username && users[i][1] == password)
                    {
                        // Add one to the currentAccountIndex
                        currentAccountIndex = i;

                        // Return true 
                        return true;
                    }
                }

                // Add one to the attempts
                attempts++;

                Console.WriteLine();

              // Check so that the attempts is not greater than 3
            } while (attempts < 3);

            // Return false
            return false;
        }

        private static string[][] LoadUsers(ref string[][] users)
        {

            // Initialize all of the 5 users
            users[0] = new string[] { "admin", "0000", "konto", "9999" };
            users[1] = new string[] { "theo", "1111", "sparkonto", "1500,0", "lönekonto", "4000,5" };
            users[2] = new string[] { "erik", "2222", "sparkonto", "430,0", "lönekonto", "5302,0" };
            users[3] = new string[] { "kalle", "3333", "sparkonto", "20,5", "lönekonto", "200,25" };
            users[4] = new string[] { "maja", "4444", "sparkonto", "4300,0", "lönekonto", "23050,25" };

            // Return users as "string[][]"
            return users;
        }
    }
}
