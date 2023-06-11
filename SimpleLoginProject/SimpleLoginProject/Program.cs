// Samuel Batey 16/4/22 Gelos Login Project
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SimpleLoginProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declaration of variables
            string userIn;
            bool running = true;

            // accounts is a list containing each username and corresponding password in a list
            // i.e. [["user1", "pass1"], ["user2", "pass2"], ["user3", "pass3"]]
            List<List<string>> accounts;

            // Main loop
            while (running) {

                // Update the accounts list with the current contents of the file
                accounts = UpdateAcc("accounts.txt");

                // If UpdateAcc() could not find the accounts file, break out of the main loop
                if (accounts == null) {
                    // Error message is in UpdateAcc()
                    // Using break instead of running = false; in order to prevent the user from seeing and using the menu options
                    break;
                }

                // Display menu options
                Console.WriteLine("Select an option by entering the corresponding number: ");
                Console.WriteLine("1 -- Login");
                Console.WriteLine("2 -- Register");
                Console.WriteLine("3 -- View Accounts");
                Console.WriteLine("4 -- Exit");
                userIn = Console.ReadLine();

                // Run corresponding code
                if (userIn == "1") {
                    Login(accounts);
                }
                else if (userIn == "2") {
                    Register("accounts.txt");
                }
                else if (userIn == "3") {
                    ViewAcc(accounts);
                }
                else if (userIn == "4") {
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    running = false;
                }
                else {
                    Console.WriteLine("Invalid option");
                }
            }
        }

        static void Login(List<List<string>> accounts) {
            // Declare variables
            string usernameIn;
            string passwordIn;
            bool loginSuccess = false;

            // Ask user for username and password
            Console.WriteLine("Enter username:");
            usernameIn = Console.ReadLine();
            Console.WriteLine("Enter password:");
            passwordIn = Console.ReadLine();

            // Loop through all account lists
            foreach (List<string> account in accounts) {
                // Check if the username and password match
                if (account[0] == usernameIn && account[1] == passwordIn) {
                    Console.WriteLine("Successfully logged on");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    loginSuccess = true;
	            }
	        }

            // If the match didn't happen, then notify the user they didn't login
            if (loginSuccess == false){
                Console.WriteLine("Incorrect username or password");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }

        static bool ValidatePass(string pass) {
            // Create flags
            bool hasSymbol = false;
            bool hasLetter = false;
            bool hasNumber = false;
            bool correctLength = false;

            // Valiables containing valid symbos, letters, and numbers
            List<string> symbols = new List<string>() { "!", "?", "@", "#", "$", "%", "&", "*", "(", ")", "=", "_", "-" };
            List<string> letters = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            List<string> numbers = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            // Check password length
            if (pass.Length >= 8 && pass.Length <= 14) {
                correctLength = true;
            }

            // Check password has a symbol
            foreach (string symbol in symbols) {
                if (pass.Contains(symbol)) {
                    hasSymbol = true;
                }
            }

            // Check password has a letter
            foreach (string letter in letters) {
                if (pass.Contains(letter) || pass.Contains(letter.ToUpper())) {
                    hasLetter = true;
                }
            }

            // Check password has a number
            foreach (string number in numbers) {
                if (pass.Contains(number)) {
                    hasNumber = true;
                }
            }

            // Check if all tests passed
            if (hasSymbol && hasLetter && hasNumber && correctLength) {
                return true;
            }
            else {
                return false;
            }
        }

        static void Register(string fileName) {
            // Set up Streamwriter
            StreamWriter accountsFile;
            accountsFile = File.AppendText(fileName);

            // Declare variables holding user inputs
            string usrSlct;
            string usrName = "";
            string usrPass = "";

            bool validOption = false;
            while (!validOption) {

                // Menu of options for which password to use
                Console.WriteLine("Which password would you like?");
                Console.WriteLine("1 -- User Entered");
                Console.WriteLine("2 -- Randomly Generated");
                usrSlct = Console.ReadLine();

                // If user entered password
                if (usrSlct == "1") {
                    validOption = true;

                    // Inform user of their selection
                    Console.WriteLine("Follow the steps to register with your own password...");

                    // Ask for their username
                    Console.WriteLine("Please enter a username:");
                    usrName = Console.ReadLine();

                    // Loop until the user has enetered a valid password
                    bool validPass = false;
                    while (!validPass) {
                        // Get password from user and validate it
                        Console.WriteLine("Enter a password:");
                        usrPass = Console.ReadLine();
                        validPass = ValidatePass(usrPass);

                        // Display message if password was invalid
                        if (!validPass) {
                            Console.WriteLine("Invalid Password. Must be between 8 and 14 characters and must include 1 letter, 1 symbol and 1 number.");
                        }
                    }
                    
                }
                // If randomly generated password
                else if (usrSlct == "2") {
                    validOption = true;

                    // Inform user of their selection
                    Console.WriteLine("Follow the steps to register with a generated password...");

                    // Ask for their username
                    Console.WriteLine("Please enter a username:");
                    usrName = Console.ReadLine();

                    // All valid characters to randomly pick from
                    string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!?@#$%&*()=_-";
                    
                    // Declare variables
                    Random r = new Random();
                    char[] pass = new char[8];

                    // Keep generating passwords until one is valid
                    // This probably isn't the best method because theoretically it could just keep generating passwords infinitely, but practically it'll work fine for this
                    bool validPass = false;
                    while (!validPass) {

                        // For each char in the pass array, ramdonly pick a character from the validChars string
                        for (int i = 0; i < pass.Length; i++) {
                            pass[i] = validChars[r.Next(0, validChars.Length)];
                        }

                        // Validate generated password
                        usrPass = new string(pass);
                        validPass = ValidatePass(usrPass);

                        // Used for testing to gauge how many times the password was re-generated because it was invalid
                        //if (!validPass) {
                        //    Console.WriteLine("Generated password " + usrPass + " is invalid, generating new one");
                        //}
                    }
                    
                    // Tell the user what their password is
                    Console.WriteLine("Your generated password is " + usrPass);
                }
                else {
                    Console.WriteLine("Invalid option");
                }
            }

            // Write the new user's username and password to the accounts file
            accountsFile.WriteLine(usrName + " " + usrPass);
            accountsFile.Close();
            Console.WriteLine("Successfully Registered");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static void ViewAcc(List<List<string>> accounts) {
            // Loop through all the account lists
            foreach (List<string> account in accounts) {
                // Display the username and password
                Console.WriteLine(account[0] + "  " + account[1]);
	        }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static List<List<string>> UpdateAcc(string fileName) {
            // Declare variables
            List<string> fileLines;
            List<List<string>> accounts = new List<List<string>>();

            // Make sure the accounts file exists
            if (File.Exists(fileName)) {
                // Read the file into fileLines, then split each line into the username password list and add it to the accounts list
                fileLines = new List<string>(File.ReadAllLines(fileName));
                foreach (string line in fileLines) {
                    accounts.Add(line.Split(' ').ToList());
                }

                // Used for testing
                // Writes out the contents of the accounts list
                //foreach (List<string> group in accounts) {
                //    foreach (string item in group) {
                //        Console.WriteLine(item);
                //    }
                //}
            }
            else {
                // If the file wasn't found, set accounts to null which will trigger an if statement in the main loop, exiting out of the main loop
                Console.WriteLine("Error: Accounts file could not be found");
                accounts = null;
                Console.ReadKey();
            }

            return accounts;
        }
    }
}
