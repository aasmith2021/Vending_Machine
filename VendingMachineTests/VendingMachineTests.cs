using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using VendingMachineApplication;

namespace VendingMachineTests
{
    [TestClass]
    public class VendingMachineTests
    {
        private Item[] GetTestItems()
        {
            Item[] testItems = new Item[]
            {
                new Chip("Potato Crisps", "Chip"),
                new Chip("Stackers", "Chip"),
                new Chip("Grain Waves", "Chip"),
                new Chip("Cloud Popcorn", "Chip"),
                new Candy("Moonpie", "Candy"),
                new Candy("Cowtales", "Candy"),
                new Candy("Wonka Bar", "Candy"),
                new Candy("Crunchie", "Candy"),
                new Drink("Cola", "Drink"),
                new Drink("Dr. Salt", "Drink"),
                new Drink("Mountain Melter", "Drink"),
                new Drink("Heavy", "Drink"),
                new Gum("U-Chews", "Gum"),
                new Gum("Little League Chew", "Gum"),
                new Gum("Chiclets", "Gum"),
                new Gum("Triplemint", "Gum"),
            };

            return testItems;
        }

        private Slot[] GetTestSlots(Item[] testItems)
        {
            Slot[] testSlots = new Slot[]
            {
                new Slot("A1", testItems[0], 3.05m, 5),
                new Slot("A2", testItems[1], 1.45m, 5),
                new Slot("A3", testItems[2], 2.75m, 5),
                new Slot("A4", testItems[3], 3.65m, 5),
                new Slot("B1", testItems[4], 1.80m, 5),
                new Slot("B2", testItems[5], 1.50m, 5),
                new Slot("B3", testItems[6], 1.50m, 5),
                new Slot("B4", testItems[7], 1.75m, 5),
                new Slot("C1", testItems[8], 1.25m, 5),
                new Slot("C2", testItems[9], 1.50m, 5),
                new Slot("C3", testItems[10], 1.50m, 5),
                new Slot("C4", testItems[11], 1.50m, 5),
                new Slot("D1", testItems[12], 0.85m, 5),
                new Slot("D2", testItems[13], 0.95m, 5),
                new Slot("D3", testItems[14], 0.75m, 5),
                new Slot("D4", testItems[15], 0.75m, 5)
            };

            return testSlots;
        }

        [TestMethod]
        public void Creating_new_Vending_Machine_populates_vending_machine_with_items_from_Inventory_file()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            IDataIO dataIO = new FileIO();
            VendingMachine testVM = new VendingMachine(userIO, dataIO);

            Slot[] testSlots = GetTestSlots(GetTestItems());
            SortedList<string, Slot> expectedInventoryList = new SortedList<string, Slot>()
            {
                { testSlots[0].SlotId, testSlots[0] },
                { testSlots[1].SlotId, testSlots[1] },
                { testSlots[2].SlotId, testSlots[2] },
                { testSlots[3].SlotId, testSlots[3] },
                { testSlots[4].SlotId, testSlots[4] },
                { testSlots[5].SlotId, testSlots[5] },
                { testSlots[6].SlotId, testSlots[6] },
                { testSlots[7].SlotId, testSlots[7] },
                { testSlots[8].SlotId, testSlots[8] },
                { testSlots[9].SlotId, testSlots[9] },
                { testSlots[10].SlotId, testSlots[10] },
                { testSlots[11].SlotId, testSlots[11] },
                { testSlots[12].SlotId, testSlots[12] },
                { testSlots[13].SlotId, testSlots[13] },
                { testSlots[14].SlotId, testSlots[14] },
                { testSlots[15].SlotId, testSlots[15] }
            };

            //Act
            SortedList<string, Slot> actualInventoryList = testVM.InventoryList;

            //Assert
            for (int i = 0; i < expectedInventoryList.Count; i++)
            {
                string expectedItemName = expectedInventoryList.Values[i].ItemInSlot.Name;
                string expectedItemCategory = expectedInventoryList.Values[i].ItemInSlot.Category;
                decimal expectedItemPrice = expectedInventoryList.Values[i].Price;

                string actualItemName = actualInventoryList.Values[i].ItemInSlot.Name;
                string actualItemCategory = actualInventoryList.Values[i].ItemInSlot.Category;
                decimal actualItemPrice = actualInventoryList.Values[i].Price;

                Assert.AreEqual(expectedItemName, actualItemName, "Item name of a vending machine item does not match the expected name");
                Assert.AreEqual(expectedItemCategory, actualItemCategory, "Item category of a vending machine item does not match the expected category");
                Assert.AreEqual(expectedItemPrice, actualItemPrice, "Item price of a vending machine item does not match the expected price");
            }
        }

