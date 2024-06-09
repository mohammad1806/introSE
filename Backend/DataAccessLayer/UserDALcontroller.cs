using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDALcontroller : DALcontroller
    {
        private const string UserTable = "Users";
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UserDALcontroller() : base(UserTable) { }

        public bool DeleteAll()
        {
            return DeleteData();
        }

        public bool addUser(UserDTO user)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTable}({UserDTO.EmailColumnName},{UserDTO.PasswordColumnName})"
                        + $"VALUES(@email,@password)";

                    SQLiteParameter Pram1 = new SQLiteParameter(@"email", user.Email);
                    SQLiteParameter Pram2 = new SQLiteParameter(@"password", user.Password);
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


        public List<UserDTO> getAllUsers()
        {
            List<UserDTO> results = new List<UserDTO>();
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {UserTable};";
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

        public UserDTO ConvertReaderToObject(SQLiteDataReader dataReader)
        {
            return new UserDTO(dataReader.GetString(0),dataReader.GetString(1),false);
        }
    }
}
