using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Capstone
{
    public class VendingMachine
    {
        //This property is the password for an admin to login to the vending machine
        //to view the sales report. It's default value is set to "admin" until it is changed.
        public static string adminPassword = "admin";

        //The <salesList> contains the count of the number of sales for each item
        public Dictionary<string, int> salesList = new Dictionary<string, int>();

        public decimal SalesSum { get; set; } = 0;

        public decimal CurrentBalance { get; set; }

        public SortedList<string, Slot> InventoryList { get; private set; } = new SortedList<string, Slot>();

        public VendingMachine(IInput input, IOutput output, IDisplay display, IDataReader dataReader)
        {
            StockInventoryFromFile(dataReader);
            CreateInitialSalesList();
            RunUI(input, output, display, dataReader);
        }

        //When a new <VendingMachine> object is created, this method creates
        //all of the inventory and "stocks" the vending machine with all of the
        //items that are read from the Inventory.txt file.
        public void StockInventoryFromFile(IDataReader dataReader)
        {
            List<string[]> inventory = ReadWrite.ReadInventoryFile(dataReader);

            int indexOfSlotId = 0;
            int indexOfItemName = 1;
            int indexOfItemPrice = 2;
            int indexOfItemCategory = 3;

            foreach (string[] element in inventory)
            {
                Item newItem;

                switch (element[indexOfItemCategory])
                {
                    case "Candy":
                        newItem = new Candy(element[indexOfItemName], decimal.Parse(element[indexOfItemPrice]), element[indexOfItemCategory]);
                        break;
                    case "Chip":
                        newItem = new Chip(element[indexOfItemName], decimal.Parse(element[indexOfItemPrice]), element[indexOfItemCategory]);
                        break;
                    case "Drink":
                        newItem = new Drink(element[indexOfItemName], decimal.Parse(element[indexOfItemPrice]), element[indexOfItemCategory]);
                        break;
                    case "Gum":
                        newItem = new Gum(element[indexOfItemName], decimal.Parse(element[indexOfItemPrice]), element[indexOfItemCategory]);
                        break;
                    default:
                        newItem = new UnknownItem(element[indexOfItemName], decimal.Parse(element[indexOfItemPrice]), element[indexOfItemCategory]);
                        break;
                }

                InventoryList[element[indexOfSlotId]] = new Slot(element[indexOfSlotId], newItem);
            }
        }

        //When a new <VendingMachine> object is created, this method is called to
        //popluate the <salesList> with all of the items in the vending machine
        //and sets their quanity sold to 0
        public void CreateInitialSalesList()
        {
            foreach (KeyValuePair<string, Slot> slot in InventoryList)
            {
                salesList[slot.Value.ItemInSlot.Name] = 0;
            }
        }

        //When a new <VendingMachine> object is created, this method is called to
        //run the User Interface of the vending machine so a customer or vending machine
        //owner can interact with the machine. It is run in a "do-while" loop so that
        //the user interface continues to run until the user selects the "Exit" option.
        public void RunUI(IInput input, IOutput output, IDisplay display, IDataReader dataReader)
        {
            bool exitProgram = false;

            do
            {
                string userOption = UserInterface.RunMainMenu(input, display);

                //This switch statement takes the user's option and calls that option's
                //method to run that section of the vending machine's User Interface.
                switch (userOption)
                {
                    case "1":
                        UserInterface.DisplayVendingMachineItems(InventoryList.Values, input, display);
                        break;
                    case "2":
                        Purchase(input, output, display);
                        break;
                    case "3":
                        exitProgram = true;
                        break;
                    case "4":
                        AdminFeatures(input, output, display, dataReader);
                        break;
                    default:
                        break;
                }
            }
            while (!exitProgram);
        }

        //This method runs the Purchase functions of the vending machine
        public void Purchase(IInput input, IOutput output, IDisplay display)
        {
            bool exitPurchase = false;

            do
            {
                string userOption = UserInterface.RunPurchaseMenu(input, display, CurrentBalance);

                switch (userOption)
                {
                    case "1":
                        FeedMoney(input, output, display);
                        break;
                    case "2":
                        SelectProduct(input, output, display);
                        break;
                    case "3":
                        FinishTransaction(input, output, display);
                        exitPurchase = true;
                        break;
                    default:
                        break;
                }
            }
            while (!exitPurchase);
        }

        //Allows the user to feed a whole dollar amount into the vending machine
        //and adds the money amount fed in to their current balance.
        public void FeedMoney(IInput input, IOutput output, IDisplay display)
        {
            int moneyToAdd = UserInterface.RunFeedMoneyMenu(input, display, CurrentBalance);
            
            decimal startingBalance = CurrentBalance;
            CurrentBalance += moneyToAdd;
            ReadWrite.LogEntry(output, "FEED MONEY:", startingBalance, CurrentBalance);
        }

        //Displays all the items in the vending machine and prompts the user to select
        //a product they would like to purchase.
        public void SelectProduct(IInput input, IOutput output, IDisplay display)
        {
            bool exitSelectProduct = false;

            do
            {
                string userOption = UserInterface.RunSelectProductMenu(input, display, InventoryList, CurrentBalance);

                if (userOption == "EXIT")
                {
                    exitSelectProduct = true;
                }
                else
                {
                    Slot slotWithItemToBuy = InventoryList[userOption];
                    BuyItem(input, output, display, slotWithItemToBuy);
                }
            } while(!exitSelectProduct);
        }

        //Purchases and item for the user, reducing their current balance to pay for the
        //item, decrementing the quantity of the item in the vending machine, and
        //logging the sale to the Log file.
        public void BuyItem(IInput input, IOutput output, IDisplay display, Slot slot)
        {
            Item itemToBuy = slot.ItemStack.Pop();
            
            decimal startingBalance = CurrentBalance;
            CurrentBalance -= itemToBuy.Price;
            SalesSum += itemToBuy.Price;
            salesList[itemToBuy.Name]++;

            string transactionForLog = $"{itemToBuy.Name} {slot.SlotId}";
            ReadWrite.LogEntry(output, transactionForLog, startingBalance, CurrentBalance);

            UserInterface.PrintItemMessage(display, itemToBuy, CurrentBalance);
            UserInterface.EndOfMenu(input, display, "Press \"Enter\" to continue...");
        }

        //Completes a user's purchase transactions by cashing out their current
        //balance in quarters, dimes, nickels, and pennies
        public void FinishTransaction(IInput input, IOutput output, IDisplay display)
        {
            //The <change> array holds the numbers of:
            //[0] quarters
            //[1] dimes
            //[2] nickels
            //[3] pennies
            //to be given back to the vending machine patron

            int[] change = new int[4];
            (int, string)[] changeAmounts = new (int, string)[] {
                ( 25, "Quarters:" ),
                ( 10, "Dimes:" ),
                ( 5, "Nickels:" ),
                ( 1, "Pennies:" )
            };

            int amountRemaining = (int)(CurrentBalance * 100);

            display.DisplayData();
            display.DisplayData("Your transaction is complete!");
            display.DisplayData();
            display.DisplayData("Your change is:");

            //This for loop sets the amount of each coin denomination (i.e., quarter) to the greatest
            //whole number of the coins that can given from the <amountRemaining>. Then, the
            //CurrentBalance is reduced by that number of coin denomination (i.e., 5 quarters = 5 x .25)
            //The %= then reduces the <amountRemaining> to the amount of change that still needs to be
            //given but can only be made from the smaller denominations of coints (i.e., nickels)
            decimal startingBalance = CurrentBalance;
            for (int i = 0; i < change.Length; i++)
            {
                change[i] = amountRemaining / changeAmounts[i].Item1;
                CurrentBalance -= change[i] * (decimal)(changeAmounts[i].Item1 / 100m);
                amountRemaining %= changeAmounts[i].Item1;
                display.DisplayData($"{changeAmounts[i].Item2} {change[i]}");
            }
            ReadWrite.LogEntry(output, "GIVE CHANGE:", startingBalance, CurrentBalance);

            UserInterface.EndOfMenu(input, display, "Press \"Enter\" to continue...");
        }

        //This runs the Administrator features of the vending machine, including the
        //ability to generate a sales report that is saved to a file
        public void AdminFeatures(IInput input, IOutput output, IDisplay display, IDataReader dataReader)
        {
            bool exitAdminMenu = false;

            do
            {
                if (UserInterface.AdminLogin(input, display, adminPassword))
                {
                    AdminMenu(input, output, display, dataReader);
                }
                else
                {
                    UserInterface.EndOfMenu(input, display, "Invalid password. Press \"Enter\" to continue...");
                }

                exitAdminMenu = true;
            }
            while (!exitAdminMenu);
        }

        public void AdminMenu(IInput input, IOutput output, IDisplay display, IDataReader dataReader)
        {
            bool exitAdminMenu = false;

            do
            {
                string userOption = UserInterface.RunAdminMenu(input, display);

                switch(userOption)
                {
                    case "1":
                        GenerateSalesReport(input, output, display, salesList, SalesSum);
                        break;
                    case "2":
                        ChangePassword(input, display);
                        break;
                    case "3":
                        RestockVendingMachine(input, display, dataReader);
                        break;
                    case "4":
                        exitAdminMenu = true;
                        break;
                    default:
                        break;
                }

            } while (!exitAdminMenu);
        }

        //This metod is used to change the admin password of the vending machine
        public static void ChangePassword(IInput input, IDisplay display)
        {
            adminPassword = UserInterface.ChangePasswordMenu(input, display);
            UserInterface.EndOfMenu(input, display, "Admin password successfully changed! Press \"Enter\" to continue...");
        }

        //Writes a sales report to a file, using the current date and time in the file name
        public void GenerateSalesReport(IInput input, IOutput output, IDisplay display, Dictionary<string, int> salesList, decimal salesSum)
        {
            ReadWrite.WriteSalesReport(output, salesList, SalesSum);
            UserInterface.EndOfMenu(input, display, "Sales report successfully generated! Press \"Enter\" to continue...");
        }

        //Allows the administrator to restock the vending machine by loading the machine with
        //the default items from the Inventory.txt file
        public void RestockVendingMachine(IInput input, IDisplay display, IDataReader dataReader)
        {
            StockInventoryFromFile(dataReader);
            UserInterface.EndOfMenu(input, display, "Machine successfully restocked! Press \"Enter\" to continue...");
        }
    }
}