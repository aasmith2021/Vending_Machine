using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace VendingMachineApplication
{
    public class DatabaseIO : IDataIO
    {
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=VendingMachine;Trusted_Connection=True;";
        
        //This class creates an object that implements the IDataIO interface to read data from or
        //write data to a file in the current directory
        public List<string[]> GetInventoryData(IUserIO userIO)
        {
            List<string[]> inventoryFromDatabase = new List<string[]>();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    string sqlSelectAllInventory = "SELECT slot_number, item_name, item_price, item_category FROM inventory;";
                    SqlCommand sqlCmd = new SqlCommand(sqlSelectAllInventory, sqlCon);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string[] newItemData = new string[4];
                        newItemData[0] = Convert.ToString(reader["slot_number"]);
                        newItemData[1] = Convert.ToString(reader["item_name"]);
                        newItemData[2] = Convert.ToString(reader["item_price"]);
                        newItemData[3] = Convert.ToString(reader["item_category"]);

                        inventoryFromDatabase.Add(newItemData);
                    }

                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "ERROR: Unable to access database. Please contact a Vendo-Matic service expert for assistance.";
                userIO.DisplayData(errorMessage);
            }

            return inventoryFromDatabase;
        }

        public void WriteOutputToSource(IUserIO userIO, List<string[]> outputToWrite, int destinationNumber)
        {
            //For this Vending Machine application, the following destination numbers correspond to
            //the labeled output tables in the database:
            // 0 - transaction_log
            // 1 - sales_report

            string sqlWriteStatement;

            if (destinationNumber == 0)
            {
                sqlWriteStatement = "INSERT INTO transaction_log (transaction_date_time, transaction_name, balance_before, balance_after) " +
                                    "VALUES(@transaction_date_time, @transaction_name, @balance_before, @balance_after);";
            }
            else
            {
                sqlWriteStatement = "INSERT INTO sales_report(report_date_time, item_id, quantity_sold, total_sales_for_report) " +
                                    "VALUES(@report_date_time, (SELECT id FROM inventory WHERE item_name = @item_name), @quantity_sold, @total_sales_for_report);";
            }

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand sqlCmd = new SqlCommand(sqlWriteStatement, sqlCon);

                    //Writing log entry to database
                    if (destinationNumber == 0)
                    {
                        //Creating variables and parameters for a log entry
                        string[] logEntryDataToWrite = outputToWrite[0];
                        DateTime transactionDateTime = Convert.ToDateTime(logEntryDataToWrite[0]);
                        string transactionName = logEntryDataToWrite[1];
                        decimal balanceBefore = Convert.ToDecimal(logEntryDataToWrite[2].Substring(1));
                        decimal balanceAfter = Convert.ToDecimal(logEntryDataToWrite[3].Substring(1));

                        sqlCmd.Parameters.AddWithValue("@transaction_date_time", transactionDateTime);
                        sqlCmd.Parameters.AddWithValue("@transaction_name", transactionName);
                        sqlCmd.Parameters.AddWithValue("@balance_before", balanceBefore);
                        sqlCmd.Parameters.AddWithValue("@balance_after", balanceAfter);

                        sqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //These data values are the same for every row of a sales report, so they
                        //are not included in the loop below
                        DateTime reportDateTime = DateTime.Now;
                        decimal totalSalesForReport = Convert.ToDecimal(outputToWrite[outputToWrite.Count - 1][1].Substring(1));

                        //Writing sales report to database
                        for (int i = 0; i < outputToWrite.Count - 1; i++)
                        {
                            string itemName = outputToWrite[i][0];
                            decimal quantitySold = Convert.ToInt32(outputToWrite[i][1]);
                            
                            sqlCmd.Parameters.AddWithValue("@report_date_time", reportDateTime);
                            sqlCmd.Parameters.AddWithValue("@item_name", itemName);
                            sqlCmd.Parameters.AddWithValue("@quantity_sold", quantitySold);
                            sqlCmd.Parameters.AddWithValue("@total_sales_for_report", totalSalesForReport);

                            sqlCmd.ExecuteNonQuery();

                            sqlCmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "ERROR: Unable to access database. Please contact a Vendo-Matic service expert for assistance.";
                userIO.DisplayData(errorMessage);
            }
        }
    }
}
