using System;
using System.Text;
using System.Collections.Generic;

namespace Capstone
{
    public class VendingMachine
    {
        //This field is the password for an admin to login to the vending machine to view
        //the sales report. It's default value is set to "admin" until it is changed.
        public static string adminPassword = "admin";

        //The <salesList> contains the count of the number of sales for each item
        public Dictionary<string, int> salesList = new Dictionary<string, int>();

        public VendingMachine(IUserIO userIO, IDataIO dataIO)
        {
            StockInventory(userIO, dataIO);
            CreateInitialSalesList();
            RunUI(userIO, dataIO);
        }

        public decimal CurrentBalance { get; set; }

        public SortedList<string, Slot> InventoryList { get; private set; } = new SortedList<string, Slot>();

        public decimal SalesSum { get; set; } = 0;

        //When a new <VendingMachine> object is created, this method creates
        //all of the inventory and "stocks" the vending machine with all of the
        //items that are read from the source provided to the dataIO
        public void StockInventory(IUserIO userIO, IDataIO dataIO)
        {
            List<string[]> inventory = ReadWrite.ReadInventory(userIO, dataIO);

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
                        newItem = new Candy(element[indexOfItemName], element[indexOfItemCategory]);
                        break;
                    case "Chip":
                        newItem = new Chip(element[indexOfItemName], element[indexOfItemCategory]);
                        break;
                    case "Drink":
                        newItem = new Drink(element[indexOfItemName], element[indexOfItemCategory]);
                        break;
                    case "Gum":
                        newItem = new Gum(element[indexOfItemName], element[indexOfItemCategory]);
                        break;
                    default:
                        newItem = new UnknownItem(element[indexOfItemName], element[indexOfItemCategory]);
                        break;
                }

                InventoryList[element[indexOfSlotId]] = new Slot(element[indexOfSlotId], newItem, decimal.Parse(element[indexOfItemPrice]));
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
        public void RunUI(IUserIO userIO, IDataIO dataIO)
        {
            bool exitProgram = false;

            do
            {
                string userOption = UserInterface.RunMainMenu(userIO);

                //This switch statement takes the user's option and calls that option's
                //method to run that section of the vending machine's User Interface.
                switch (userOption)
                {
                    case "1":
                        UserInterface.DisplayVendingMachineItems(InventoryList.Values, userIO);
                        break;
                    case "2":
                        Purchase(userIO, dataIO);
                        break;
                    case "3":
                        exitProgram = true;
                        break;
                    case "4":
                        AdminFeatures(userIO, dataIO);
                        break;
                    default:
                        break;
                }
            }
            while (!exitProgram);
        }

