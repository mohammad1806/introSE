using System;


namespace IntroSE.Kanban.Backend.ServiceLayer;

/// <summary>
/// A class for grading your work <b>ONLY</b>. The methods are not using good SE practices and you should <b>NOT</b> infer any insight on how to write the service layer/business layer. 
/// <para>
/// Each of the class' methods should return a JSON string with the following structure (see <see cref="System.Text.Json"/>):
/// <code>
/// {
///     "ErrorMessage": &lt;string&gt;,
///     "ReturnValue": &lt;object&gt;
/// }
/// </code>
/// Where:
/// <list type="bullet">
///     <item>
///         <term>ReturnValue</term>
///         <description>
///             The return value of the function.
///             <para>
///                 The value may be either a <paramref name="primitive"/>, a <paramref name="Task"/>, or an array of of them. See below for the definition of <paramref name="Task"/>.
///             </para>
///             <para>If the function does not return a value or an exception has occorred, then the field should be either null or undefined.</para>
///         </description>
///     </item>
///     <item>
///         <term>ErrorMessage</term>
///         <description>If an exception has occorred, then this field will contain a string of the error message. Otherwise, the field will be null or undefined.</description>
///     </item>
/// </list>
/// </para>
/// <para>
/// An empty response is a response that both fields are either null or undefined.
/// </para>
/// <para>
/// The structure of the JSON of a Task, is:
/// <code>
/// {
///     "Id": &lt;int&gt;,
///     "CreationTime": &lt;DateTime&gt;,
///     "Title": &lt;string&gt;,
///     "Description": &lt;string&gt;,
///     "DueDate": &lt;DateTime&gt;
/// }
/// </code>
/// </para>
/// </summary>
public class GradingService
{
    private ServiceHup ServiceHup;
    public GradingService()
    {
      this.ServiceHup = new ServiceHup();

    }

    public GradingService(ServiceHup serviceHup)
    {
        this.ServiceHup = serviceHup;
    }
    /// <summary>
    /// This method registers a new user to the system.
    /// </summary>
    /// <param name="email">The user email address, used as the username for logging the system.</param>
    /// <param name="password">The user password.</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string Register(string email, string password)
    {
        return ServiceHup.Register(email, password);
    }


    /// <summary>
    ///  This method logs in an existing user.
    /// </summary>
    /// <param name="email">The email address of the user to login</param>
    /// <param name="password">The password of the user to login</param>
    /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string Login(string email, string password)
    {
        return ServiceHup.Login(email, password);
    }


    /// <summary>
    /// This method logs out a logged in user. 
    /// </summary>
    /// <param name="email">The email of the user to log out</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string Logout(string email)
    {
        return ServiceHup.Logout(email);
    }

    /// <summary>
    /// This method limits the number of tasks in a specific column.
    /// </summary>
    /// <param name="email">The email address of the user, must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
    /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
    {
        return ServiceHup.LimitColumn(email, boardName, columnOrdinal, limit);
    }

    /// <summary>
    /// This method gets the limit of a specific column.
    /// </summary>
    /// <param name="email">The email address of the user, must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
    /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string GetColumnLimit(string email, string boardName, int columnOrdinal)
    {
        return ServiceHup.GetColumnLimit(email, boardName, columnOrdinal);
    }


    /// <summary>
    /// This method gets the name of a specific column
    /// </summary>
    /// <param name="email">The email address of the user, must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
    /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string GetColumnName(string email, string boardName, int columnOrdinal)
    {
        return ServiceHup.GetColumnName(email, boardName, columnOrdinal);
    }


    /// <summary>
    /// This method adds a new task.
    /// </summary>
    /// <param name="email">Email of the user. The user must be logged in.</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="title">Title of the new task</param>
    /// <param name="description">Description of the new task</param>
    /// <param name="dueDate">The due date if the new task</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
    {
        return ServiceHup.AddTask(email, boardName, title, description, dueDate);
    }


    /// <summary>
    /// This method updates the due date of a task
    /// </summary>
    /// <param name="email">Email of the user. Must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
    /// <param name="taskId">The task to be updated identified task ID</param>
    /// <param name="dueDate">The new due date of the column</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
    {
        return ServiceHup.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
    }


    /// <summary>
    /// This method updates task title.
    /// </summary>
    /// <param name="email">Email of user. Must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
    /// <param name="taskId">The task to be updated identified task ID</param>
    /// <param name="title">New title for the task</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
    {
        return ServiceHup.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
    }


