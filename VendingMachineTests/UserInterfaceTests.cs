using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachineApplication;

namespace VendingMachineTests
{
    [TestClass]
    public class UserInterfaceTests
    {
        [TestMethod]
        public void RunMainMenu_given_valid_menu_option_returns_menu_option()
        {
            //Arrange
            string[] mainMenuValidOptions = new string[] { "1", "2", "3", "4" };
            string expected;
            string actual;

            //Act & Assert
            for (int i = 0; i < mainMenuValidOptions.Length; i++)
            {
                expected = mainMenuValidOptions[i];
                IUserIO userIORunMainMenuTest = new FakeUserIO(mainMenuValidOptions[i]);

                actual = UserInterface.RunMainMenu(userIORunMainMenuTest);

                Assert.AreEqual(expected, actual, "RunMainMenu did not return the option selected");
            }
        }

        [TestMethod]
        public void RunPurchaseMenu_given_valid_menu_option_returns_menu_option()
        {
            //Arrange
            string[] purchaseMenuValidOptions = new string[] { "1", "2", "3" };
            string expected;
            string actual;

            //Act & Assert
            for (int i = 0; i < purchaseMenuValidOptions.Length; i++)
            {
                expected = purchaseMenuValidOptions[i];
                IUserIO userIORunPurchaseMenuTest = new FakeUserIO(purchaseMenuValidOptions[i]);

                actual = UserInterface.RunMainMenu(userIORunPurchaseMenuTest);

                Assert.AreEqual(expected, actual, "RunPurchaseMenu did not return the option selected");
            }
        }


        [TestMethod]
        public void RunFeedMoneyMenu_given_current_balance_of_1000_returns_0()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO();
            decimal currentBalance = 1000m;
            int expected = 0;

            //Act
            int actual = UserInterface.RunFeedMoneyMenu(userIO, currentBalance);

            //Assert
            Assert.AreEqual(expected, actual, "RunFeedMoneyMenu returned an amount other than 0 when current balance was 1000");
        }

        [TestMethod]
        public void RunFeedMoneyMenu_given_current_balance_of_0_and_valid_user_input_of_1000_or_less_returns_correct_amount()
        {
            //Arrange
            decimal currentBalance = 0m;
            int expected = 0;
            int actual = 0;

            //Act
            for (int i = 1; i <= 1000; i++)
            {
                IUserIO userIO = new FakeUserIO(i.ToString());
                expected = i;
                actual = UserInterface.RunFeedMoneyMenu(userIO, currentBalance);
            }

            //Assert
            Assert.AreEqual(expected, actual, "RunFeedMoneyMenu did not return correct amount to add to current balance");
        }

        [TestMethod]
        public void RunFeedMoneyMenu_given_a_positive_current_balance_and_valid_user_input_returns_correct_amount()
        {
            //Arrange
            decimal currentBalance = 0m;
            int expected = 0;
            int actual = 0;

            //Act
            for (int i = 0; i <= 1000; i++)
            {
                currentBalance = (decimal)i;

                for (int j = 1; j <= 1000 - i; j++)
                {
                    IUserIO userIO = new FakeUserIO(j.ToString());
                    expected = j;
                    actual = UserInterface.RunFeedMoneyMenu(userIO, currentBalance);
                }
            }

            //Assert
            Assert.AreEqual(expected, actual, "RunFeedMoneyMenu did not return correct amount to add to current balance");
        }

        [TestMethod]
        public void GetUserMoney_given_exit_returns_EXIT()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO("exit");
            decimal currentBalance = 0m;
            string expected = "EXIT";

            //Act
            string actual = UserInterface.GetUserMoney(userIO, currentBalance);

