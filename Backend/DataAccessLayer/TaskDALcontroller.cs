using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDALcontroller : DALcontroller
    {
        private const string TaskTable = "Tasks";
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TaskDALcontroller() : base(TaskTable) { }

        public bool DeleteAll()
        {
            return DeleteData();
        }


        public bool AddTask(TaskDTO task)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTable}({TaskDTO.TaskIDColumnName}, {TaskDTO.BoardIDColumnName} ,{TaskDTO.TitleColumnName}, {TaskDTO.DescriptionColumnName}, {TaskDTO.CreationTimeColumnName}, {TaskDTO.DueDateColumnName},{TaskDTO.ColumnOrdinalColumnName},{TaskDTO.AssigneeColumnName})"
                        + $"VALUES(@taskid ,@brdid, @title, @desc , @creationtime, @duedate , @ordinal, @assignee)";
                    SQLiteParameter Param1 = new SQLiteParameter(@"taskid", task.TaskID);
                    SQLiteParameter Param2 = new SQLiteParameter(@"brdid", task.BoardID);
                    SQLiteParameter Param3 = new SQLiteParameter(@"title", task.Title);
                    SQLiteParameter Param4 = new SQLiteParameter(@"desc", task.Description);
                    SQLiteParameter Param5 = new SQLiteParameter(@"creationtime", task.CreationTime);
                    SQLiteParameter Param6 = new SQLiteParameter(@"duedate", task.DueDate);
                    SQLiteParameter Param7 = new SQLiteParameter(@"ordinal", task.ColOrd);
                    SQLiteParameter Param8 = new SQLiteParameter(@"assignee", task.Assignee);
                    command.Parameters.Add(Param1);
                    command.Parameters.Add(Param2);
                    command.Parameters.Add(Param3);
                    command.Parameters.Add(Param4);
                    command.Parameters.Add(Param5);
                    command.Parameters.Add(Param6);
                    command.Parameters.Add(Param7);
                    command.Parameters.Add(Param8);
                    command.Prepare();
                    res = command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    log.Error("error occured while updating the database");
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
        /// this SQL query deletes the task
        /// </summary>
        /// <param name="boardID"> the ID of the board</param>
        /// <returns></returns>
        public bool DeleteBoardsTasks(int boardID)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {TaskTable} WHERE BoardID = @id";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
                    command.Parameters.Add(Pram1);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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

        public bool Update(int taskID, int brdid, string attributeName, string attributeValue)
        {

            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"UPDATE {TaskTable} SET [{attributeName}] = @attributevalue WHERE TaskID = @ID AND BoardID = @brdId";
                    SQLiteParameter attrib = new SQLiteParameter(@"attributevalue", attributeValue);
                    SQLiteParameter idPar = new SQLiteParameter(@"ID", taskID);
                    SQLiteParameter brdPar = new SQLiteParameter(@"brdId", brdid);
                    command.Parameters.Add(idPar);
                    command.Parameters.Add(brdPar);
                    command.Parameters.Add(attrib);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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


        public List<TaskDTO> listTasksForClumn(int brdid, int ordinal)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TaskTable} WHERE BoardID = @brdid and ColumnOrdinal = @ord;";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    SQLiteParameter Pram1 = new SQLiteParameter(@"brdid", brdid);
                    SQLiteParameter Pram2 = new SQLiteParameter(@"ord", ordinal);
                    command.Parameters.Add(Pram1);
                    command.Parameters.Add(Pram2);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                catch(Exception e)
                {
                    Response response = new Response(e);
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


        public TaskDTO ConvertReaderToObject(SQLiteDataReader dataReader)
        {
            return new TaskDTO(dataReader.GetInt32(0), dataReader.GetInt32(1),dataReader.GetString(2),dataReader.GetString(3),Convert.ToDateTime(dataReader.GetString(4)),Convert.ToDateTime(dataReader.GetString(5)),dataReader.GetInt32(6),dataReader.GetString(7));
        }

    }
}