    /// <summary>
    /// This method updates the description of a task.
    /// </summary>
    /// <param name="email">Email of user. Must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
    /// <param name="taskId">The task to be updated identified task ID</param>
    /// <param name="description">New description for the task</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
    {
        return ServiceHup.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
    }


    /// <summary>
    /// This method advances a task to the next column
    /// </summary>
    /// <param name="email">Email of user. Must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
    /// <param name="taskId">The task to be updated identified task ID</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
    {
        return ServiceHup.AdvanceTask(email, boardName, columnOrdinal, taskId);
    }


    /// <summary>
    /// This method returns a column given it's name
    /// </summary>
    /// <param name="email">Email of the user, must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
    /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string GetColumn(string email, string boardName, int columnOrdinal)
    {
        return ServiceHup.GetColumn(email, boardName, columnOrdinal);
    }


    /// <summary>
    /// This method creates a board for the given user.
    /// </summary>
    /// <param name="email">Email of the user, must be logged in</param>
    /// <param name="name">The name of the new board</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string CreateBoard(string email, string name)
    {
        return ServiceHup.CreateBoard(email, name);
    }


    /// <summary>
    /// This method deletes a board.
    /// </summary>
    /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
    /// <param name="name">The name of the board</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string DeleteBoard(string email, string name)
    {
        return ServiceHup.DeleteBoard(email, name);
    }


    /// <summary>
    /// This method returns all in-progress tasks of a user.
    /// </summary>
    /// <param name="email">Email of the user. Must be logged in</param>
    /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string InProgressTasks(string email)
    {
        return ServiceHup.InProgressTasks(email);
    }
    /// <summary>
    /// this method changes users password
    /// </summary>
    /// <param name="email">the email for the specific user</param>
    /// <param name="password">the old password</param>
    /// <param name="NewPassword">the new one</param>
    /// <returns></returns>
    public string ChangePassword(string email, string password,String NewPassword) 
    {
        return ServiceHup.ChangePassword(email, password, NewPassword);
    }
    /// <summary>
    /// This method returns a list of IDs of all user's boards.
    /// </summary>
    /// <param name="email">Email of the user. Must be logged in</param>
    /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string GetUserBoards(string email)
    {
        return ServiceHup.GetUserBoards(email);
    }

    /// <summary>
    /// This method adds a user as member to an existing board.
    /// </summary>
    /// <param name="email">The email of the user that joins the board. Must be logged in</param>
    /// <param name="boardID">The board's ID</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string JoinBoard(string email, int boardID)
    {
        return ServiceHup.JoinBoard(email, boardID);
    }

    /// <summary>
    /// This method removes a user from the members list of a board.
    /// </summary>
    /// <param name="email">The email of the user. Must be logged in</param>
    /// <param name="boardID">The board's ID</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string LeaveBoard(string email, int boardID)
    {
        return ServiceHup.LeaveBoard(email, boardID);
    }

    /// <summary>
    /// This method assigns a task to a user
    /// </summary>
    /// <param name="email">Email of the user. Must be logged in</param>
    /// <param name="boardName">The name of the board</param>
    /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
    /// <param name="taskID">The task to be updated identified a task ID</param>        
    /// <param name="emailAssignee">Email of the asignee user</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
    {
        return ServiceHup.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
    }

    /// <summary>
    /// This method returns a board's name
    /// </summary>
    /// <param name="boardId">The board's ID</param>
    /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string GetBoardName(int boardId)
    {
        return ServiceHup.GetBoardName(boardId);
    }

    /// <summary>
    /// This method transfers a board ownership.
    /// </summary>
    /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
    /// <param name="newOwnerEmail">Email of the new owner</param>
    /// <param name="boardName">The name of the board</param>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
    {
        return ServiceHup.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
    }

    ///<summary>This method loads all persisted data.
    ///<para>
    ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
    ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
    ///</para>
    /// </summary>
    /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string LoadData()
    {
        return ServiceHup.LoadData();
    }

    ///<summary>This method deletes all persisted data.
    ///<para>
    ///<b>IMPORTANT:</b> 
    ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
    ///</para>
    /// </summary>
    ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
    public string DeleteData()
    {
        return ServiceHup.DeleteData();
    }
}
