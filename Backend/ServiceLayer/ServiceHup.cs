using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using IntroSE.Kanban.Backend.businessLayer.board;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceHup
    {
        private boardService boardService;
        private userService userService;
        private columnService columnService;
        private TasksService tasksService;

        public ServiceHup() 
        {
            boardService = new boardService();
            userService = new userService();
            columnService = new columnService();
            tasksService = new TasksService();

        }


        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string Register(string email, string password)
        {
           String json = userService.Register(email, password);
            if (checkJSN(json))
            {
                boardService.getCon().AddUser(email);
                updateControllers(boardService.getCon());
            }
            return json;
        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs </returns>
        public string Login(string email, string password)
        {
           String json = userService.Login(email, password);
            if(checkJSN(json))
            {
                boardService.getCon().usersStatus[email] = true;
                updateControllers(boardService.getCon());
            }
            return json;
        }


        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string Logout(string email)
        {
            
            String json = userService.Logout(email);
            if (checkJSN(json))
            {
                boardService.getCon().usersStatus[email]=false;
                updateControllers(boardService.getCon());
            }
            return json;
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            String json = columnService.LimitColumn(email, boardName, columnOrdinal, limit);
            if (checkJSN(json))
            {
                updateControllers(columnService.getCon());
            }
            return json;
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs </returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            return columnService.GetColumnLimit(email, boardName, columnOrdinal);
        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs </returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            return columnService.GetColumnName(email, boardName, columnOrdinal);
        }


        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            string json = tasksService.AddTask(email, boardName, title, description, dueDate);
            if (checkJSN(json))
            {
               updateControllers(tasksService.getCon());
            }
            return json;
        }


        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            String json = tasksService.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
            if (checkJSN(json))
            {
                updateControllers(tasksService.getCon());
            }
            return json;
        }


        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            String json = tasksService.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
            if (checkJSN(json))
            {
                updateControllers(tasksService.getCon());
            }
            return json;
        }


        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            String json = tasksService.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
            if (checkJSN(json))
            {
                updateControllers(tasksService.getCon());
            }
            return json;
        }


        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            String json = tasksService.AdvanceTask(email, boardName, columnOrdinal, taskId);
            if (checkJSN(json))
            {
                 updateControllers(tasksService.getCon());
            }
            return json;
        }


        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs </returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            return columnService.GetColumn(email, boardName, columnOrdinal);
        }


        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string CreateBoard(string email, string name)
        {
            String json = boardService.CreateBoard(email, name);
            if(checkJSN(json))
            {
                updateControllers(boardService.getCon());
            }
            return json;
        }


        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string DeleteBoard(string email, string name)
        {
            string json = boardService.DeleteBoard(email, name);
            if (checkJSN(json))
            {
                updateControllers(boardService.getCon());
            }
            return json;
        }


        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs </returns>
        public string InProgressTasks(string email)
        {
            return tasksService.InProgressTasks(email);
        }
        /// <summary>
        /// this function changes a specific users password
        /// </summary>
        /// <param name="email"> the users email</param>
        /// <param name="password"> the old password</param>
        /// <param name="NewPass">the new one</param>
        /// <returns> an empty respone unless an error occur</returns>
        public string ChangePassword(string email, string password,string NewPass)
        {
            return userService.changePassword(email, password,NewPass);
        }
        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs </returns>
        public string GetUserBoards(string email)
        {
            return boardService.GetUserBoards(email);
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string JoinBoard(string email, int boardID)
        {
           string json = boardService.JoinBoard(email, boardID);
            if (checkJSN(json))
            {
                updateControllers(boardService.getCon());
            }
            return json;
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string LeaveBoard(string email, int boardID)
        {
            string json = boardService.LeaveBoard(email, boardID);
            if (checkJSN(json))
            {
                updateControllers(boardService.getCon());
            }
            return json;
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            string json = tasksService.AssignTask(email,boardName,columnOrdinal,taskID,emailAssignee);
            if (checkJSN(json))
            {
                updateControllers(tasksService.getCon());
            }
            return json;
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs </returns>
        public string GetBoardName(int boardId)
        {
            return boardService.GetBoardName(boardId);
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            string json = boardService.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
            if (checkJSN(json))
            {
                updateControllers(boardService.getCon());
            }
            return json;
        }
        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs </returns>
        public string LoadData()
        {
            string json = userService.loadData();
            if (checkJSN(json))
            {
                boardService.loadData();
                updateControllers(boardService.getCon());
            }
            return json;
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs </returns>
        public string DeleteData()
        {
            string json = userService.DeleteData();
            if (checkJSN(json))
            {
                boardService.DeleteData();
                updateControllers(boardService.getCon());
            }
            return json;
        }
        /// <summary>
        /// this function checks if the json return exeption
        /// </summary>
        /// <param name="JSN">receives string Json</param>
        /// <returns></returns>
        public bool checkJSN(string JSN)
        {
            Response res = JsonSerializer.Deserialize<Response>(JSN);
            if (res.ErrorOccured)
            {
                return false;
            }
            return true;
        }

        public void updateControllers(boardController controller)
        {
           this.boardService = new boardService(controller);
           this.tasksService = new TasksService(controller);
           this.columnService = new columnService(controller);

        }
    }

   

}
