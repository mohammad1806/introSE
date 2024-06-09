using IntroSE.Kanban.Backend.businessLayer.board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class columnService
    {
        
        private boardController Bcontroller;

        public columnService()
        {
            Bcontroller = new boardController();
        }

        public columnService(boardController other)
        {
            this.Bcontroller = other;
        }

        public boardController getCon()
        {
            return Bcontroller;
        }
        /// <summary>
        /// Thos method limits the number of tasks that can be in a specific column in the board
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">he new maximum limit for the number of tasks in the column</param>
        /// <returns>JSON-formatted response containing either an error message or a success message</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.LimitColumn(email, boardName, columnOrdinal, limit);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;

        }
        /// <summary>
        /// This method retrieves the limit of the number of tasks that can be in a column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>JSON-formatted response containing either an error message or a success message</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            Response res = new Response();
            String Json;
            try
            {
                int L = Bcontroller.getColLimit(email, boardName, columnOrdinal);
                res = new Response(null, L);
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// This method retrieves the name of the column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>JSON-formatted response containing either an error message or a success message</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            Response res = new Response();
            String Json;
            try
            {
                String e = Bcontroller.getColName(email, boardName, columnOrdinal);
                res = new Response(null,e);
            }
            catch( Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// This method retrieves all the tasks in a column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>JSON-formatted response containing either an error message or a success message</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {

            Response<TaskToSend[]> res;
            String Json;
            try
            {
                List<businessLayer.board.Task> tsks = Bcontroller.getCol(email, boardName, columnOrdinal).getcolumn;
                List<TaskToSend> tasks = new List<TaskToSend>();
                foreach(var task in tsks)
                {
                    TaskToSend taskToReturn = new TaskToSend(task);
                    tasks.Add(taskToReturn);
                }
                TaskToSend[] T = tasks.ToArray();
                res = new Response<TaskToSend[]>(T);
            }
            catch(Exception ex)
            {
                res = new Response<TaskToSend[]>(null,ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }
    }
}
