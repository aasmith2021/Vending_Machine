using System;
using System.Text;
using System.Collections.Generic;

namespace Capstone
{
    public static class UserInterface
    {
        //This method displays the text and gets the input from the user for the main menu
        public static string RunMainMenu(IUserIO userIO)
        {
            string[] mainMenuOptions = new string[] { "Display Vending Machine Items", "Purchase", "Exit" };
            List<string> optionChoices = GetOptionChoicesForMenu(mainMenuOptions);
            optionChoices.Add((optionChoices.Count + 1).ToString());

            string header = "WELCOME TO THE VENDO-MATIC 800";
            UIHeaderAndMenu(userIO, header, mainMenuOptions);
            string userOption = GetUserOption(userIO, optionChoices);

            return userOption;
        }

        //Displays the vending machine inventory on the console
        public static void PrintInventory(IList<Slot> inventoryList, IUserIO userIO)
        {
            string header = "------------- VENDING MACHINE INVENTORY --------------";
            string[] noOptions = new string[] { };
            UIHeaderAndMenu(userIO, header, noOptions);

            string slotColumnTitle = "SLOT";
            string itemNameColumnTitle = "ITEM NAME";
            string priceColumnTitle = "PRICE";
            string qtyColumnTitle = "QTY";

            userIO.DisplayData($"{slotColumnTitle,-10}{itemNameColumnTitle,-30}{priceColumnTitle,-10}{qtyColumnTitle,-10}");
            userIO.DisplayData();

            foreach (Slot slot in inventoryList)
            {
                string itemQuantity;

                if (slot.Quantity == 0)
                {
                    itemQuantity = "SOLD OUT";
                }
                else
                {
                    itemQuantity = slot.Quantity.ToString();
                }

                string slotId = slot.SlotId;
                string itemName = slot.ItemInSlot.Name;
                decimal itemPrice = slot.Price;

                userIO.DisplayData($"{slotId,-10}{itemName,-30}{itemPrice,-10:C2}{itemQuantity,-10}");
            }
        }

        //Runs when the user selects "Display Vending Machine Items" from the vending machine's main menu
        public static void DisplayVendingMachineItems(IList<Slot> inventoryList, IUserIO userIO)
        {
            PrintInventory(inventoryList, userIO);
            EndOfMenu(userIO, "Press \"Enter\" to return to main menu...");
        }

        //Displays the purchase menu and allows the user to select from the available purchase options
        public static string RunPurchaseMenu(IUserIO userIO, decimal currentBalance)
        {
            string[] purchaseMenuOptions = new string[] { "Feed Money", "Select Product", "Finish Transaction" };
            List<string> optionChoices = GetOptionChoicesForMenu(purchaseMenuOptions);

            string header = "------- PURCHASE ITEMS -------";
            UIHeaderAndMenu(userIO, header, purchaseMenuOptions, "", true, currentBalance);
            string userOption = GetUserOption(userIO, optionChoices);

            return userOption;
        }

        //Displays the Feed Money menu and allows the user to enter a dollar amount to add to their current balance
        public static int RunFeedMoneyMenu(IUserIO userIO, decimal currentBalance)
        {
            string header = "----- FEED MONEY -----";
            string message = currentBalance == 1000 ? $"Current Balance: {currentBalance:C2}" : $"Current Balance: {currentBalance:C2}{Environment.NewLine}{Environment.NewLine}Insert money by typing in a whole dollar amount, no cents (ex: 5), or \"Exit\" to exit:";
            string[] noOptions = new string[] { };
            UIHeaderAndMenu(userIO, header, noOptions, message);

            int moneyToAdd = 0;

            if (currentBalance == 1000)
            {
                EndOfMenu(userIO, "Press \"Enter\" to return to the purchase menu...", "You have already reached the maximum vending machine balance of $1,000.", true);
            }
            else
            {
                string userInput = GetUserMoney(userIO, currentBalance);
                int result;

                if (int.TryParse(userInput, out result))
                {
                    moneyToAdd = result;
                    EndOfMenu(userIO, $"Success! {moneyToAdd:C2} was added to your balance. Press \"Enter\" to continue...");
                }
            }
            
            return moneyToAdd;
        }

        //Get's the user's money amount input for the <FeedMoney> aspect of the vending machine
        public static string GetUserMoney(IUserIO userIO, decimal currentBalance)
        {            
            int result;
            string userInput = userIO.GetInput().ToUpper();

            while ((!int.TryParse(userInput, out result) || result < 1 || currentBalance + result > 1000) && userInput != "EXIT")
            {
                if (!int.TryParse(userInput, out result) || result < 1)
                {
                    userIO.DisplayData();
                    userIO.DisplayData("Invalid entry. Please enter a whole dollar amount, no cents (ex: 5), or \"Exit\" to exit:");
                }
                else if (currentBalance + result > 1000)
                {
                    userIO.DisplayData();
                    userIO.DisplayData("Invalid entry; the vending machine balance cannot exceed $1,000.");
                    userIO.DisplayData("Please enter a whole dollar amount, no cents (ex: 5), or \"Exit\" to exit:");
                }

                userInput = userIO.GetInput().ToUpper();
            }

            return userInput;
        }

