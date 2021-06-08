using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class VendUITests : VendUI
    {
        [TestMethod]
        public void GetUserMoney_given_valid_user_input_returns_the_option_selected()
        {
            //Arrange
            string[] testUserInputs = { "1", "7", "14", "28", "45", "2000" };
            int[] expected = { 1, 7, 14, 28, 45, 2000 };

            //Act
            int[] actual = new int[6];

            for (int i = 0; i < testUserInputs.Length; i++)
            {
                actual[i] = GetUserMoney(testUserInputs[i], new InputGetter());
            }

            //Assert
            CollectionAssert.AreEqual(expected, actual, "GetUserMoney did not return expected output from given input");
        }

        [TestMethod]
        public void GetOptionChoicesForMenu_given_5_menu_options_returns_1_through_5_as_option_choices()
        {
            //Arrange
            string[] testMenuOptions = { "Hello", "", "Menu", "Pizza", "Word" };
            List<string> expected = new List<string>() { "1", "2", "3", "4", "5" };

            //Act
            List<string> actual = GetOptionChoicesForMenu(testMenuOptions);

            //Assert
            CollectionAssert.AreEqual(expected, actual, "The output of GetOptionChoicesForMenu was incorrect for the given input");
        }

        [TestMethod]
        public void GetOptionChoicesForMenu_given_0_menu_options_returns_an_empty_List_of_strings_as_option_choices()
        {
            //Arrange
            string[] emptyTestMenuOptions = { };
            List<string> expected = new List<string>() { };

            //Act
            List<string> actual = GetOptionChoicesForMenu(emptyTestMenuOptions);

            //Assert
            CollectionAssert.AreEqual(expected, actual, "The output of GetOptionChoicesForMenu was incorrect for the given input");
        }

        [TestMethod]
        public void GetUserOption_given_valid_option_of_5_returns_5()
        {
            //Arrange
            List<string> testOptions = new List<string>() { "1", "2", "3", "4", "5", "6", "7" };
            string expected = "5";

            //Act
            string actual = GetUserOption(testOptions, new FakeUserInput5());

            //Assert
            Assert.AreEqual(expected, actual, "GetUserOption did not return the expected output");
        }

        [TestMethod]
        public void GetUserOption_given_valid_option_of_A4_returns_A4()
        {
            //Arrange
            List<string> testOptions = new List<string>() { "A1", "A2", "A3", "A4", "B1", "B2", "B3", "B4" };
            string expected = "A4";

            //Act
            string actual = GetUserOption(testOptions, new FakeUserInputA4());

            //Assert
            Assert.AreEqual(expected, actual, "GetUserOption did not return the expected output");
        }

        [TestMethod]
        public void GetUserMoney_given_valid_input_of_5_as_a_string_returns_5_as_an_int()
        {
            //Arrange
            string testUserInput = "5";
            int expected = 5;

            //Act
            int actual = GetUserMoney(testUserInput, new FakeUserInput5());

            //Assert
            Assert.AreEqual(expected, actual, "GetUserMoney did not return the expected output");
        }

        [TestMethod]
        public void GetUserMoney_given_invalid_input_of_Q_but_then_valid_input_of_5_returns_5_as_an_int()
        {
            //Arrange
            string testUserInput = "Q";
            int expected = 5;

            //Act
            int actual = GetUserMoney(testUserInput, new FakeUserInput5());

            //Assert
            Assert.AreEqual(expected, actual, "GetUserMoney did not return the expected output");
        }

        [TestMethod]
        public void AdminLogin_default_setting_given_default_admin_password_returns_true()
        {
            //Arrange
            bool expected = true;

            //Act
            bool actual = AdminLogin(new FakeUserInputAdmin());

            //Assert
            Assert.AreEqual(expected, actual, "AdminLogin did not allow user to login with default password");
        }

        [TestMethod]
        public void AdminLogin_default_setting_given_incorrect_password_returns_false()
        {
            //Arrange
            bool expected = false;

            //Act
            bool actual = AdminLogin(new FakeUserInputA4());

            //Assert
            Assert.AreEqual(expected, actual, "AdminLogin allows user to login by entering something other than the default password");
        }

        [TestMethod]
        public void AdminLogin_given_default_password_after_changing_password_returns_false()
        {
            //Arrange
            bool expected = false;

            //Act
            ChangePassword(new FakeUserInputA4());
            bool actual = AdminLogin(new FakeUserInputAdmin());

            //Assert
            Assert.AreEqual(expected, actual, "AdminLogin allowed user to login with default password after password was changed");
        }

        [TestMethod]
        public void ChangePassword_given_new_password_changes_vending_machine_password()
        {
            //Arrange
            string expected = "A4";

            //Act
            ChangePassword(new FakeUserInputA4());
            string actual = AdminPassword;

            //Assert
            Assert.AreEqual(expected, actual, "ChangePassword does not change password to the string provided by the user");
        }
    }
}
