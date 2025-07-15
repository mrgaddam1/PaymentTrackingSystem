using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Core.Helpers
{
    public static class DataHelper
    {
        public static DataTable GetData(DbConnection conn, string storedProcName, SqlParameter[] values)
        {
            var dt = new DataTable();
            var connectionState = conn.State;
            try
            {
                if (connectionState != ConnectionState.Open) conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (values != null)
                        cmd.Parameters.AddRange(values);
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            finally
            {
                if (connectionState != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        public static void ExecuteSql(DbConnection conn, string command)
        {
            var connectionState = conn.State;

            if (connectionState != ConnectionState.Open) conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }

        public static DataSet GetDataSet(DbConnection conn, string storedProcName, SqlParameter[] values)
        {
            DataSet dataset = new DataSet();
            using (conn)
            {
                SqlDataAdapter adapt = new SqlDataAdapter();
                adapt.SelectCommand = new SqlCommand(storedProcName, (SqlConnection)conn);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;

                if (values != null)
                    adapt.SelectCommand.Parameters.AddRange(values);

                if (conn.State != ConnectionState.Open) conn.Open();

                using (adapt)
                {
                    try
                    {
                        adapt.Fill(dataset);
                    }
                    catch (SqlException e)
                    {
                    }
                    finally
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                    }
                }
            }
            conn.Close();
            return dataset;
        }


        public static void ExecuteSqlWithParams(DbConnection conn, string storedProcName, SqlParameter[] values)
        {
            var connectionState = conn.State;

            if (connectionState != ConnectionState.Open) conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                foreach (var param in values)
                    cmd.Parameters.Add(param);

                cmd.CommandText = storedProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }



    }
}

