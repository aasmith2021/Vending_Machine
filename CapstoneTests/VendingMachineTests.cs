using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineTests
    {
        private Item[] GetTestItems()
        {
            Item[] testItems = new Item[]
            {
                new Item("Potato Crisps", 3.05m, "Chip"),
                new Item("Stackers", 1.45m, "Chip"),
                new Item("Grain Waves", 2.75m, "Chip"),
                new Item("Cloud Popcorn", 3.65m, "Chip"),
                new Item("Moonpie", 1.80m, "Candy"),
                new Item("Cowtales", 1.50m, "Candy"),
                new Item("Wonka Bar", 1.50m, "Candy"),
                new Item("Crunchie", 1.75m, "Candy"),
                new Item("Cola", 1.25m, "Drink"),
                new Item("Dr. Salt", 1.50m, "Drink"),
                new Item("Mountain Melter", 1.50m, "Drink"),
                new Item("Heavy", 1.50m, "Drink"),
                new Item("U-Chews", .85m, "Gum"),
                new Item("Little League Chew", 0.95m, "Gum"),
                new Item("Chiclets", 0.75m, "Gum"),
                new Item("Triplemint", 0.75m, "Gum"),
            };

            return testItems;
        }

        private Slot[] GetTestSlots(Item[] testItems)
        {
            Slot[] testSlots = new Slot[]
            {
                new Slot("A1", testItems[0], 5),
                new Slot("A2", testItems[1], 5),
                new Slot("A3", testItems[2], 5),
                new Slot("A4", testItems[3], 5),
                new Slot("B1", testItems[4], 5),
                new Slot("B2", testItems[5], 5),
                new Slot("B3", testItems[6], 5),
                new Slot("B4", testItems[7], 5),
                new Slot("C1", testItems[8], 5),
                new Slot("C2", testItems[9], 5),
                new Slot("C3", testItems[10], 5),
                new Slot("C4", testItems[11], 5),
                new Slot("D1", testItems[12], 5),
                new Slot("D2", testItems[13], 5),
                new Slot("D3", testItems[14], 5),
                new Slot("D4", testItems[15], 5)
            };

            return testSlots;
        }

        [TestMethod]
        public void Creating_a_new_Vending_Machine_correctly_populates_vending_machine_with_items_from_Inventory_file()
        {
            //Arrange
            VendingMachine testVM = new VendingMachine(false);

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
                decimal expectedItemPrice = expectedInventoryList.Values[i].ItemInSlot.Price;

                string actualItemName = actualInventoryList.Values[i].ItemInSlot.Name;
                string actualItemCategory = actualInventoryList.Values[i].ItemInSlot.Category;
                decimal actualItemPrice = actualInventoryList.Values[i].ItemInSlot.Price;

                Assert.AreEqual(expectedItemName, actualItemName, "Item name of a vending machine item does not match the expected name");
                Assert.AreEqual(expectedItemCategory, actualItemCategory, "Item category of a vending machine item does not match the expected name");
                Assert.AreEqual(expectedItemPrice, actualItemPrice, "Item price of a vending machine item does not match the expected name");
            }
        }
    }
}
