using static System.Formats.Asn1.AsnWriter;
using System.Buffers.Text;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Security.Principal;
using System.Threading.Tasks;
using System.ComponentModel;
using System;
using System.Runtime.Intrinsics.X86;

namespace assignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Task 1: Conditional Statements

            //            Control Structure
            //In a bank, you have been given the task is to create a program that checks if a customer is eligible for
            //a loan based on their credit score and income.The eligibility criteria are as follows:
            //• Credit Score must be above 700.
            //• Annual Income must be at least $50, 000.



            int credit;
            int annual;
            Console.WriteLine("Enter the credit score");
            credit = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the annual income");
            annual = Convert.ToInt32(Console.ReadLine());

            if (credit > 700 && annual >= 50000)
            {
                Console.WriteLine("Eligible for loan");
            }
            else

            {
                Console.WriteLine("Not Eligible for loan");
            }



            //            2Task 2: Nested Conditional Statements
            //            Create a program that simulates an ATM transaction. Display options such as "Check Balance,"
            //"Withdraw," "Deposit,".Ask the user to enter their current balance and the amount they want to
            //withdraw or deposit. Implement checks to ensure that the withdrawal amount is not greater than the
            //available balance and that the withdrawal amount is in multiples of 100 or 500.Display appropriate
            //messages for success or failure.

            int choice;
            double balance, withdrawAmount, depositAmount;
            Console.WriteLine("Welcome to the ATM");
            Console.WriteLine("1.Check Balance");
            Console.WriteLine("2.Withdraw");
            Console.WriteLine("3.Deposit");

            Console.WriteLine("Enter your choice (1-3)");
            choice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the balance");
            balance = Convert.ToDouble(Console.ReadLine());

            switch (choice)
            {

                case 1:
                    Console.WriteLine($"Your current balance is: {balance}");
                    break;

                case 2:
                    Console.WriteLine("Enter amount to withdraw");
                    withdrawAmount = Convert.ToDouble(Console.ReadLine());

                    if (withdrawAmount > balance)
                    {
                        Console.WriteLine(" Insufficient balance.");
                    }
                    else if (withdrawAmount % 100 != 0 && withdrawAmount % 500 != 0)
                    {
                        Console.WriteLine(" Withdrawal amount must be in multiples of 100 or 500.");
                    }
                    else
                    {
                        balance -= withdrawAmount;
                        Console.WriteLine($"Transaction successful! New balance: {balance}");
                    }
                    break;

                case 3:
                    Console.WriteLine("\nEnter amount to deposit: ");
                    depositAmount = Convert.ToDouble(Console.ReadLine());
                    balance += depositAmount;
                    Console.WriteLine($"\nTransaction successful! New balance: {balance}");
                    break;

                default:
                    Console.WriteLine("\nInvalid choice! Please select a valid option.");
                    break;
            }

            //            Task 3: Loop Structures
            //You are responsible for calculating compound interest on savings accounts for bank customers. You
            //need to calculate the future balance for each customer's savings account after a certain number of years.
            //Tasks:
            //1.Create a program that calculates the future balance of a savings account.
            //2.Use a loop structure(e.g., for loop) to calculate the balance for multiple customers.
            //3.Prompt the user to enter the initial balance, annual interest rate, and the number of years.
            //4.Calculate the future balance using the formula:      
            //future_balance = initial_balance * (1 + annual_interest_rate / 100) ^ years.
            //5.Display the future balance for each customer.

            int num, years;
            double initialbal, interest, futurebalance;

            Console.WriteLine("Enter the number of customers: ");
            num = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= num; i++)
            {
                Console.WriteLine($"Customer {i}:");


                Console.Write("Enter initial balance: $");
                initialbal = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter annual interest rate (in %): ");
                interest = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Enter number of years: ");
                years = Convert.ToInt32(Console.ReadLine());


                futurebalance = initialbal * Math.Pow(1 + interest / 100, years);


                Console.WriteLine($"Future balance after {years} years:{futurebalance}");
            }

            //            Task 5: Password Validation
            //Write a program that prompts the user to create a password for their bank account.Implement if
            //conditions to validate the password according to these rules:
            //• The password must be at least 8 characters long.
            //• It must contain at least one uppercase letter.
            //• It must contain at least one digit.
            //• Display appropriate messages to indicate whether their password is valid or not.

            string password;
            Console.Write("Create your password (at least 8 characters, one uppercase letter, one digit): ");
            password = Console.ReadLine();

            if (password.Length < 8)
            {
                Console.WriteLine("Password must be at least 8 characters long.");
            }
            else if (!password.Any(char.IsUpper))
            {
                Console.WriteLine("Password must contain at least one uppercase letter.");
            }
            else if (!password.Any(char.IsDigit))
            {
                Console.WriteLine("Password must contain at least one digit.");
            }
            else
            {
                Console.WriteLine("Password is valid.");
            }


            //            Task 6: Password Validation
            //Create a program that maintains a list of bank transactions(deposits and withdrawals) for a customer.
            //Use a while loop to allow the user to keep adding transactions until they choose to exit.Display the
            //transaction history upon exit using looping statements. 

            string[] transaction = new string[100];
            int count = 0,ch;
            bool run = true;

            while (run)
            {
                Console.WriteLine("\n1. Deposit\n2. Withdrawal\n3. Exit");
                ch=Convert.ToInt32(Console.ReadLine());

                switch (ch)
                {
                    case 1:
                        Console.Write("Enter deposit amount");
                        double d = Convert.ToDouble(Console.ReadLine());
                        transaction[count] = $"Deposited: {d}";
                        count++;
                        Console.WriteLine($"Deposited {d} successfully.");
                        break;

                    case 2:
                        Console.Write("Enter withdrawal amount");
                        double withdraw = Convert.ToDouble(Console.ReadLine());
                        transaction[count] = $"Withdrew: {withdraw}";
                        count++;
                        Console.WriteLine($"Withdrew {withdraw} successfully.");
                        break;

                    case 3:
                        Console.WriteLine("Transaction History:");
                        for (int i = 0; i < count; i++)
                        {
                            Console.WriteLine(transaction[i]);
                        }
                        run = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