        //This method runs the Purchase functions of the vending machine
        public void Purchase(IUserIO userIO, IDataIO dataIO)
        {
            bool exitPurchase = false;

            do
            {
                string userOption = UserInterface.RunPurchaseMenu(userIO, CurrentBalance);

                switch (userOption)
                {
                    case "1":
                        FeedMoney(userIO, dataIO);
                        break;
                    case "2":
                        SelectProduct(userIO, dataIO);
                        break;
                    case "3":
                        FinishTransaction(userIO, dataIO);
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
        public void FeedMoney(IUserIO userIO, IDataIO dataIO)
        {
            int moneyToAdd = UserInterface.RunFeedMoneyMenu(userIO, CurrentBalance);
            
            decimal startingBalance = CurrentBalance;
            CurrentBalance += moneyToAdd;
            ReadWrite.LogEntry(userIO, dataIO, "FEED MONEY:", startingBalance, CurrentBalance);
        }

        //Displays all the items in the vending machine and prompts the user to select
        //a product they would like to purchase.
        public void SelectProduct(IUserIO userIO, IDataIO dataIO)
        {
            bool exitSelectProduct = false;

            do
            {
                string userOption = UserInterface.RunSelectProductMenu(userIO, InventoryList, CurrentBalance);

                if (userOption == "EXIT")
                {
                    exitSelectProduct = true;
                }
                else
                {
                    Slot slotWithItemToBuy = InventoryList[userOption];
                    BuyItem(userIO, dataIO, slotWithItemToBuy);
                }
            } while(!exitSelectProduct);
        }

        //Purchases and item for the user, reducing their current balance to pay for the
        //item, decrementing the quantity of the item in the vending machine, and
        //logging the sale to the Log file.
        public void BuyItem(IUserIO userIO, IDataIO dataIO, Slot slot)
        {
            Item itemToBuy = slot.ItemStack.Pop();
            
            decimal startingBalance = CurrentBalance;
            CurrentBalance -= slot.Price;
            SalesSum += slot.Price;
            salesList[itemToBuy.Name]++;

            string transactionForLog = $"{itemToBuy.Name} {slot.SlotId}";
            ReadWrite.LogEntry(userIO, dataIO, transactionForLog, startingBalance, CurrentBalance);

            UserInterface.PrintItemMessage(userIO, itemToBuy, slot, CurrentBalance);
            UserInterface.EndOfMenu(userIO, "Press \"Enter\" to continue...");
        }

        //Completes a user's purchase transactions by cashing out their current
        //balance in quarters, dimes, nickels, and pennies
        public void FinishTransaction(IUserIO userIO, IDataIO dataIO)
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

            userIO.DisplayData();
            userIO.DisplayData("Your transaction is complete!");
            userIO.DisplayData();
            userIO.DisplayData("Your change is:");

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
                userIO.DisplayData($"{changeAmounts[i].Item2} {change[i]}");
            }
            ReadWrite.LogEntry(userIO, dataIO, "GIVE CHANGE:", startingBalance, CurrentBalance);

            UserInterface.EndOfMenu(userIO, "Press \"Enter\" to continue...");
        }

        //This runs the Administrator features of the vending machine, including the
        //ability to generate a sales report that is saved to a file
        public void AdminFeatures(IUserIO userIO, IDataIO dataIO)
        {
            bool exitAdminMenu = false;

            do
            {
                if (UserInterface.AdminLogin(userIO, adminPassword))
                {
                    AdminMenu(userIO, dataIO);
                }
                else
                {
                    UserInterface.EndOfMenu(userIO, "Invalid password. Press \"Enter\" to continue...");
                }

                exitAdminMenu = true;
            }
            while (!exitAdminMenu);
        }

        public void AdminMenu(IUserIO userIO, IDataIO dataIO)
        {
            bool exitAdminMenu = false;

            do
            {
                string userOption = UserInterface.RunAdminMenu(userIO);

                switch(userOption)
                {
                    case "1":
                        GenerateSalesReport(userIO, dataIO, salesList, SalesSum);
                        break;
                    case "2":
                        ChangePassword(userIO);
                        break;
                    case "3":
                        RestockVendingMachine(userIO, dataIO);
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
        public static void ChangePassword(IUserIO userIO)
        {
            adminPassword = UserInterface.ChangePasswordMenu(userIO);
            UserInterface.EndOfMenu(userIO, "Admin password successfully changed! Press \"Enter\" to continue...");
        }

        //Writes a sales report to a file, using the current date and time in the file name
        public void GenerateSalesReport(IUserIO userIO, IDataIO dataIO, Dictionary<string, int> salesList, decimal salesSum)
        {
            ReadWrite.WriteSalesReport(userIO, dataIO, salesList, SalesSum);
            UserInterface.EndOfMenu(userIO, "Sales report successfully generated! Press \"Enter\" to continue...");
        }

        //Allows the administrator to restock the vending machine by loading the machine with
        //the default items from the Inventory file
        public void RestockVendingMachine(IUserIO userIO, IDataIO dataIO)
        {
            StockInventory(userIO, dataIO);
            UserInterface.EndOfMenu(userIO, "Machine successfully restocked! Press \"Enter\" to continue...");
        }
    }
}