            //Assert
            Assert.AreEqual(expected, actual, "GetUserMoney given \"exit\" did not return \"EXIT\"");
        }

        [TestMethod]
        public void GetUserMoney_given_whole_dollar_amount_that_would_make_current_balance_less_than_or_equal_to_1000_returns_dollar_amount()
        {
            //Arrange
            decimal currentBalance = 0m;
            string expected = "";
            string actual = "";

            //Act
            for (int i = 0; i <= 1000; i++)
            {
                currentBalance = (decimal)i;

                for (int j = 1; j <= 1000 - i; j++)
                {
                    IUserIO userIO = new FakeUserIO(j.ToString());
                    expected = j.ToString();
                    actual = UserInterface.GetUserMoney(userIO, currentBalance);
                }
            }

            //Assert
            Assert.AreEqual(expected, actual, "GetUserMoney did not return correct amount");
        }

        [TestMethod]
        public void RunSelectProductMenu_given_exit_returns_EXIT()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO("exit");
            SortedList<string, Slot> testInventoryList = new SortedList<string, Slot>();
            decimal currentBalance = 0m;
            string expected = "EXIT";

            //Act
            string actual = UserInterface.RunSelectProductMenu(userIO, testInventoryList, currentBalance);

            //Assert
            Assert.AreEqual(expected, actual, "RunSelectProductMenu given \"exit\" did not return \"EXIT\"");
        }

        [TestMethod]
        public void RunSelectProductMenu_given_in_stock_slot_id_returns_slot_id()
        {
            //Arrange
            IUserIO userIOVendingMachine = new FakeUserIO();
            IDataIO dataIO = new FileIO();
            VendingMachine testVM = new VendingMachine(userIOVendingMachine, dataIO);
            decimal currentBalance = 1000m;
            int numberOfSlotsPerLetter = 4;
            string slotIdSelected = "";
            string expected = "";
            string actual = "";

            //Act
            for (int i = 0; i < testVM.InventoryList.Count; i++)
            {
                string[] slotIdFirstLetters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                                             "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

                //The integer division in this line matches i = 0 through i = 3 with the letter "A",
                // and i = 4 through i = 7 with the letter "B", etc. 
                slotIdSelected = slotIdFirstLetters[(i / numberOfSlotsPerLetter)] + ((i % numberOfSlotsPerLetter) + 1).ToString();

                IUserIO userIOSlotSelected = new FakeUserIO(slotIdSelected);

                expected = slotIdSelected;

                actual = UserInterface.RunSelectProductMenu(userIOSlotSelected, testVM.InventoryList, currentBalance);

                //Assert
                Assert.AreEqual(expected, actual, "RunSelectProductMenu given a valid slot ID did not return that slot ID");
            }
        }

        [TestMethod]
        public void GetOptionChoicesForMenu_given_menu_options_as_strings_returns_a_list_of_number_values_as_strings()
        {
            //Arrange
            string[] testMenuOptions = new string[] { "Option 1", "Option 2", "Option 3"};
            List<string> expected = new List<string>() { "1", "2", "3" };

            //Act
            List<string> actual = UserInterface.GetOptionChoicesForMenu(testMenuOptions);

            //Assert
            CollectionAssert.AreEqual(expected, actual, "Menu option numbers that were returned did not match what was expected");
        }

        [TestMethod]
        public void GetOptionChoicesForMenu_given_no_menu_options_returns_an_empty_list()
        {
            //Arrange
            string[] testMenuOptions = new string[] { };
            List<string> expected = new List<string>() { };

            //Act
            List<string> actual = UserInterface.GetOptionChoicesForMenu(testMenuOptions);

            //Assert
            CollectionAssert.AreEqual(expected, actual, "OptionChoicesForMenu given no menu options did not return expected output");
        }

        [TestMethod]
        public void GetUserOption_given_valid_option_returns_the_option_selected()
        {
            //Arrange
            List<string> testOptions = new List<string>() { "1", "2", "3", "4", "5" };
            string expected = "";
            string actual = "";

            for (int i = 0; i < testOptions.Count; i++)
            {
                expected = testOptions[i];
                IUserIO userIO = new FakeUserIO(testOptions[i]);

                //Act
                actual = UserInterface.GetUserOption(userIO, testOptions);

                //Assert
                Assert.AreEqual(expected, actual, "GetUserOption given valid option does not return the option selected");
            }
        }

        [TestMethod]
        public void AdminLogin_given_correct_password_returns_true()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO("admin");
            bool expected = true;

            //Act
            bool actual = UserInterface.AdminLogin(userIO, "admin");

            //Assert
            Assert.AreEqual(expected, actual, "AdminLogin given correct password did not return true");
        }

        [TestMethod]
        public void AdminLogin_given_incorrect_password_returns_false()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO("admin99");
            bool expected = false;

            //Act
            bool actual = UserInterface.AdminLogin(userIO, "admin");

            //Assert
            Assert.AreEqual(expected, actual, "AdminLogin given incorrect password did not return false");
        }

        [TestMethod]
        public void RunAdminMenu_given_valid_menu_option_returns_menu_option()
        {
            //Arrange
            string[] adminMenuValidOptions = new string[] { "1", "2", "3", "4" };
            string expected;
            string actual;

            //Act & Assert
            for (int i = 0; i < adminMenuValidOptions.Length; i++)
            {
                expected = adminMenuValidOptions[i];
                IUserIO userIORunAdminMenuTest = new FakeUserIO(adminMenuValidOptions[i]);

                actual = UserInterface.RunAdminMenu(userIORunAdminMenuTest);

                Assert.AreEqual(expected, actual, "RunAdminMenu did not return the option selected");
            }
        }

        [TestMethod]
        public void ChangePasswordMenu_given_new_password_string_returns_the_string()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO("hello");
            string expected = "hello";

            //Act
            string actual = UserInterface.ChangePasswordMenu(userIO);

            //Assert
            Assert.AreEqual(expected, actual, "ChangePasswordMenu given new password did not return new password");
        }
    }
}