        [TestMethod]
        public void Creating_new_Vending_Machine_populates_vending_machine_with_items_from_Database()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            IDataIO dataIO = new DatabaseIO();
            VendingMachine testVM = new VendingMachine(userIO, dataIO);

            Slot[] testSlots = GetTestSlots(GetTestItems());
            SortedList<string, Slot> expectedInventoryList = new SortedList<string, Slot>()
            {
                { testSlots[0].SlotId, testSlots[0] },
                { testSlots[1].SlotId, testSlots[1] },
                { testSlots[2].SlotId, testSlots[2] },
                { testSlots[3].SlotId, testSlots[3] },
                { testSlots[4].SlotId, testSlots[4] },
                { testSlots[5].SlotId, testSlots[5] },
                { testSlots[6].SlotId, testSlots[6] },
                { testSlots[7].SlotId, testSlots[7] },
                { testSlots[8].SlotId, testSlots[8] },
                { testSlots[9].SlotId, testSlots[9] },
                { testSlots[10].SlotId, testSlots[10] },
                { testSlots[11].SlotId, testSlots[11] },
                { testSlots[12].SlotId, testSlots[12] },
                { testSlots[13].SlotId, testSlots[13] },
                { testSlots[14].SlotId, testSlots[14] },
                { testSlots[15].SlotId, testSlots[15] }
            };

            //Act
            SortedList<string, Slot> actualInventoryList = testVM.InventoryList;