        //This menu gets input from the user to purchase a product and allows them to make a purchase if the
        //item is not sold out and they have enouth money to make the purchase
        public static string RunSelectProductMenu(IUserIO userIO, SortedList<string, Slot> inventoryList, decimal currentBalance)
        {
            userIO.ClearDisplay();
            UserInterface.PrintInventory(inventoryList.Values, userIO);
            userIO.DisplayData();
            userIO.DisplayData($"Current Balance: {currentBalance:C2}");

            userIO.DisplayData();
            userIO.DisplayData("Enter the slot of the item you wish to purchase, or \"Exit\" to exit:");
            string userOption = userIO.GetInput().ToUpper();

            while ((!inventoryList.ContainsKey(userOption) || inventoryList[userOption].Quantity == 0 || inventoryList[userOption].Price > currentBalance) && userOption != "EXIT")
            {
                userIO.DisplayData();

                if (!inventoryList.ContainsKey(userOption) && userOption != "EXIT")
                {
                    userIO.DisplayData("Invalid entry. Please enter the slot of the item you wish to purchase, or \"Exit\" to exit:");
                }
                else if (inventoryList[userOption].Quantity == 0)
                {
                    userIO.DisplayData("That item is SOLD OUT. Please enter a different slot for an item you wish to purchase, or \"Exit\" to exit:");
                }
                else if (inventoryList[userOption].Price > currentBalance)
                {
                    userIO.DisplayData("You don't have enough funds to purchase that item. Please enter a different slot for an item you wish to purchase, or \"Exit\" to exit:");
                }

                userOption = userIO.GetInput().ToUpper();
            }

            return userOption;
        }

        //Displays the item's message for the user once it is purchased
        public static void PrintItemMessage(IUserIO userIO, Item item, Slot slot, decimal currentBalance)
        {
            string message = item.Message;
            userIO.DisplayData();
            userIO.DisplayData($"{message} {message}, Yum!");
            userIO.DisplayData();
            userIO.DisplayData($"Bought 1 {item.Name} for {slot.Price:C2}; you have {currentBalance:C2} remaining.");
        }

        //Displays a menu of the provided menu options and can display the vending machine current balance
        public static void DisplayMenu(IUserIO userIO, string[] menuOptions, bool displayBalance = false, decimal currentBalance = 0)
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                userIO.DisplayData($"({i + 1}) {menuOptions[i]}");
            }

            if (displayBalance)
            {
                userIO.DisplayData();
                userIO.DisplayData($"Current Balance: {currentBalance:C2}");
            }
        }

        //Takes a list of menu options and returns a list of the valid option
        public static List<string> GetOptionChoicesForMenu(string[] menuOptions)
        {
            List<string> optionChoices = new List<string>();

            for (int i = 0; i < menuOptions.Length; i++)
            {
                optionChoices.Add((i + 1).ToString());
            }
            return optionChoices;
        }

        //Get the user's input from a list of valid options
        public static string GetUserOption(IUserIO userIO, List<string> options)
        {
            string userInput = userIO.GetInput().ToUpper();
            string helperMessage = "Invalid entry. Please enter an option from the menu";

            while (!options.Contains(userInput))
            {
                userIO.DisplayData();
                userIO.DisplayData($"{helperMessage}: ");
                userInput = userIO.GetInput().ToUpper();
            }

            return userInput;
        }

        //This menu allows an administrator to login to the vending machine by entering a password
        public static bool AdminLogin(IUserIO userIO, string adminPassword)
        {
            bool loginSuccessful = false;

            userIO.DisplayData();
            userIO.DisplayData("Enter admin password (default: admin):");
            string pwAttempt = userIO.GetInput();

            if (pwAttempt == adminPassword)
            {
                loginSuccessful = true;
            }

            return loginSuccessful;
        }

        //This displays the Admin menu where the user can see the administrator functions of the machine
        public static string RunAdminMenu(IUserIO userIO)
        {
            string header = "------- ADMIN MENU --------";
            string[] adminOptions = new string[] { "Generate Sales Report", "Change Admin password", "Restock Vending Machine", "Return to Main Menu" };
            List<string> optionChoices = GetOptionChoicesForMenu(adminOptions);
            UIHeaderAndMenu(userIO, header, adminOptions);
            string userOption = GetUserOption(userIO, optionChoices);

            return userOption;
        }

        //This menu displays to prompt the user to enter a new password when changing the password of
        //the vending machine
        public static string ChangePasswordMenu(IUserIO userIO)
        {
            userIO.DisplayData();
            userIO.DisplayData("Enter new password: ");
            string newPassword = userIO.GetInput();

            return newPassword;
        }

        //This is a general display of a page header and a menu for the user to select from
        public static void UIHeaderAndMenu(IUserIO userIO, string header, string[] menuOptions, string message = "", bool displayBalance = false, decimal currentBalance = 0)
        {
            userIO.ClearDisplay();
            userIO.DisplayData(header);
            userIO.DisplayData();
            if (menuOptions.Length != 0)
            {
                DisplayMenu(userIO, menuOptions, displayBalance, currentBalance);
                userIO.DisplayData();
            }

            if (message != "")
            {
                userIO.DisplayData(message);
                userIO.DisplayData();
            }
        }

        //This displays a message at the end of a menu and prompts the user for input before continuing
        public static void EndOfMenu(IUserIO userIO, string exitMessage, string infoMessage = "", bool printInfoMessage = false)
        {
            userIO.DisplayData();

            if (printInfoMessage)
            {
                userIO.DisplayData(infoMessage);
                userIO.DisplayData();
            }

            userIO.DisplayData(exitMessage);
            userIO.GetInput();
        }
    }
}
