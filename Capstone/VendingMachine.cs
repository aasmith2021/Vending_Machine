using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Capstone
{
    public class VendingMachine
    {

        public decimal CurrentBalance { get; set; }
        public SortedList<string, Slot> InventoryList { get; private set; } = new SortedList<string, Slot>();
        public decimal SalesSum { get; set; } = 0;

        public Dictionary<string, int> salesList = new Dictionary<string, int>();

        public VendingMachine(bool run)
        {
            InitInventory();
            InitSalesList();

            if (run)
            {
                VendUI.RunUI(this);
            }
        }

        //When a new <VendingMachine> object is created, this method is called to
        //popluate the <salesList> with all of the items in the vending machine
        //and sets their quanity sold to 0
        public void InitSalesList()
        {
            foreach (KeyValuePair<string, Slot> slot in InventoryList)
            {
                salesList[slot.Value.ItemInSlot.Name] = 0;
            }
        }

        //When a new <VendingMachine> object is created, this method creates
        //all of the inventory and "stocks" the vending machine with all of the
        //items that are read from the Inventory.txt file.
        public void InitInventory()
        {
            VendIO.ReadInput(this);
        }

        //Displays the purchase menu and allows the user to select from the available
        //purchase options
        public void Purchase()
        {
            bool exitPurchase = false;
            string[] purchaseMenuOptions = new string[] { "Feed Money", "Select Product", "Finish Transaction" };
            List<string> optionChoices = VendUI.GetOptionChoicesForMenu(purchaseMenuOptions);

            do
            {
                string header = "------- PURCHASE ITEMS -------";
                VendUI.UIFrameHeaderAndMenu(header, purchaseMenuOptions, "", true, CurrentBalance);
                string userOption = VendUI.GetUserOption(optionChoices, new InputGetter());

                switch (userOption)
                {
                    case "1":
                        FeedMoney();
                        break;
                    case "2":
                        SelectProduct();
                        break;
                    case "3":
                        FinishTransaction();
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
        public void FeedMoney()
        {
            string header = "----- FEED MONEY -----";
            string message = $"Insert money by typing in a whole dollar amount, no cents (ex: 5) or \"Exit\" to{Environment.NewLine}exit:";
            string[] noOptions = new string[] { };
            VendUI.UIFrameHeaderAndMenu(header, noOptions, message);
            string userInput = Console.ReadLine().ToUpper();

            if (userInput == "EXIT")
            {
                return;
            }
            else
            {
                int userMoney = VendUI.GetUserMoney(userInput, new InputGetter());
                decimal prevBalance = CurrentBalance;
                CurrentBalance += userMoney;
                VendIO.LogEntry("FEED MONEY:", prevBalance, CurrentBalance);
            }

        }

        //Displays all the items in the vending machine and prompts the user to select
        //a product they would like to purchase.
        public void SelectProduct()
        {
            Console.Clear();
            VendUI.PrintInventory(InventoryList.Values);
            Console.WriteLine();
            Console.WriteLine($"Current Money Provided: {CurrentBalance:C2}");

            Console.WriteLine();
            Console.WriteLine("Enter the slot of the item you wish to purchase or \"Exit\" to exit:");
            string userOption = Console.ReadLine().ToUpper();

            if (userOption == "EXIT")
            {
                return;
            }

            if (InventoryList.ContainsKey(userOption))
            {
                ItemInStock(InventoryList[userOption]);
            }
            else
            {
                VendUI.EndOfMenu("Invalid input; press \"Enter\" to continue...");

                SelectProduct();
            }
        }

        //Completes a user's purchase transactions by cashing out their current
        //balance in quarters, dimes, nickels, and pennies
        public void FinishTransaction()
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

            Console.WriteLine();
            Console.WriteLine("Your transaction is complete!");
            Console.WriteLine();
            Console.WriteLine("Your change is:");

            //This for loop sets the amount of each coin denomination
            //(i.e., quarter) to the greatest whole number of the coins
            //that can given from the <amountRemaining>.
            //Then, the CurrentBalance is reduced by that number of coin denomination (i.e., 5 quarters = 5 x .25)
            //The %= then reduces the <amountRemaining> to the amount
            //of change that still needs to be given but can only be
            //made from the smaller denominations of coints (i.e., nickels)
            decimal prevBalance = CurrentBalance;
            for (int i = 0; i < change.Length; i++)
            {
                change[i] = amountRemaining / changeAmounts[i].Item1;
                CurrentBalance -= change[i] * (decimal)(changeAmounts[i].Item1 / 100m);
                amountRemaining %= changeAmounts[i].Item1;
                Console.WriteLine($"{changeAmounts[i].Item2} {change[i]}");
            }
            VendIO.LogEntry("GIVE CHANGE:", prevBalance, CurrentBalance);

            VendUI.EndOfMenu("Press \"Enter\" to continue...");
        }

        //Checks if an item is in stock before continuing with the item's purchase
        public void ItemInStock(Slot selectedSlot)
        {
            if (selectedSlot.Quantity > 0)
            {
                if (BuyProduct(selectedSlot))
                {
                    selectedSlot.ItemStack.Pop();
                }
            }
            else
            {
                VendUI.EndOfMenu("Press \"Enter\" to continue...", "That item is sold out. Please select a different product.", true);
                SelectProduct();
            }
        }

        //Finalizes an item's sale by ensuring the user has enough money to
        public bool BuyProduct(Slot slot)
        {
            bool purchaseSuccessful = false;

            if (CurrentBalance >= slot.ItemInSlot.Price)
            {
                decimal prevBalance = CurrentBalance;
                CurrentBalance -= slot.ItemInSlot.Price;
                SalesSum += slot.ItemInSlot.Price;
                salesList[slot.ItemInSlot.Name]++;

                VendIO.LogEntry($"{slot.ItemInSlot.Name} {slot.SlotId}", prevBalance, CurrentBalance);
                purchaseSuccessful = true;

                VendUI.PrintItemMessage(slot.ItemInSlot, CurrentBalance);
                VendUI.EndOfMenu("Press \"Enter\" to continue...");
            }
            else
            {
                VendUI.EndOfMenu("Your current balance is too low to buy this item. Press \"Enter\" to continue...");
                SelectProduct();
            }

            return purchaseSuccessful;
        }

    }
}