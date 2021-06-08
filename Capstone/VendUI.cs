using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class VendUI : VendingMachine
    {
        //This property is the password for an admin to login to the vending
        //machine to view the sales report. It's default value is set to "admin"
        //until it is changed.
        protected static string AdminPassword = "admin";

        //This constructor is provided to create test instances
        //of a VendingMachine object
        public VendUI() : base(false)
        {

        }

        //When a new <VendingMachine> object is created, this method is called to
        //run the User Interface of the vending machine so a customer or vending machine
        //owner can interact with the machine. It is run in a "do-while" loop so that
        //the user interface continues to run until the user selects the "Exit" option.
        public static void RunUI(VendingMachine vm)
        {
            bool exitProgram = false;

            do
            {
                string[] mainMenuOptions = new string[] { "Display Vending Machine Items", "Purchase", "Exit" };
                List<string> optionChoices = GetOptionChoicesForMenu(mainMenuOptions);
                optionChoices.Add((optionChoices.Count + 1).ToString());

                string header = "WELCOME TO THE VENDO-MATIC 800";
                UIFrameHeaderAndMenu(header, mainMenuOptions);
                string userOption = GetUserOption(optionChoices, new InputGetter());

                //This switch statement takes the user's option and calls that option's
                //method to run that section of the vending machine.
                switch (userOption)
                {
                    case "1":
                        DisplayVendingMachineItems(vm.InventoryList.Values);
                        break;
                    case "2":
                        vm.Purchase();
                        break;
                    case "3":
                        exitProgram = true;
                        break;
                    case "4":
                        if (AdminLogin(new InputGetter()))
                        {
                            DisplayAdminMenu(vm);
                        }
                        else
                        {
                            EndOfMenu("Invalid password. Press \"Enter\" to return to the main menu...");
                        }
                        break;
                    default:
                        break;
                }
            }
            while (!exitProgram);
        }

        //Displays the vending machine inventory on the console
        public static void PrintInventory(IList<Slot> InventoryList)
        {
            string header = "------------- VENDING MACHINE INVENTORY --------------";
            string[] noOptions = new string[] { };
            UIFrameHeaderAndMenu(header, noOptions);

            Console.WriteLine("{0,-10}{1,-30}{2,-10}{3,-10}", "SLOT", "ITEM NAME", "PRICE", "QTY");
            Console.WriteLine();

            foreach (Slot slot in InventoryList)
            {
                string displayQuantity = slot.Quantity.ToString();

                if (slot.Quantity == 0)
                {
                    displayQuantity = "SOLD OUT";
                }
                Console.WriteLine("{0,-10}{1,-30}{2,-10:C2}{3,-10}", slot.SlotId, slot.ItemInSlot.Name, slot.ItemInSlot.Price, displayQuantity);
            }
        }

        //Runs when the user selects "Display Vending Machine Items" from the
        //vending machine's main menu
        public static void DisplayVendingMachineItems(IList<Slot> inventoryList)
        {
            PrintInventory(inventoryList);
            EndOfMenu("Press \"Enter\" to return to main menu...");
        }


        //Get the user's input from a list of valid options
        public static string GetUserOption(List<string> options, IUserInput input)
        {
            string userInput = input.GetInput().ToUpper();
            string helperMessage = "Invalid entry. Please enter an option from the menu";

            while (!options.Contains(userInput))
            {
                Console.WriteLine();
                Console.Write($"{helperMessage}: ");
                userInput = input.GetInput().ToUpper();
            }

            return userInput;
        }

        //Get's the user's money amount input for the <FeedMoney> aspect of the vending machine
        public static int GetUserMoney(string userInput, IUserInput input)
        {
            int result;

            while (!int.TryParse(userInput, out result) || result < 1)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid entry. Please enter a whole dollar amount, no cents (ex: 5): ");

                userInput = input.GetInput();
            }

            return result;
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

        public static void PrintItemMessage(Item item, decimal currentBalance)
        {
            string message = item.Message;
            Console.WriteLine();
            Console.WriteLine($"{message} {message}, Yum!{Environment.NewLine}Bought 1 {item.Name} for {item.Price:C2}; you have {currentBalance:C2} remaining.");
        }

        public static void DisplayMenu(string[] menuOptions, bool displayBalance = false, decimal currentBalance = 0)
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine($"({i + 1}) {menuOptions[i]}");
            }

            if (displayBalance)
            {
                Console.WriteLine();
                Console.WriteLine($"Current Money Provided: {currentBalance:C2}");
            }
        }

        public static bool AdminLogin(IUserInput input)
        {
            Console.Write("Enter admin password (default: admin): ");
            string pwAttempt = input.GetInput();
            if (pwAttempt == AdminPassword)
            {
                return true;
            }
            return false;
        }

        protected static void ChangePassword(IUserInput input)
        {
            Console.Write("Enter new password: ");
            AdminPassword = input.GetInput();
        }

        protected static void DisplayAdminMenu(VendingMachine vm)
        {
            bool exitAdminMenu = false;

            do
            {
                string header = "------- ADMIN MENU --------";
                string[] adminOptions = new string[] { "Generate Sales Report", "Change Admin password", "Restock Vending Machine", "Return to Main Menu" };
                List<string> optionChoices = GetOptionChoicesForMenu(adminOptions);
                UIFrameHeaderAndMenu(header, adminOptions);
                string userOption = GetUserOption(optionChoices, new InputGetter());
                switch (userOption)
                {
                    case "1":
                        VendIO.WriteSalesReport(vm);
                        EndOfMenu("Sales report successfully generated! Press \"Enter\" to continue...");
                        break;
                    case "2":
                        ChangePassword(new InputGetter());
                        EndOfMenu("Admin password successfully changed! Press \"Enter\" to continue...");
                        break;
                    case "3":
                        vm.InitInventory();
                        EndOfMenu("Machine successfully restocked! Press \"Enter\" to continue...");
                        break;
                    case "4":
                        exitAdminMenu = true;
                        break;
                    default:
                        break;
                }

            } while (!exitAdminMenu);
        }

            public static void UIFrameHeaderAndMenu(string header, string[] menuOptions, string message = "", bool displayBalance = false, decimal currentBalance = 0)
        {
            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine();
            if (menuOptions.Length != 0)
            {
                DisplayMenu(menuOptions, displayBalance, currentBalance);
                Console.WriteLine();
            }

            if (message != "")
            {
                Console.WriteLine(message);
                Console.WriteLine();
            }
        }

        public static void UIFrameHeaderAndMessage(string header, string[] menuOptions, bool displayBalance = false, decimal currentBalance = 0)
        {
            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine();
            DisplayMenu(menuOptions, displayBalance, currentBalance);
        }

        public static void EndOfMenu(string exitMessage, string infoMessage = "", bool printInfoMessage = false)
        {
            Console.WriteLine();

            if (printInfoMessage)
            {
                Console.WriteLine(infoMessage);
                Console.WriteLine();
            }

            Console.WriteLine(exitMessage);
            Console.ReadLine();
        }

    }
}
