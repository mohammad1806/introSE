using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DALcontroller
    {
        protected readonly string _connection;
        private readonly string _table;
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DALcontroller(string table)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connection = $"Data Source={path}; Version=3";
            _table = table;
        }

        /// <summary>
		/// deletes all data in a wanted table
		/// </summary>
		/// <returns></returns>
		public bool DeleteData()
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    command.CommandText = $"DELETE  FROM {_table}";
                    command.Prepare();
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error("error connecting to the dataBase");
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
    }
}
