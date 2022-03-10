﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
       
        public Transfer GetTransferById(int id)
        {
            Transfer returnTransfer = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfer WHERE transfer_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        returnTransfer = GetTransferFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnTransfer;
        }
        public bool SendBucks(Transfer transfer)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO transfer(account_from, account_to, amount) VALUES(@sendFrom, @sendTo, @amount)", conn);
                cmd.Parameters.AddWithValue("@sendFrom", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@sendTo", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                int count = cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE account SET balance = balance - @amount WHERE account_id = @sendFrom", conn);
                cmd.Parameters.AddWithValue("@sendFrom", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@sendTo", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
               count += cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE account SET balance = balance + @amount WHERE account_id = @sendTo", conn);
                cmd.Parameters.AddWithValue("@sendFrom", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@sendTo", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
               count += cmd.ExecuteNonQuery();
                if(count == 3)
                {
                    result = true;
                }

            }
            return result;
        }

       
        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer t = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                Amount = Convert.ToDecimal(reader["amount"]),
                TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]),
                TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]),

            };

            return t;
        }
    }
}