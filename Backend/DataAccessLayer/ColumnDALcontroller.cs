using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class ColumnDALcontroller : DALcontroller
    {
        private const string ColumnTable = "Columns";
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ColumnDALcontroller() : base(ColumnTable) { }

        public bool DeleteAll()
        {
            return DeleteData();
        }


        public List<ColumnDTO> getAllColumns()
        {
            List<ColumnDTO> results = new List<ColumnDTO>();
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {ColumnTable};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        public bool addColumn(ColumnDTO column)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTable}({ColumnDTO.BoardIDColumnName},{ColumnDTO.ColumnNameColumnName},{ColumnDTO.ColumnLimitColumnName},{ColumnDTO.ColumnOrdinalColumnName})"
                        + $"VALUES(@id,@ColumnName,@ColumnLimit,@ordinal)";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"id", column.BrdID);
                    SQLiteParameter Pram2 = new SQLiteParameter(@"ColumnName", column.ColumnName);
                    SQLiteParameter Pram3 = new SQLiteParameter(@"ColumnLimit", column.ColumnLimit);
                    SQLiteParameter Pram4 = new SQLiteParameter(@"ordinal", column.ColumnOrdinal);
                    command.Parameters.Add(Pram1);
                    command.Parameters.Add(Pram2);
                    command.Parameters.Add(Pram3);
                    command.Parameters.Add(Pram4);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    log.Error("Error inserting to dataBase");
                    log.Debug(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > -1;
            }


        }
        /// <summary>
        /// Deleting a column 
        /// </summary>
        /// <param name="boardID"> the specific ID of a column we want to delete</param>
        /// <returns></returns>
        public bool DeleteBoardsColumns(int boardID)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {ColumnTable} WHERE BoardID = @id";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
                    command.Parameters.Add(Pram1);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Error deleting to dataBase");
                    log.Debug(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > -1;
            }


        }


        public int getColumnlimit(int brdid, int ordinal)
        {
            int limit = 0;
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {ColumnTable} WHERE BoardID = @brdid and ColumnOrdinal = @ord;";
                SQLiteDataReader dataReader = null;


                try
                {
                    connection.Open();
                    SQLiteParameter Pram1 = new SQLiteParameter(@"brdid", brdid);
                    SQLiteParameter Pram2 = new SQLiteParameter(@"ord", ordinal);
                    command.Parameters.Add(Pram1);
                    command.Parameters.Add(Pram2);
                    command.Prepare();


                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        try
                        {
                            limit = dataReader.GetInt32(2);
                        }
                        catch (Exception e)
                        {
                            limit = -1;
                        }

                    }

                }
                catch (Exception e)
                {
                    log.Debug(e.Message);
                    Console.WriteLine(e.Message);

                }

                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return limit;
        }


        public bool Update(int id, int Ordinal, string attributeName, int attributeValue)
        {

            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"UPDATE {ColumnTable} SET [{attributeName}] = @attributevalue WHERE BoardID = @boardId AND ColumnOrdinal = @ColOrdinal";
                    SQLiteParameter attribparm = new SQLiteParameter(@"attributevalue", attributeValue);
                    SQLiteParameter idParm = new SQLiteParameter(@"boardId", id);
                    SQLiteParameter ordParm = new SQLiteParameter(@"ColOrdinal", Ordinal);
                    command.Parameters.Add(attribparm);
                    command.Parameters.Add(idParm);
                    command.Parameters.Add(ordParm);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Error in updating dataBase");
                    log.Debug(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > -1;
            }
        }
        public ColumnDTO ConvertReaderToObject(SQLiteDataReader dataReader)
        {
            return new ColumnDTO(dataReader.GetInt32(0), dataReader.GetString(1),dataReader.GetInt32(2),dataReader.GetInt32(3));
        }
    }
}
