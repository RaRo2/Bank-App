using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace BankingApp
{
    public class Account
    {
        public int accountNo { get; set; }
        public double phone { get; set; }
        public double balance { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string address { get; set; }
        public string email { get; set; }

        public Account(int accountNo, string fName, string lName, string address, double phone, string email, double balance)
        {
            this.accountNo = accountNo;
            this.fName = fName;
            this.lName = lName;
            this.address = address;
            this.phone = phone;
            this.email = email;
            this.balance = balance;

        }

        public void deposit(double amount)
        {
            balance += amount;
        }

        public void withdraw(double amount)
        {
            balance -= amount;
        }

    }

    class AccountDB
    {
        Account[] accounts = new Account[20];
        string accDir = "accounts";
        string logFile = "login.txt";
        string tranFile = "transactions.txt";

        /********************************************************************************************************************
        *  1) Login Menu                                                       
        
        /********************************************************************************************************************
        *               1a. login                                                                                           
        *                   This method displays the initial log in screen of the Banking App.                              
        *                   It takes two values: enterUser and enterPass from readPassword()                                                    
        *                   These values are checked with the admin user and pass                                           
        *                   If there is a match, function returns true, if not false                                        
        *                                                                                                                   
        *               Inputs:                                                                                             
        *                   N/A                                                                                             
        *               Outputs:                                                                                            
        *                   success(bool): If the login was successful or not                                               
        *                                                                                                                   
        ********************************************************************************************************************/
        public bool login()
        {
            bool success = false;
            try
            {
                menuUI("logTop");
                Console.Write("|    Username: ");
                int cursorX = Console.CursorLeft;
                int cursorY = Console.CursorTop;
                menuUI("logWait");
                Console.SetCursorPosition(cursorX, cursorY);
                string enterUser = Console.ReadLine();
                Console.SetCursorPosition(cursorX, cursorY + 1);
                string enterPass = readPassword();


                string[] loginAccounts = System.IO.File.ReadAllLines(logFile);
                foreach (string account in loginAccounts)
                {
                    string[] details = account.Split('|');
                    success = (details[0].Equals(enterUser) && details[1].Equals(enterPass));
                    if (success)
                    {
                        break;
                    }
                }
                cursorX = Console.CursorLeft;
                cursorY = Console.CursorTop;
                Console.SetCursorPosition(cursorX, cursorY + 2);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Login file could not be found.");
                Console.ReadKey();

            }
            return success;
        }

        /********************************************************************************************************************
        *               1b. readPassword                                                                                    
        *                   This method takes inputs from the user and blurs the input.                                     
        *                   This input is saved into a password string which is then                                        
        *                   returned.                                                                                       
        *                                                                                                                   
        *               Inputs:                                                                                             
        *                   N/A                                                                                             
        *               Outputs:                                                                                           
        *                   password(string): the password without the masking                                              
        *                                                                                                                   
        ********************************************************************************************************************/
        public string readPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return password;
        }


        /********************************************************************************************************************
         *  2) Main Menu

        /********************************************************************************************************************
        *              2.  menuUI 
        *                  This method creates all the visual user interfaces within the application.
        *                  It takes a string input from the different functions which call it
        *                  and depending on the input, it displays a different portion of the UI.
        *                  
        *              Inputs:
        *                  choice(string): a string which contains a keyword to print out menu segments
        *              Outputs:
        *                  N/A
        *                  
        *******************************************************************************************************************/
        public void menuUI(string choice)
        {
            switch (choice)
            {
                case "logTop":
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|       Welcome to this Simple Banking System      |");
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|               Please login to start              |");
                    Console.WriteLine("|                                                  |");
                    break;
                case "logWait":
                    Console.WriteLine("                                    |");
                    Console.WriteLine("|    Password:                                     |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|==================================================|");
                    break;

                case "mainTop":
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|      Welcome to this Simple Banking System       |");
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|            1. Create a new account               |");
                    Console.WriteLine("|            2. Search for an account              |");
                    Console.WriteLine("|            3. Deposit                            |");
                    Console.WriteLine("|            4. Withdraw                           |");
                    Console.WriteLine("|            5. A/C Statement                      |");
                    Console.WriteLine("|            6. Delete account                     |");
                    Console.WriteLine("|            7. Exit                               |");
                    Console.WriteLine("|==================================================|");
                    break;

                case "detailStart":
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|              Enter the Account Details           |");
                    Console.WriteLine("|--------------------------------------------------|");
                    Console.WriteLine("|            |                                     |");
                    break;
                case "endLine":
                    Console.WriteLine("|==================================================|");
                    break;

                case "createTop":
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                Create a New Account              |");
                    Console.WriteLine("|==================================================|");
                    break;
                case "createBottom":
                    Console.WriteLine("                                    |");
                    Console.WriteLine("|  Last Name |                                     |");
                    Console.WriteLine("|    Address |                                     |");
                    Console.WriteLine("|      Phone |                                     |");
                    Console.WriteLine("|      Email |                                     |");
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|==================================================|");
                    break;
                case "searchTop":
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                 Search for Account               |");
                    Console.WriteLine("|==================================================|");
                    break;
                case "searchBottom":
                    Console.WriteLine("                                     |");
                    Console.WriteLine("|--------------------------------------------------|");
                    Console.WriteLine("|            |                                     |");
                    Console.WriteLine("|------------|-------------------------------------|");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|==================================================|");
                    break;
                case "deposit":
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                     Deposit                      |");
                    Console.WriteLine("|==================================================|");
                    break;
                case "withdraw":
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                     Withdraw                     |");
                    Console.WriteLine("|==================================================|");
                    break;
                case "balanceBottom":
                    Console.WriteLine("                        |");
                    Console.WriteLine("|        Amount        | $                         |");
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|==================================================|");
                    break;
                case "delete":
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                Delete an Account                 |");
                    Console.WriteLine("|==================================================|");
                    break;
                case "statement":
                    Console.WriteLine("|             Most Recent Transactions             |");
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("| Date and Time         | Type       | Amount      |");
                    Console.WriteLine("|=======================|============|=============|");
                    Console.WriteLine("|                       |            |             |");
                    Console.WriteLine("|                       |            |             |");
                    Console.WriteLine("|                       |            |             |");
                    Console.WriteLine("|                       |            |             |");
                    Console.WriteLine("|                       |            |             |");
                    Console.WriteLine("|==================================================|");
                    Console.WriteLine("|                                                  |");
                    Console.WriteLine("|==================================================|");
                    break;
            }
        }

        /********************************************************************************************************************
         *  3)  Create an Account
           
        /*******************************************************************************************************************
         *              3.  createAccount
         *                  This method first checks how many current account files there are in the directory using
         *                  readFile() to get the index in which to add the account. It then finds the first 
         *                  free account number which can be used. It collects all the data necessary to create
         *                  an account and checks if each field is valid. Once validated, a new account file is
         *                  created and the database is updated using writeAccounts()
         *                  
         *              Inputs:
         *                  index(int): a value which indicates where the account array is currently filled to
         *              Outputs:
         *                  N/A
         *                  
         *******************************************************************************************************************/
        public void createAccount(int index)
        {
        bool confirm = false;
            while (!confirm)
            {
                Console.Clear();
                bool valid = true;
                string failReason = "";
                int accountNo = 100001;

                string[] filePaths = Directory.GetFiles(accDir);
                foreach (string file in filePaths)
                {
                    if (file.Contains(accountNo.ToString()))
                    {
                        accountNo++;
                    }
                }

                menuUI("createTop");

                Console.Write("| First Name | ");
                int cursorX = Console.CursorLeft;
                int cursorY = Console.CursorTop;
                menuUI("createBottom");

                Console.SetCursorPosition(cursorX, cursorY);
                string firstName = Console.ReadLine();
                cursorY++;

                Console.SetCursorPosition(cursorX, cursorY);
                string lastName = Console.ReadLine();
                cursorY++;

                Console.SetCursorPosition(cursorX, cursorY);
                string address = Console.ReadLine();
                cursorY++;

                Console.SetCursorPosition(cursorX, cursorY);
                string phoneInput = Console.ReadLine();
                double phone;
                if (!(double.TryParse(phoneInput, out phone)))
                {
                    valid = false;
                    failReason += " [Phone] ";
                }

                cursorY++;

                Console.SetCursorPosition(cursorX, cursorY);
                string email = Console.ReadLine();
                if (!(email.Contains("@")))
                {
                    valid = false;
                    failReason += " [Email] ";
                }

                Console.SetCursorPosition(0, cursorY + 2);
                if (valid)
                {

                    Console.Write($"|   Is the account information correct (y/n)? ");
                    if (Console.ReadLine() == "y")
                    {
                        accounts[index] = new Account(accountNo, firstName, lastName, address, phone, email, 0);
                        Console.WriteLine($"|             Account {accountNo} Added");
                        writeAccounts(1);
                        readAccounts();
                        Account a = accounts[index];
                        string body = $"Account Name: {a.fName} {a.lName}<br/>AccountNo: {a.accountNo}<br/>Balance: {a.balance}<br/>Phone: {a.phone}<br/>";
                        sendEmail(email, "Account Creation ", body);
                        confirm = true;
                    }
                    else
                    {
                        Console.Clear();
                    }

                }
                else
                {
                    confirm = true;
                    Console.WriteLine($"|            Account could not be made.");
                    Console.WriteLine($"|       Invalid Field(s): {failReason}");
                }
            }
            Console.WriteLine($"|          Press Enter to Return to Menu");
            Console.ReadKey();
        }


        /********************************************************************************************************************
         *  4)  Search for an Account
           
        /********************************************************************************************************************
         *              4a. searchAccount
         *                  This method searches through the accounts array to see if any of the
         *                  account numbers match the account number inputted by the user. It returns an
         *                  account if there is a match, and returns null if there isnt
         *                  
         *              Inputs:
         *                  enterredNo(int): The account number that is searched for
         *              Outputs:
         *                  Account: the account with the matchign account number
         *                  
         *******************************************************************************************************************/
        public Account searchAccount(int enterredNo)
        {
            for (int i = 0; i < readAccounts(); i++)
            {
                if (accounts[i].accountNo.Equals(enterredNo))
                {
                    return accounts[i];
                }
            }
            return null;
        }

        /********************************************************************************************************************
        *              4b. displayAccount
        *                  This method takes an input from the user and first checks if it is a valid account
        *                  number. If it is, it uses searchAccount() to find out if there is a match in the
        *                  database. It then displays all the information regarding the account. 
        *                  
        *              Inputs:
        *                  N/A
        *              Outputs:
        *                  N/A
        *                  
        ********************************************************************************************************************/
        public void displayAccount()
        {
            bool loop = true;
            while (loop)
            {
                writeAccounts(0);
                Console.Clear();
                menuUI("searchTop");
                Console.Write("| Account No: ");
                int cursorX = Console.CursorLeft;
                int cursorY = Console.CursorTop;
                menuUI("searchBottom");
                Console.SetCursorPosition(cursorX, cursorY);
                int enterredNo = 0;

                while (!(int.TryParse(Console.ReadLine(), out enterredNo)))
                {
                    Console.SetCursorPosition(0, cursorY + 5);
                    Console.WriteLine("|             Invalid account number.");
                    Console.WriteLine("|      Please enter a valid account number.");
                    Console.SetCursorPosition(cursorX, cursorY);
                    Console.Write("       ");
                    Console.SetCursorPosition(cursorX, cursorY);
                }
                Account a = searchAccount(enterredNo);
                if (a != null)
                {
                    Console.SetCursorPosition(cursorX, cursorY);
                    string fullName = a.fName + " " + a.lName;
                    Console.Write($"{a.accountNo} | Name:  {fullName.PadLeft(20)}\n\n");
                    Console.WriteLine($"|    Balance | $ {a.balance.ToString().PadLeft(24)}\n");
                    Console.WriteLine($"|    Address | {a.address.PadRight(26)}");
                    Console.WriteLine($"|      Phone | {a.phone.ToString().PadRight(26)}");
                    Console.WriteLine($"|      Email | {a.email.PadRight(26)}");
                    Console.WriteLine("|==================================================|");
                }
                else
                {
                    Console.SetCursorPosition(0, cursorY + 5);
                    Console.WriteLine($"|         There is no account with number: ");
                    Console.WriteLine($"|                      [{enterredNo}]");
                }
                Console.Write($"|          Check another account (y/n)? ");
                if (Console.ReadLine() != "y")
                {
                    loop = false;
                }
            }
        }


        /********************************************************************************************************************
         *  5/6)  Deposit and Withdraw
           
        /********************************************************************************************************************
        *          5/6. changeBalance
        *               This function takes an input character to determine if it will perform a deposit (d)
        *               or withdrawal (w). It then checks if the user has entered a valid accountNumber.
        *               searchAccount() is then used to see if the account exists in the database or not.
        *               If it does, the user is prompted to input a sum in which the processes are done.
        *               Any amount can be deposited but the system will only allow withdrawals if the account's
        *               balance is greater than or equal to the amount.
        *               
        *               Inputs:
        *                   mode(char): a character used to determine whether a deposit or withdrawal is performed
        *               Outputs:
        *                   N/A
        *                   
        ********************************************************************************************************************/
        public void changeBalance(char mode)
        {
            bool loop = true;
            while (loop)
            {
                Console.Clear();
                if (mode == 'd')
                {
                    menuUI("deposit");
                }
                else if (mode == 'w')
                {
                    menuUI("withdraw");
                }

                Console.Write("|      Account No      |   ");
                int cursorX = Console.CursorLeft;
                int cursorY = Console.CursorTop;
                menuUI("balanceBottom");
                Console.SetCursorPosition(cursorX, cursorY);
                int enterredNo = 0;

                while (!(int.TryParse(Console.ReadLine(), out enterredNo)))
                {
                    Console.SetCursorPosition(0, cursorY + 3);
                    Console.WriteLine("|             Invalid account number.");
                    Console.WriteLine("|      Please enter a valid account number.");
                    Console.SetCursorPosition(cursorX, cursorY);
                    Console.Write("       ");
                    Console.SetCursorPosition(cursorX, cursorY);
                }
                Account a = searchAccount(enterredNo);
                if (a != null)
                {
                    cursorY++;
                    Console.SetCursorPosition(cursorX, cursorY);
                    if (!double.TryParse(Console.ReadLine(), out double amount))
                    {
                        Console.SetCursorPosition(0, cursorY + 3);
                        Console.WriteLine($"|           Invalid amount. ");
                        Console.WriteLine("|      Please enter a valid amount.");
                    }
                    else
                    {
                        if (mode == 'd')
                        {
                            Console.SetCursorPosition(0, cursorY + 2);
                            Console.WriteLine($"|          Deposited ${amount}.00 into {a.accountNo} ");
                            string transaction = ($"{a.accountNo}-Deposit|{amount}|{DateTime.Now}\n");
                            System.IO.File.AppendAllText(tranFile, transaction);
                            a.deposit(amount);
                        }
                        if (mode == 'w')
                        { 
                            if (a.balance >= amount)
                            {
                                Console.SetCursorPosition(0, cursorY + 2);
                                Console.WriteLine($"|            Withdrew ${amount}.00 from {a.accountNo}  ");
                                string transaction = ($"{a.accountNo}-Withdraw|{amount}|{DateTime.Now}\n");
                                System.IO.File.AppendAllText(tranFile, transaction);
                                a.withdraw(amount);
                                
                            }
                            else
                            {
                                Console.SetCursorPosition(0, cursorY + 2);
                                Console.WriteLine($"|       Not enough funds to withdraw from {a.accountNo}  ");

                            }
                        }
                    }
                    writeAccounts(0);
                }
                else
                {
                    Console.SetCursorPosition(0, cursorY + 3);
                    Console.WriteLine($"|         There is no account with number: ");
                    Console.WriteLine($"|                      [{enterredNo}]");
                }
                Console.Write($"|            Try another account (y/n)? ");
                if (Console.ReadLine() != "y")
                {
                    loop = false;
                }
            }
        }


        /********************************************************************************************************************
         *  7)  Account Statement
         
        /********************************************************************************************************************
        *         7a.   getStatement
        *               This function first takes an input to check for a valid account number and displays
        *               the account similar to displayAccount(). It then scans through the selected accounts file
        *               and checks for the last 5 transactions to display by reversing the list and splitting it 
        *               into segments.
        *               
        *               Inputs:
        *                   N/A
        *               Outputs:
        *                   N/A
        *                   
        ********************************************************************************************************************/
        public void getStatement()
        {
            writeAccounts(0);
            Console.Clear();
            menuUI("searchTop");
            Console.Write("| Account No: ");
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            menuUI("searchBottom");
            Console.SetCursorPosition(cursorX, cursorY);
            int enterredNo = 0;

            while (!(int.TryParse(Console.ReadLine(), out enterredNo)))
            {
                Console.SetCursorPosition(0, cursorY + 4);
                Console.WriteLine("|             Invalid account number.");
                Console.WriteLine("|      Please enter a valid account number.");
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write("       ");
                Console.SetCursorPosition(cursorX, cursorY);
            }
            Account a = searchAccount(enterredNo);
            string body = $"Account Name: {a.fName} {a.lName}<br/>AccountNo: {a.accountNo}<br/>Balance: {a.balance}<br/>Phone: {a.phone}<br/>";
            if (a != null)
            {
                Console.SetCursorPosition(cursorX, cursorY);
                string fullName = a.fName + " " + a.lName;
                Console.Write($"{a.accountNo} | Name:  {fullName.PadLeft(20)}\n\n");
                Console.WriteLine($"|    Balance | $ {a.balance.ToString().PadLeft(24)}\n");
                Console.WriteLine($"|    Address | {a.address.PadRight(26)}");
                Console.WriteLine($"|      Phone | {a.phone.ToString().PadRight(26)}");
                Console.WriteLine($"|      Email | {a.email.PadRight(26)}");
                Console.WriteLine("|==================================================|");
                cursorX = 0;
                cursorY += 12;
                menuUI("statement");
                Console.SetCursorPosition(cursorX, cursorY);
                string[] lines = System.IO.File.ReadAllLines(accDir + "\\" + a.accountNo.ToString()+".txt");
                    
                Array.Reverse(lines);
                string[] transactions = new string[5];
                if (lines.Length > 5)
                {
                    Array.Copy(lines, transactions, 5);
                }
                else
                {
                    Array.Resize(ref transactions, lines.Length-1);
                    Array.Copy(lines, transactions, lines.Length-1);
                }                    
                foreach (string transaction in transactions)
                {
                    string[] details = transaction.Split('|');
                    Console.WriteLine($"| {details[2]} | {details[0].PadLeft(10)} | ${details[1].PadLeft(10)} |");
                    body += $"Date: {details[2]} | Type: {details[1]} | Amount: {details[0]}<br/>";

                }

                    
            }
            else
            {
                Console.SetCursorPosition(0, cursorY + 5);
                Console.WriteLine($"|         There is no account with number: ");
                Console.WriteLine($"|                      [{enterredNo}]");
            }
            Console.SetCursorPosition(0, cursorY+6);
            Console.Write($"|          Do you want to email the statement (y/n)? ");
            if (Console.ReadLine() == "y")
            {
                sendEmail(a.email, "Account Statement ",body);
                Console.ReadKey();
            }
        }


        /********************************************************************************************************************
        *         7b.   sendEmail
        *               This function takes the mail information from the user and sends an attachment
        *               of either their registration or their statement depending on when sendEmail() was called. 
        *               This function can only be accessed through gmail smtp services.
        *               
        *               Inputs:
        *                   recipient(string): the receiever of the email
        *                   subject/body(string): the mail subject and body
        *                   fileName(string): the attachments file directory
        *               Outputs:
        *                   N/A
        *                   
        ********************************************************************************************************************/
        public void sendEmail(string recipient, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential("littlefighter750@gmail.com", "fqgzuhigvnsbpsdv");

                mail.From = new MailAddress("littlefighter750@gmail.com", "Mail System");
                mail.To.Add(new MailAddress(recipient));
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;
                Console.WriteLine($"\n\n\n\n  Attempting to send email to {recipient}...");

                smtp.Send(mail);
                Console.WriteLine($"   Email sent to {recipient}...");

            }
            
            catch (SmtpException e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n     There was an error sending the Email...");
            }
        }


        /********************************************************************************************************************
         *  8)  Delete an Account
           
        /********************************************************************************************************************
        *           8.  deleteAccount
        *               This function performs the usual procedure of checking for a valid account using 
        *               searchAccount(). If there is a match, the system displays the account information with
        *               a confirmation prompt to ask the user if they do want to delete the account.
        *               If yes, the account file is deleted and the database is updated using readAccounts() for
        *               the array and writeAccounts() to the database.
        *           
        *           Inputs:
        *               N/A
        *           Outputs:
        *               N/A
        ********************************************************************************************************************/
        public void deleteAccount()
        {
            Console.Clear();
            menuUI("delete");
            Console.Write("| Account No: ");
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            menuUI("searchBottom");
            Console.SetCursorPosition(cursorX, cursorY);
            int enterredNo = 0;

            while (!(int.TryParse(Console.ReadLine(), out enterredNo)))
            {
                Console.SetCursorPosition(0, cursorY + 5);
                Console.WriteLine("|             Invalid account number.");
                Console.WriteLine("|      Please enter a valid account number.");
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write("       ");
                Console.SetCursorPosition(cursorX, cursorY);
            }
            Account a = searchAccount(enterredNo);
            if (a != null)
            {
                Console.SetCursorPosition(cursorX, cursorY);
                string fullName = a.fName + " " + a.lName;
                Console.Write($"{a.accountNo} | Name:  {fullName.PadLeft(20)}\n\n");
                Console.WriteLine($"|    Balance | $ {a.balance.ToString().PadLeft(24)}\n");
                Console.WriteLine($"|    Address | {a.address.PadRight(26)}");
                Console.WriteLine($"|      Phone | {a.phone.ToString().PadRight(26)}");
                Console.WriteLine($"|      Email | {a.email.PadRight(26)}");
                Console.WriteLine("|==================================================|");

            }
            else
            {
                Console.SetCursorPosition(0, cursorY + 5);
                Console.WriteLine($"|         There is no account with number: ");
                Console.WriteLine($"|                      [{enterredNo}]");
            }
            Console.Write($"|          Delete this account (y/n)? ");
            if (Console.ReadLine() == "y")
            {
                string[] filePaths = Directory.GetFiles(accDir);
                foreach (string file in filePaths)
                {
                    if (file.Equals(accDir + "\\" + a.accountNo + ".txt"))
                    {
                        File.Delete(file);
                    }
                    readAccounts();
                    writeAccounts(-1);
                }

            }
        }


        /********************************************************************************************************************
        *           readAccounts
        *               This method first finds the directory of the accounts. It then opens each textfile and 
        *               splits the contents with the delimiter '|'. Each segment is then inputted into their
        *               respective variable and then inputted into the array accounts[].
        *               If the file cannot be found, the method outputs an error message.
        *           
        *           Inputs:
        *               N/A
        *           Outputs:
        *               int: Returns the current amount of filled accounts within the accounts array
        ********************************************************************************************************************/
        public int readAccounts()
        {
            int pos = 0;
            try
            {
                string[] filePaths = Directory.GetFiles(accDir);
                foreach (string file in filePaths)
                {
                    string[] details = System.IO.File.ReadAllText(file).Split('|');
                    int accountNo = int.Parse(details[0]);
                    string fname = details[1];
                    string lname = details[2];
                    string address = details[3];
                    double phone = Convert.ToDouble(details[4]);
                    string email = details[5];
                    double balance = Convert.ToDouble(details[6]);
                    accounts[pos] = new Account(accountNo, fname, lname, address, phone, email, balance);
                    pos++;
                }
            }

            catch (FileNotFoundException e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n         Account file could not be found.");
                Console.ReadKey();

            }
            return pos;
        }

        /********************************************************************************************************************
        *           writeAccounts
        *               This function does the inverse of the readAccounts() function. It opens each of the text
        *               files within accounts directory and inputs a culminated string gathered from the accounts
        *               array. 
        *               If the file cannot be found, the method outputs an error message.
        *           
        *           Inputs:
        *               sizeChange(int): A value to alter the loop to make up for any changes in array size
        *           Outputs:
        *               N/A
        ********************************************************************************************************************/
        public void writeAccounts(int sizeChange)
        {
            string[] accountsArray = new string[20];
            try
            {
                string[] filePaths = Directory.GetFiles(accDir);
                for (int i = 0; i < (filePaths.Length + sizeChange); i++)
                {
                    accountsArray[i] = $"{accounts[i].accountNo}|{accounts[i].fName}|{accounts[i].lName}|{accounts[i].address}|{accounts[i].phone}|{accounts[i].email}|{accounts[i].balance}|";
                    string filePath = accDir + "\\" + accounts[i].accountNo + ".txt";
                    System.IO.File.WriteAllText(filePath, accountsArray[i]);

                    string[] transactions = System.IO.File.ReadAllLines(tranFile);
                    foreach (string transaction in transactions)
                    {
                        string[] details = transaction.Split('-');
                        if (details[0].Equals(accounts[i].accountNo.ToString()))
                        {
                            System.IO.File.AppendAllText(filePath, "\n" + details[1]);
                        }

                    }
                }
            }

            catch (System.IO.IOException e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n         The account is already being accessed from another method.");
                Console.ReadKey();
            }
        }

    }

    class BankingSystem
    {
        static void Main(string[] args)
        {
            AccountDB database = new AccountDB();

            while (!database.login())
            {

                Console.WriteLine("|        Username and/or Password Incorrect!");
                Console.WriteLine("|               Press Enter to Retry");
                Console.ReadKey();
                Console.Clear();
            }
            Console.WriteLine("|                  Login Successful");
            Console.WriteLine("|              Press Enter to Continue");
            Console.ReadKey();
            Console.Clear();

            database.readAccounts();
            int choice = 0;
            while (choice != 7)
            {
                Console.Clear();
                database.menuUI("mainTop");
                Console.Write("|           Enter your choice (1-7): ");
                int choiceX = Console.CursorLeft;
                int choiceY = Console.CursorTop;
                Console.WriteLine("              |");
                database.menuUI("endLine");
                Console.SetCursorPosition(choiceX, choiceY);
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    choice = '0';
                }

                if (choice == 1)
                {
                    int index = database.readAccounts();
                    database.createAccount(index);
                }
                else if (choice == 2)
                {
                    database.displayAccount();
                }
                else if (choice == 3)
                {
                    database.changeBalance('d');
                }
                else if (choice == 4)
                {
                    database.changeBalance('w');
                }
                else if (choice == 5) database.getStatement();
                else if (choice == 6) database.deleteAccount();
                else if (choice == 7)
                {
                    choiceX = Console.CursorLeft;
                    choiceY = Console.CursorTop;
                    Console.SetCursorPosition(choiceX+17, choiceY+2);
                    Console.WriteLine("Closing Application");
                }
                else Console.WriteLine("Invalid Input");
            }
        }
    }
}
