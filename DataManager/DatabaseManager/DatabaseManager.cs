﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TransferProtocolLibrary.DataManagement.Database
{
    public class DatabaseManager
    {
        private string connectionString;

        private string databaseName;

        public DatabaseManager(string connectionString, string databaseName)
        {
            this.connectionString = connectionString;
            this.databaseName = databaseName;
        }

        public List<List<KeyValuePair<string, object>>> GetTableValues(string table)
        {
            List<List<KeyValuePair<string, object>>> tableRows = new List<List<KeyValuePair<string, object>>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String query = $"USE {databaseName}; SELECT * FROM {table};";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<KeyValuePair<string, object>> rowValues = new List<KeyValuePair<string, object>>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                KeyValuePair<string, object> valuePair = new KeyValuePair<string, object>(reader.GetName(i), reader.GetValue(i));

                                rowValues.Add(valuePair);
                            }

                            tableRows.Add(rowValues);
                        }
                    }
                }

            }

            return tableRows;
        }

        public void InsertValue(string table, string[] valueNames, string[] values)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"USE {databaseName}; INSERT INTO {table} ({string.Join(", " ,valueNames)}) VALUES ({"\'" + string.Join("\', \'", values) + "\'"});";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteValue(string table, string valueName, string value)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"USE {databaseName}; DELETE FROM {table} WHERE {valueName}='{value}';";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        public void CreateTable(string tableName, string[] valueNames)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"USE {databaseName}; CREATE TABLE {tableName} ({string.Join(", ", valueNames)});";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        public List<string> GetAllTablesNames()
        {
            List<string> tableNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                DataTable table = connection.GetSchema("Tables");

                foreach (DataRow row in table.Rows)
                {
                    string tablename = $"{row[1]}.{row[2]}";
                    tableNames.Add(tablename);
                }
            }

            return tableNames;
        }
    }
}