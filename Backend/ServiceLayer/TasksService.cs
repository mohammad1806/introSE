using IntroSE.Kanban.Backend.businessLayer.board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class TasksService
    {
        private boardController Bcontroller;

        public TasksService()
        {
            Bcontroller = new boardController();
        }

        public TasksService(boardController other)
        {
            this.Bcontroller = other;
        }

        public boardController getCon()
        {
            return Bcontroller;
        }
        /// <summary>
        /// Method for adding a new task to a board
        /// </summary>
        /// <param name="email">The email of the user adding the task</param>
        /// <param name="boardName">The name of the board to which the task will be added</param>
        /// <param name="title">The title of the task</param>
        /// <param name="description">The description of the task</param>
        /// <param name="dueDate">The due date of the task</param>
        /// <returns>A JSON response indicating success or failure of the operation</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.AddTask(email, boardName, title, description, dueDate);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;

        }

        /// <summary>
        /// Method for updating the due date of a task
        /// </summary>
        /// <param name="email">The email of the user updating the task</param>
        /// <param name="boardName">The name of the board that contains the task</param>
        /// <param name="columnOrdinal">The column ordinal of the column containing the task</param>
        /// <param name="taskId">The ID of the task to update</param>
        /// <param name="dueDate">The new due date of the task</param>
        /// <returns>A JSON response indicating success or failure of the operation</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// Method for updating the title of a task
        /// </summary>
        /// <param name="email">The email of the user updating the task</param>
        /// <param name="boardName">The name of the board that contains the task</param>
        /// <param name="columnOrdinal">The column ordinal of the column containing the task</param>
        /// <param name="taskId">The ID of the task to update</param>
        /// <param name="title">The new title of the task</param>
        /// <returns>A JSON response indicating success or failure of the operation</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
            }
            catch(Exception ex) 
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// This method updates the description of a specific task
        /// </summary>
        /// <param name="email">The email of the user updating the task</param>
        /// <param name="boardName">The name of the board that contains the task</param>
        /// <param name="columnOrdinal">The column ordinal of the column containing the task</param>
        /// <param name="taskId">The ID of the task to update</param>
        /// <param name="description">The updated description of the task</param>
        /// <returns>A JSON string representing a response object</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.UpdateTaskDescription(email, boardName, columnOrdinal,taskId, description);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// This method advances a specific task to the next column
        /// </summary>
        /// <param name="email">The email of the user updating the task</param>
        /// <param name="boardName">The name of the board that contains the task</param>
        /// <param name="columnOrdinal">The column ordinal of the column containing the task</param>
        /// <param name="taskId">The ID of the task to update</param>
        /// <returns>A JSON string representing a response object</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.AdvanceTask(email, boardName, columnOrdinal, taskId);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// This method retrieves all tasks that are currently in progress for a specific user
        /// </summary>
        /// <param name="email">The email of the user updating the task</param>
        /// <returns>A JSON string representing a response object</returns>
        public string InProgressTasks(string email)
        {
            Response res = new Response();
            String Json;
            try
            {
                List<businessLayer.board.Task> tsks = Bcontroller.InProgressTasks(email);
                List<TaskToSend> tasks = new List<TaskToSend>();
                foreach (var task in tsks)
                {
                    TaskToSend taskToReturn = new TaskToSend(task);
                    tasks.Add(taskToReturn);
                }
                TaskToSend[] T = tasks.ToArray();
                res = new Response(T);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }
        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.AssignTask(email,boardName,columnOrdinal,taskID,emailAssignee);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }
    }
}
