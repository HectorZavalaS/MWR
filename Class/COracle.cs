using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace MWR.Class
{
    public class COracle
    {
        String m_server;
        String m_SID;
        private String m_user;
        private String m_pass;
        OracleConnection m_OracleDB;
        public string Server { get => m_server; set => m_server = value; }
        public string SID { get => m_SID; set => m_SID = value; }

        public COracle() {
            m_server = "192.168.0.25";
            m_SID = "SEMPROD";
            m_user = "APPS";
            m_pass = "apps";
            m_OracleDB = GetDBConnection(Server, 0, SID, m_user, m_pass);
            
            try {
                m_OracleDB.Open();
            } catch (Exception ex)  {
                Console.WriteLine(ex);
            }
        }

        public COracle(String serv, String Sid)  {
            m_server = serv;
            m_SID = Sid;
            m_user = "APPS";
            m_pass = "apps";
            m_OracleDB = GetDBConnection(Server, 0, SID, m_user, m_pass);
            m_OracleDB.Open();
        }

        private OracleConnection GetDBConnection(string host, int port, String sid, String user, String password)  {

            Console.WriteLine("Getting Connection ...");

            // 'Connection string' to connect directly to Oracle.
            string connString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                 + Server + ")(PORT = " + "1521" + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                 + SID + ")));Password=" + m_pass + ";User ID=" + m_user + ";Enlist=false;Pooling=true";

            OracleConnection conn = new OracleConnection();

            try  {
                conn.ConnectionString = connString;
            }  catch (Exception ex)  {
                conn = null;
            }

            return conn;
        }

        /// <summary>
        /// This method is responsible to get the first TRANSACTION_ID in a given time lap. To do it,
        /// it needs two params, date1 and date2.
        /// </summary>
        /// <param name="date1">This param should be shaped by the first day, a month and a year. For example, 01-Jun-2023</param>
        /// <param name="date2">This param should be shaped by the second day, a month and a year. For example, 02-Jun-2023</param>
        /// <returns>an int value which indicates if there was an error.</returns>
        public int getID(String date1, String date2, ref int d1)  {
            int result = 0;
            String sql = "SELECT TRANSACTION_ID FROM INV.MTL_MATERIAL_TRANSACTIONS " +
                "WHERE CREATION_DATE >= '" + date1 + "' AND CREATION_DATE < '" + date2 + "' " +
                "AND ROWNUM = 1 ";

            try  {
                OracleCommand cmd = new OracleCommand();

                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;

                using (DbDataReader reader = cmd.ExecuteReader())  {
                    if (reader.HasRows)  {
                        while (reader.Read())
                            d1 = int.Parse(reader.GetValue(0).ToString());

                        result = 1;
                    }  else  {
                        result = 0;
                    }
                        
                }
            }  catch (Exception ex)  {
                result = -1;
            }

            return result;
        }


        /// <summary>
        /// This method 
        /// </summary>
        /// <param name="date1">First day of the last month, for example, "01-Jun-2023"</param>
        /// <param name="date2">First day of the current month, for example, "01-Jul-2023"</param>
        /// <param name="dr1"></param>
        /// <param name="dr2"></param>
        /// <param name="dr3"></param>
        /// <param name="dr4"></param>
        /// <returns>
        /// Returns a -1 if there was an error.
        /// Returns a 0 if there is not a value.
        /// Resturn a 1 if there is a value.
        /// </returns>
        public int getStInData(String date1, String date2, ref int dr1, ref int dr2, ref int dr3, ref int dr4)  {
            int result = 0;
            List<int> data = new List<int>();

            String sql = "SELECT 'PCK(TRAY)' AS DESCRIPTION,COUNT(*) as QTY " +
                            "FROM SIIXSEM.INCOMING_LOT_DETAILS " +
                            "WHERE CREATED_DT >= '" + date1 + "' AND RECEIPT_NUM<> 'OPEN-DO-001' AND CREATED_DT < '"+ date2 +"' AND ITEM_NAME LIKE 'PCK%' " +
                         "UNION " +
                         "SELECT 'CHINA' AS DESCRIPTION, COUNT(*) as QTY " +
                            "FROM SIIXSEM.INCOMING_LOT_DETAILS " +
                            "WHERE CREATED_DT >= '"+ date1 + "' AND RECEIPT_NUM<> 'OPEN-DO-001' AND CREATED_DT < '"+ date2 +"' " +
                            "AND ITEM_NAME IN(SELECT ITEM_CD FROM SIIXSEM.M_ITEM_SPECIFICATIONS WHERE TRADE_FG_FLAG = 'Y') " +
                         "UNION " +
                         "SELECT 'RECEIVING' AS DESCRIPTION, COUNT(*) as QTY " +
                            "FROM SIIXSEM.INCOMING_LOT_DETAILS " +
                            "WHERE CREATED_DT >= '"+ date1 +"' AND RECEIPT_NUM<> 'OPEN-DO-001' AND CREATED_DT < '"+ date2 +"' " +
                            "AND ITEM_NAME NOT LIKE 'PCK%' AND ITEM_NAME NOT IN(SELECT ITEM_CD FROM SIIXSEM.M_ITEM_SPECIFICATIONS WHERE TRADE_FG_FLAG = 'Y') " +
                         "UNION " +
                         "SELECT 'TOTAL' AS DESCRIPTION, COUNT(*) as QTY " +
                            "FROM SIIXSEM.INCOMING_LOT_DETAILS " +
                            "WHERE CREATED_DT >= '"+ date1 +"' AND RECEIPT_NUM<> 'OPEN-DO-001' AND CREATED_DT < '"+ date2 +"' ";

            try  {
                OracleCommand cmd = new OracleCommand();

                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;

                using (DbDataReader reader = cmd.ExecuteReader())  {
                    if (reader.HasRows)  {

                        while (reader.Read())
                            data.Add(int.Parse(reader.GetValue(1).ToString()));

                        dr1 = data[0];
                        dr2 = data[1];
                        dr3 = data[2];
                        dr4 = data[3];

                        result = 1;

                    }  else  {
                        result = 0;
                    }
                }
            }  catch (Exception ex)  {
                result = -1;
            }

            return result;
        }

        
        /// <summary>
        /// PICKING BOTH SMT and ASSY
        /// </summary>
        /// <param name="date1">First day of the last month, for example, "01-Jun-2023"</param>
        /// <param name="date2">First day of the current month, for example, "01-Jul-2023"</param>
        /// <param name="subInvCode">Sub-Inventory Code, which could be "ASSY" or "SMT1"</param>
        /// <param name="d1">Variable used to save the value obtained from Oracle</param>
        /// <returns>
        /// Returns a -1 if there was an error.
        /// Returns a 0 if there is not a value.
        /// Resturn a 1 if there is a value.
        /// </returns>
        public int getPicking(String date1, String date2, String subInvCode, ref int d1)  {
            int result = 0;

            string sql = "SELECT '"+subInvCode+" Picking' AS DESCRIPTION,COUNT ( * ) AS QTY FROM SIIXSEM.DJ_MASTER_PICK_LIST " +
                         "WHERE CREATED_DT >= '"+date1+"' AND CREATED_DT < '"+ date2 +"' AND PICKED_FLAG = 'Y' " +
                         "AND SUPPLY_SUBINV = '"+ subInvCode +"' ";

            try  {
                OracleCommand cmd = new OracleCommand();

                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;

                using (DbDataReader reader = cmd.ExecuteReader())  {
                    if (reader.HasRows)  {
                        while (reader.Read())
                            d1 = int.Parse(reader.GetValue(1).ToString());

                        result = 1;
                    } else  {
                        result = 0;
                    }
                }
            }  catch (Exception ex) {
                result = -1;
            }

            return result;
        }


        /// <summary>
        /// Free Pick and SubInventory method to extract information from Oracle.
        /// </summary>
        /// <param name="firstID">First ID in the month</param>
        /// <param name="lastID">Last ID in the month</param>
        /// <param name="subInvCode">Sub-Inventory Code, which could be "ASSY" or "SMT1"</param>
        /// <param name="d1">Get an int value which represents the number of transactions</param>
        /// <returns>
        /// Returns a -1 if there was an error.
        /// Returns a 0 if there is not a value.
        /// Resturn a 1 if there is a value.
        /// </returns>
        public int FPandSI(int firstID, int lastID, String subInvCode, ref int d1)  {
            int result = 0;

            string sql = "SELECT 'Free Pick and SubInventory "+ subInvCode +"' AS DESCRIPTION, COUNT(*) AS QTY " +
                         "FROM INV.MTL_MATERIAL_TRANSACTIONS " +
                         "WHERE (TRANSACTION_ID >= " + firstID +"  AND TRANSACTION_ID < "+ lastID +") AND SUBINVENTORY_CODE = '"+ subInvCode +
                         "' AND (SOURCE_CODE = 'SUBINVENTORY TRANSFER' OR SOURCE_CODE LIKE 'FREE PICK%') ";

            try  {
                OracleCommand cmd = new OracleCommand();

                // Set connection for command.
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;

                using (DbDataReader reader = cmd.ExecuteReader()){
                    if (reader.HasRows)  {
                        while (reader.Read())
                            d1 = int.Parse(reader.GetValue(1).ToString());
                            

                        result = 1;
                    } else  {
                        result = 0;
                    }
                        
                }
            }  catch (Exception ex)  {
                result = -1;
            }

            return result;
        }

        /// <summary>
        /// Tray CONS Transactions method.
        /// </summary>
        /// <param name="transID1">First ID in the month</param>
        /// <param name="transID2">Last ID in the month</param>
        /// <param name="d1">Get an int value which represents the number of transactions</param>
        /// <returns>
        /// Returns a -1 if there was an error.
        /// Returns a 0 if there is not a value.
        /// Resturn a 1 if there is a value.
        /// </returns>
        public int TConsT(int transID1, int transID2, ref int d1) {

            int result = 0;
            String sql = "SELECT COUNT ( * ) FROM INV.MTL_MATERIAL_TRANSACTIONS " +
                         "WHERE TRANSACTION_ID >= "+ transID1 +" AND TRANSACTION_ID < "+ transID2 +
                         " AND SUBINVENTORY_CODE = 'CONS' AND TRANSFER_SUBINVENTORY = 'IWH' " +
                         "AND SOURCE_CODE = 'SUBINVENTORY TRANSFER'";

            try {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;

                using(DbDataReader reader = cmd.ExecuteReader()) {
                    if (reader.HasRows) {
                        while (reader.Read())
                            d1 = int.Parse(reader.GetValue(0).ToString());

                        result = 1;
                    } else  {
                        result = 0;
                    }
                }
            } catch (Exception ex) {
                result = -1;
            }

            return result;
        }


        //SMT -> WH
        //No hay instrucción SQL para obtener este dato.
        public int smtTOwh() {

            return 1;
        }


        /// <summary>
        /// OWG-IN method.
        /// </summary>
        /// <param name="date1">First day of the last month, for example, "01-Jun-2023"</param>
        /// <param name="date2">First day of the current month, for example, "01-Jul-2023"</param>
        /// <param name="d1">Get an int value which represents the number of transactions</param>
        /// <returns>
        /// Returns a -1 if there was an error.
        /// Returns a 0 if there is not a value.
        /// Resturn a 1 if there is a value.
        /// </returns>
        public int Owh_In(String date1, String date2, ref int d1) {
            int result;
            String sql = "SELECT COUNT ( * ) FROM SIIXSEM.PACKING_HDR " +
                         "WHERE CREATED_DT >= '"+date1+ "' AND CREATED_DT < '" + date2 + "'";

            try  {
                OracleCommand cmd = new OracleCommand();

                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;

                using (DbDataReader reader = cmd.ExecuteReader())  {
                    if (reader.HasRows){
                        while (reader.Read())
                            d1 = int.Parse(reader.GetValue(0).ToString());

                        result = 1;
                    } else  {
                        result = 0;
                    }
                        
                }

            } catch(Exception ex)  {
                result = -1;
            }

            return result;
        }


        /// <summary>
        /// OWH-Out method
        /// </summary>
        /// <param name="date1">First day of the last month, for example, "01-Jun-2023"</param>
        /// <param name="date2">First day of the current month, for example, "01-Jul-2023"</param>
        /// <param name="d1">Get an int value which represents the number of transactions</param>
        /// <returns>
        /// Returns a -1 if there was an error.
        /// Returns a 0 if there is not a value.
        /// Resturn a 1 if there is a value.
        /// </returns>
        public int Owh_Out(String date1, String date2, ref int d1)  {
            int result = 0;
            String sql = "SELECT COUNT ( * ) FROM SIIXSEM.SHIPPING_PALLET_DTL " +
                         "WHERE CREATED_DT >= '"+date1+ "' AND CREATED_DT < '" + date2 + "'";

            try  {
                OracleCommand cmd = new OracleCommand();

                cmd.Connection = m_OracleDB;
                cmd.CommandText = sql;

                using (DbDataReader reader = cmd.ExecuteReader())  {
                    if (reader.HasRows)  {
                        while (reader.Read())
                            d1 = int.Parse(reader.GetValue(0).ToString());

                        result = 1;
                    }  else  {
                        result = 0;
                    }
                }
            }  catch (Exception ex)  {
                result = -1;
            }


            return result;
        }


        public void Close()
        {
            m_OracleDB.Dispose();
            m_OracleDB.Close();
            OracleConnection.ClearPool(m_OracleDB);
        }
    }
}