            //Assert
            for (int i = 0; i < expectedInventoryList.Count; i++)
            {
                string expectedItemName = expectedInventoryList.Values[i].ItemInSlot.Name;
                string expectedItemCategory = expectedInventoryList.Values[i].ItemInSlot.Category;
                decimal expectedItemPrice = expectedInventoryList.Values[i].Price;

                string actualItemName = actualInventoryList.Values[i].ItemInSlot.Name;
                string actualItemCategory = actualInventoryList.Values[i].ItemInSlot.Category;
                decimal actualItemPrice = actualInventoryList.Values[i].Price;

                Assert.AreEqual(expectedItemName, actualItemName, "Item name of a vending machine item does not match the expected name");
                Assert.AreEqual(expectedItemCategory, actualItemCategory, "Item category of a vending machine item does not match the expected category");
                Assert.AreEqual(expectedItemPrice, actualItemPrice, "Item price of a vending machine item does not match the expected price");
            }
        }

        [TestMethod]
        public void Creating_new_Vending_Machine_creates_empty_sales_list()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            IDataIO dataIO = new DatabaseIO();
            VendingMachine testVM = new VendingMachine(userIO, dataIO);

            Item[] testItems = GetTestItems();
            Dictionary<string, int> expectedSalesList = new Dictionary<string, int>()
            {
                { testItems[0].Name, 0 },
                { testItems[1].Name, 0 },
                { testItems[2].Name, 0 },
                { testItems[3].Name, 0 },
                { testItems[4].Name, 0 },
                { testItems[5].Name, 0 },
                { testItems[6].Name, 0 },
                { testItems[7].Name, 0 },
                { testItems[8].Name, 0 },
                { testItems[9].Name, 0 },
                { testItems[10].Name, 0 },
                { testItems[11].Name, 0 },
                { testItems[12].Name, 0 },
                { testItems[13].Name, 0 },
                { testItems[14].Name, 0 },
                { testItems[15].Name, 0 }
            };

            //Act
            Dictionary<string, int> actualSalesList = testVM.salesList;

            //Assert
            foreach (KeyValuePair<string, int> element in expectedSalesList)
            {
                string expectedItemName = element.Key;
                int expectedItemQuantity = 0;
                int actualItemQuantity;

                try
                {
                    actualItemQuantity = actualSalesList[element.Key];

                    Assert.AreEqual(expectedItemQuantity, actualItemQuantity, "Item quantity was not set to 0 when salesList was created");
                }
                catch (KeyNotFoundException)
                {
                    Assert.Fail("Item name from inventory was not found in salesList");
                }
            }
        }

        [TestMethod]
        public void FeedMoney_given_valid_input_increases_CurrentBalance()
        {
            //Arrange
            IUserIO userIOForVendingMachine = new FakeUserIO();
            IDataIO dataIO = new DatabaseIO();
            VendingMachine testVM = new VendingMachine(userIOForVendingMachine, dataIO);

            decimal expectedBalance;
            decimal actualBalance;

            for (int i = 1; i <= 1000; i++)
            {
                testVM.CurrentBalance = 0;
                expectedBalance = (decimal)i;
                IUserIO userIOForFeedMoney = new FakeUserIO(i.ToString());

                //Act                
                testVM.FeedMoney(userIOForFeedMoney, dataIO);
                actualBalance = testVM.CurrentBalance;

                //Assert
                Assert.AreEqual(expectedBalance, actualBalance, "FeedMoney did not correctly update CurrentBalance");
            }
        }

        [TestMethod]
        public void BuyItem_given_valid_slot_decreases_CurrentBalance()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            IDataIO dataIO = new DatabaseIO();
            VendingMachine testVM = new VendingMachine(userIO, dataIO);

            string slotIdSelected;
            int numberOfSlotsPerLetter = 4;
            decimal expectedBalance;
            decimal actualBalance;

            for (int i = 0; i < testVM.InventoryList.Count; i++)
            {
                slotIdSelected = "";
                testVM.CurrentBalance = 1000;

                string[] slotIdFirstLetters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                                             "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

                //The integer division in this line matches i = 0 through i = 3 with the letter "A",
                // and i = 4 through i = 7 with the letter "B", etc. 
                slotIdSelected = slotIdFirstLetters[(i / numberOfSlotsPerLetter)] + ((i % numberOfSlotsPerLetter) + 1).ToString();

                Slot selectedSlot = testVM.InventoryList[slotIdSelected];

                expectedBalance = testVM.CurrentBalance - testVM.InventoryList[slotIdSelected].Price;

                //Act             
                testVM.BuyItem(userIO, dataIO, selectedSlot);
                actualBalance = testVM.CurrentBalance;

                //Assert
                Assert.AreEqual(expectedBalance, actualBalance, "BuyItem did not correctly update CurrentBalance");
            }
        }

        [TestMethod]
        public void BuyItem_given_valid_slot_decreases_item_quantity()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            IDataIO dataIO = new DatabaseIO();
            VendingMachine testVM = new VendingMachine(userIO, dataIO);

            string slotIdSelected;
            int numberOfSlotsPerLetter = 4;
            int expectedQuanitityRemaining;
            int actualQuantityRemaining;

            for (int i = 0; i < testVM.InventoryList.Count; i++)
            {
                slotIdSelected = "";
                testVM.CurrentBalance = 1000;

                string[] slotIdFirstLetters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                                             "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

                //The integer division in this line matches i = 0 through i = 3 with the letter "A",
                // and i = 4 through i = 7 with the letter "B", etc. 
                slotIdSelected = slotIdFirstLetters[(i / numberOfSlotsPerLetter)] + ((i % numberOfSlotsPerLetter) + 1).ToString();

                Slot selectedSlot = testVM.InventoryList[slotIdSelected];
                expectedQuanitityRemaining = testVM.InventoryList[slotIdSelected].Quantity - 1;

                //Act             
                testVM.BuyItem(userIO, dataIO, selectedSlot);
                actualQuantityRemaining = testVM.InventoryList[slotIdSelected].Quantity;

                //Assert
                Assert.AreEqual(expectedQuanitityRemaining, actualQuantityRemaining, "BuyItem did not correctly reduce item quantity by 1");
            }
        }

        [TestMethod]
        public void FinishTransaction_sets_CurrentBalance_to_0()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            IDataIO dataIO = new DatabaseIO();
            VendingMachine testVM = new VendingMachine(userIO, dataIO);

            decimal expected = 0;

            //Act
            testVM.FinishTransaction(userIO, dataIO);
            decimal actual = testVM.CurrentBalance;

            //Assert
            Assert.AreEqual(expected, actual, "FinishTransaction did not set CurrentBalance to 0");
        }

        [TestMethod]
        public void ChangePassword_given_new_password_changes_admin_password()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            IDataIO dataIO = new DatabaseIO();
            VendingMachine testVM = new VendingMachine(userIO, dataIO);

            string newPassword = "newTestPassword";
            string expected = "newTestPassword";
            IUserIO userIONewPassword = new FakeUserIO(newPassword);

            //Act
            testVM.ChangePassword(userIONewPassword);
            string actual = testVM.adminPassword;

            //Assert
            Assert.AreEqual(expected, actual, "ChangePassword does not update vending machine password");
        }

        [TestMethod]
        public void RestockVendingMachine_resets_all_item_quantities_to_5()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            IDataIO dataIO = new DatabaseIO();
            VendingMachine testVM = new VendingMachine(userIO, dataIO);
            List<int> expected = new List<int>();

            foreach (KeyValuePair<string, Slot> element in testVM.InventoryList)
            {
                element.Value.ItemStack.Clear();
                expected.Add(5);
            }

            //Act
            testVM.RestockVendingMachine(userIO, dataIO);
            List<int> actual = new List<int>();

            foreach (KeyValuePair<string, Slot> element in testVM.InventoryList)
            {
                actual.Add(element.Value.Quantity);
            }

            //Assert
            CollectionAssert.AreEqual(expected, actual, "RestockVendingMachine does not reset item quantities to 5");
        }
    }
}
