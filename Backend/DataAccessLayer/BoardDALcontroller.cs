using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDALcontroller : DALcontroller
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string BoardTable = "Boards";
        private const string UsersjoineesTable = "UsersJoinees";
        public BoardDALcontroller() : base(BoardTable) { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BoardDTO> getAllBoards()
        {
            List<BoardDTO> results = new List<BoardDTO>();
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {BoardTable};";
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
        /// <summary>
		/// an a sql query to insert a board to boards table
		/// </summary>
		/// <param name="board"> dto object to add its fields to the boards table</param>
		/// <returns></returns>
		public bool addBoard(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTable}({BoardDTO.BoardIDColumnName},{BoardDTO.BoardNameColumnName},{BoardDTO.userEmailColumnName},{BoardDTO.TaskIDColumnName})"
                        + $"VALUES(@id,@boardname,@usermail,@taskid)";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"id", board.BrdID);
                    SQLiteParameter Pram2 = new SQLiteParameter(@"boardname", board.BoardName);
                    SQLiteParameter Pram3 = new SQLiteParameter(@"usermail", board.UserEmail);
                    SQLiteParameter Pram4 = new SQLiteParameter(@"taskid", board.TaskID);
                    command.Parameters.Add(Pram1);
                    command.Parameters.Add(Pram2);
                    command.Parameters.Add(Pram3);
                    command.Parameters.Add(Pram4);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
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
        /// an a sql query to insert a new row to the joinees table
        /// </summary>
        /// <param name="email"> the user email</param>
        /// <param name="boardID"> the joined board id</param>>
        /// <returns></returns>
        public bool addJoinee(string email, int boardID)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UsersjoineesTable}(Email,BoardID)"
                        + $"VALUES(@email,@boardID)";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"email", email);
                    SQLiteParameter Pram2 = new SQLiteParameter(@"boardID", boardID);
                    command.Parameters.Add(Pram1);
                    command.Parameters.Add(Pram2);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
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
		/// an a sql query to get usersbrds id's
		/// </summary>
		/// <param name="email"> the user email</param>
		/// <returns></returns>
		public List<BoardDTO> Getusersboardslist(string email)
        {
            List<BoardDTO> results = new List<BoardDTO>();
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT Boards.BoardID,Boards.BoardName,Boards.UserEmail,Boards.TaskID from {BoardTable} join {UsersjoineesTable} on UsersJoinees.BoardID = Boards.BoardID and UsersJoinees.Email = @email";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    SQLiteParameter Param1 = new SQLiteParameter(@"email", email);
                    command.Parameters.Add(Param1);
                    command.Prepare();
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
        /// <summary>
        /// this methods deletes all board in the data base
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllBoards()
        {
            return DeleteData() & DeleteJoineesTable();
        }

        /// <summary>
        /// this method delete a spicific board from the data base
        /// </summary>
        /// <param name="boardid"></param>
        /// <returns></returns>
        public bool DeleteTheBoard(int boardid)
        {
            return DeleteBoard(boardid) & DeleteboardfromJoinees(boardid);
        }
        /// <summary>
		/// an a sql query to delete the joinees table
		/// </summary>
		/// <returns></returns>
        public bool DeleteJoineesTable()
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {UsersjoineesTable}";

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Error deleting from dataBase");
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
		/// an a sql query to delete 1 board
		/// </summary>
		/// <param name="boardID"> the board id wanted to be deleted</param>
		/// <returns></returns>
		public bool DeleteBoard(int boardID)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {BoardTable} WHERE BoardID = @id";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
                    command.Parameters.Add(Pram1);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Error deleting from dataBase");
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
		/// an a sql query to delete joinees from joinees table
		/// </summary>
		/// <param name="boardID"> the board id needed to delete its joinees </param>
		/// <returns></returns>
		public bool DeleteboardfromJoinees(int boardID)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {UsersjoineesTable} WHERE BoardID = @id";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
                    command.Parameters.Add(Pram1);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Error deleting from dataBase");
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
		/// an a sql query to remove 1 joinee from the joinees table
		/// </summary>
		/// <param name="email"> the users email</param>
		/// <param name="boardID"> the boardID to delete its joinee</param>
		/// <returns></returns>
		public bool LeaveBoard(string email, int boardID)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {UsersjoineesTable} WHERE BoardID = @id AND Email = @email";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
                    SQLiteParameter Pram2 = new SQLiteParameter(@"email", email);
                    command.Parameters.Add(Pram1);
                    command.Parameters.Add(Pram2);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("Error deleting from dataBase");
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
        /// this method converts the data to opject
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public BoardDTO ConvertReaderToObject(SQLiteDataReader dataReader)
        {
            BoardDTO result = new BoardDTO(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetInt32(3));
            return result;
        }

        /// <summary>
		/// an sql query to update in the board table
		/// </summary>
		/// <param name="brdId"> boardId to specify the row</param>
		/// <param name="attributeName"> column name in the table</param>
		/// <param name="attributeValue"> the new value to set</param>
		/// <returns></returns>
		public bool Update(int brdId, string attributeName, string attributeValue)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"UPDATE {BoardTable} SET [{attributeName}]=@attributevalue WHERE BoardID=@brdId";
                    SQLiteParameter attribpram = new SQLiteParameter(@"attributevalue", attributeValue);
                    SQLiteParameter Pram1 = new SQLiteParameter(@"brdId", brdId);
                    command.Parameters.Add(attribpram);
                    command.Parameters.Add(Pram1);
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

        /// <summary>
		/// an sql query to get the maximum BoardId for the boardcontroller
		/// </summary>
		/// <returns></returns>
		public int getMaxbrdID()
        {
            int idcounter = 0;
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT max(BoardID) FROM Boards";
                SQLiteDataReader dataReader = null;


                try
                {
                    connection.Open();

                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        try
                        {
                            idcounter = dataReader.GetInt32(0) + 1;
                        }
                        catch (Exception e)
                        {
                            idcounter = 0;
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
            return idcounter;
        }
    }
}
