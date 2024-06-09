using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.businessLayer.board
{
    public class boardController
    {
        public Dictionary<string, bool> usersStatus;
        private Dictionary<string, List<board>> usersboards;
        private Dictionary<int,board> boards; 
        private int brdID;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private BoardDALcontroller BDALcon;
        private ColumnDALcontroller CDALcon;
        private TaskDALcontroller TDALcon;
        private UserDALcontroller UDALcon;
        
        //constructor
        public boardController()
        {
            boards = new Dictionary<int,board>();
            usersboards = new Dictionary<string, List<board>>();
            usersStatus = new Dictionary<string, bool>();

            BDALcon = new BoardDALcontroller();
            CDALcon = new ColumnDALcontroller();
            TDALcon = new TaskDALcontroller();
            UDALcon = new UserDALcontroller();
        }

        /// <summary>
        /// delete all the data
        /// </summary>
        public void DeleteData()
        {
            BDALcon.DeleteAllBoards();
            CDALcon.DeleteAll();
            TDALcon.DeleteAll();
            usersboards = new Dictionary<string, List<board>>();
            usersStatus = new Dictionary<string, bool>();
            boards = new Dictionary<int,board>();
            brdID = 0;
        }
        /// <summary>
        /// load the data
        /// </summary>
        public void loadData() 
        {
            loaduserStatus();
            loadUsersBoards();
            loadBoards();
        }
        /// <summary>
        /// load the user's status
        /// </summary>
        public void loaduserStatus()
        {
            List<UserDTO> users = UDALcon.getAllUsers();
            foreach (UserDTO u in users)
            {
                usersStatus.Add(u.Email, false);
            }

        }
        /// <summary>
        /// load the boards
        /// </summary>
        public void loadBoards()
        {
            List<BoardDTO> allBoards = BDALcon.getAllBoards();
            foreach(BoardDTO b in allBoards)
            {
                boards.Add(b.BrdID, new board(b));
            }
        }
        /// <summary>
        /// load the boards of the users
        /// </summary>
        public void loadUsersBoards()
        {
            List<UserDTO> allUsers = UDALcon.getAllUsers();
            foreach(UserDTO u in allUsers)
            {
                List<BoardDTO> usersBoards = BDALcon.Getusersboardslist(u.Email);
                List<board> brds = new List<board>();
                foreach(BoardDTO b in usersBoards)
                {
                    board newB = new board(b);
                    foreach(column c in newB.Columns)
                    {
                        c.loadColumnsData();
                    }
                    brds.Add(newB);
                }
                usersboards.Add(u.Email, brds);
            }
            brdID = BDALcon.getMaxbrdID();
        }
        
       
        /// <summary>
        /// This method adds a new board
        /// </summary>
        /// <param name="email">  user emai that the board belong tol</param>
        /// <param name="boardName"> name of the new board</param>
        /// <exception cref="ArgumentException"></exception>
        public void CreateBoard(string email, string boardName)
        {
            checkBoardName(boardName);
            checkStatus(email);
            if (checkMail(email))
            {
                List<board> boards = usersboards[email];
                foreach (board brd in boards)
                {
                    if (brd.BoardName.Equals(boardName))
                    {
                        log.Warn("attempt to add a board with an existing name!");
                        throw new ArgumentException("board name already exist !");
                    }
                }
            }
            board b = new board(email, boardName,brdID);
            usersboards[email].Add(b);
            boards.Add(brdID,b);
            brdID++;
            BDALcon.addBoard(b.convrtToDTO());
            BDALcon.addJoinee(b.UserEmail, b.BrdID);
            log.Info("board added successfuly");
        }

        /// <summary>
        /// This method deletes a board
        /// </summary>
        /// <param name="email">user email that the board belong to </param>
        /// <param name="boardName"> the name of the board we deleting </param>
        public void DeleteBoard(string email, string boardName)
        {
            checkBoardName(boardName);
            checkStatus(email);
            if (checkMail(email))
            {
                
                List<board> boards = usersboards[email];
                foreach (board brd in boards)
                {
                    if (brd.BoardName.Equals(boardName))
                    {
                        if (!brd.UserEmail.Equals(email))
                        {
                            log.Warn("attempt to delete a board that not belong to this user!");
                            throw new ArgumentException("this user is not the owner for this board!");
                        }
                        boards.Remove(brd);
                        BDALcon.DeleteTheBoard(brd.BrdID);
                        CDALcon.DeleteBoardsColumns(brd.BrdID);
                        TDALcon.DeleteBoardsTasks(brd.BrdID); 
                        List<string> list = new List<string>(usersboards.Keys);
                        foreach (string key in list)
                        {
                            usersboards[key].Remove(brd);
                            
                        }
                        return;
                    }
                }
                throw new ArgumentException("this user is not the board owner!");
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <exception cref="ArgumentException"></exception>
        public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            
             if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new ArgumentException("columnOrdinal not valid !");
            }
            checkBoardName(boardName);
            checkStatus(email);
             if ( checkMail(email))
            {
                List<board> boards = usersboards[email];
                foreach (board brd in boards)
                {
                    if (brd.BoardName.Equals(boardName))
                    {
                        brd.getCol(columnOrdinal).LimitColumn(limit);
                        log.Info("column limited successfuly");
                    }
                }
            }
        }


        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>column limit value, unless an error occurs</returns>
        /// <exception cref="ArgumentException"></exception>
        public int getColLimit(string email, string boardName, int columnOrdinal)
        {
           
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new ArgumentException(" not valid columnOrdinal");
            }
            checkBoardName(boardName);
            checkStatus(email);
            if(checkMail(email))
            {
                List<board> boards = usersboards[email];
                foreach (board brd in boards)
                {
                    if (brd.BoardName.Equals(boardName))
                    {
                        return (brd.getCol(columnOrdinal)).ColumnLimit;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column name value, unless an error occurs
        /// <exception cref="ArgumentException"></exception>
        public String getColName(string email, string boardName, int columnOrdinal)
        {
            
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new ArgumentException(" not valid columnOrdinal");
            }
            checkBoardName(boardName);
            checkStatus(email);
            if(checkMail(email))
            {
                List<board> boards = usersboards[email];
                foreach (board brd in boards)
                {
                    if (brd.BoardName.Equals(boardName))
                    {
                        return (brd.getCol(columnOrdinal)).getColumnName;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This method returns a list of tasks for a specific column
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID</param>
        /// <returns>list of tasks</returns>
        /// <exception cref="ArgumentException"></exception>
        public column getCol(string email, string boardName, int columnOrdinal)
        {
           
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new ArgumentException(" not valid columnOrdinal");
            }
            checkBoardName(boardName);
            checkStatus(email);
            if(checkMail(email) & usersStatus[email])
            {
                List<board> boards = usersboards[email];
                foreach (board brd in boards)
                {
                    if (brd.BoardName.Equals(boardName))
                    {
                        return brd.getCol(columnOrdinal);
                    }
                }
            }
            throw new ArgumentException("there is no board with this name for this user");
        }

        /// <summary>
        /// This method adds a new task to the board
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">title or name of the task being added</param>
        /// <param name="description">provides additional details or information about the task.</param>
        /// <param name="dueDate">deadline or due date for the task</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            
            checkBoardName(boardName);
            checkStatus(email);
            if (checkMail(email))
            {
                List<board> boards = usersboards[email];
                foreach(board brd in boards)
                {
                    if (brd.BoardName.Equals(boardName))
                    {

                        Task toAadd = new Task(brd.getTaskID, title, description, dueDate,brd.BrdID,0);
                        checktask(toAadd);
                        brd.getCol(0).AddTask(toAadd);
                        brd.getTaskID++;
                        return;
                    }
                    
                }
                log.Warn("trying to add task to board that is not belong to this user!");
                throw new ArgumentException("no board with this name for this user!");
            }
            return ;
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="boardName">the name of the board</param>
        /// <param name="columnOrdinal">the ID of the column</param>
        /// <param name="taskId">the ID  of the task</param>
        /// <param name="dueDate">deadline or due date for the task</param>
        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            checkOrd(columnOrdinal);
            checkBoardName(boardName);
            checkStatus(email);
            if(checkMail(email))
            {
                Task t = getTask(email, boardName, columnOrdinal, taskId);
                 t.getDuedate=dueDate;
                 t.convrtToDTO().DueDate=dueDate;
                log.Info("task duedate updated successfuly");

            }
        }

        /// <summary>
        /// This method updates the title of a task
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="boardName">the name of the board</param>
        /// <param name="columnOrdinal">the ID of the column</param>
        /// <param name="taskId">the ID  of the task</param>
        /// <param name="Title">title or name of the task </param>
        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, String Title)
        {
            checkOrd(columnOrdinal);
            checkBoardName(boardName);
            checkStatus(email);
            if(checkMail(email))
            {
                Task t = getTask(email, boardName, columnOrdinal, taskId);
                t.getTitle=Title;
                t.convrtToDTO().Title=Title;
                log.Info("task Title updated successfuly");
            }
        }

        /// <summary>
        /// This method updates the description of a specific task
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="boardName">the name of the board</param>
        /// <param name="columnOrdinal">the ID of the column</param>
        /// <param name="taskId">the ID  of the task</param>
        /// <param name="description">provides additional details or information about the task</param>
        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            checkOrd(columnOrdinal);
            checkBoardName(boardName);
            checkStatus(email);
            if(checkMail(email))
            {
                Task t = getTask(email, boardName, columnOrdinal, taskId);
                t.getDescription = description;
                t.convrtToDTO().Description=description;
                log.Info("task description updated successfuly");
            }
        }

        /// <summary>
        /// This method advances a task from one column to the next one
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="boardName">the name of the board</param>
        /// <param name="columnOrdinal">the ID of the column</param>
        /// <param name="taskId">the ID  of the task</param>
        /// <exception cref="ArgumentException"></exception>
        public void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            
            checkOrd(columnOrdinal);
            checkBoardName(boardName);
            checkStatus(email);
            if (checkMail(email))
            {   
                Task task = getTask(email, boardName, columnOrdinal, taskId);
                if(!task.assignee.Equals(email)) { throw new ArgumentException("this user is not the assignee for this task"); }
                foreach(board brd in usersboards[email])
                {
                    if (brd.BoardName.Equals(boardName))
                        brd.AdvanceTask(task, columnOrdinal);
                    log.Info("task advanced successfuly");
                    return;
                }
                
            }
        }

        /// <summary>
        /// This method retrieve a list of all tasks that are currently in progress for the user
        /// </summary>
        /// <param name="email">email of the user</param>
        public List<Task> InProgressTasks(string email)
        {
            List<Task> IPT = new List<Task>();
            checkStatus(email);
            if (checkMail(email))
            {
                List<board> boards = usersboards[email];
                foreach (board board in boards)
                {
                    column col = board.getCol(1);
                    List<Task> tasks = col.getcolumn;
                    foreach(var task in tasks)
                    {
                        if (task.assignee.Equals(email))
                        {
                            IPT.Add(task);
                        }
                    }
                }
                return IPT;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<int> GetUserBoards(string email)
        {
            List<int> brdsIDs = new List<int>();
            checkStatus(email);
            if (checkMail(email))
            {
                foreach(board board in usersboards[email])
                {
                    brdsIDs.Add(board.BrdID);
                }
            }
            return brdsIDs;
        }

        /// <summary>
        /// joins a user to an existing board
        /// </summary>
        /// <param name="email">the mail of the user we want to jointo the board</param>
        /// <param name="boardID">board's ID</param>
        /// <exception cref="ArgumentException"></exception>
        public void JoinBoard(string email, int boardID)
        {
            checkStatus(email);
            if (checkMail(email))
            {
                if (!boards.ContainsKey(boardID))
                {
                    log.Warn("attempt to join unexisting board!");
                    throw new ArgumentException("there is no board with this ID");
                }
                board b = boards[boardID];
                if (usersboards[email].Contains(b))
                {
                    log.Warn("user attempt to join a board that already belong to him!");
                    throw new ArgumentException("the user cannot join a board thats already belong to him!");
                }
                foreach (var board in usersboards[email])
                {
                    if (board.BoardName.Equals(b.BoardName))
                    {
                        log.Warn("attempt to join a board that its name is already exist in the boards list!");
                        throw new ArgumentException("cannot join a board thats its name is already in this users list of boards!");
                    }
                }
                usersboards[email].Add(b);
                BDALcon.addJoinee(email, boardID);
                log.Info("joined to board sccussefuly");
            }
        }
        /// <summary>
        /// this method allows to leave a board
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="boardID">the board the user wants to leave</param>
        /// <exception cref="ArgumentException"></exception>
        public void LeaveBoard(string email, int boardID)
        {
            checkStatus(email);
            if (checkMail(email))
            {
                if (!boards.ContainsKey(boardID))
                {
                    log.Warn("attempt to leave unexisting board!");
                    throw new ArgumentException("there is no board with this ID");
                }
                board b = boards[boardID];
                foreach(var board in usersboards[email])
                {
                    if (board.BoardName.Equals(b.BoardName))
                    {
                        if (b.UserEmail.Equals(email))
                        {
                            log.Warn("user attempt to leave board that he own");
                            throw new ArgumentException("cannot leave a board that you own it!");
                        }
                        b.freeTheTasks(email);
                        usersboards[email].Remove(board);
                        BDALcon.LeaveBoard(email,boardID);
                        return;
                    }
                }
                log.Warn("attempt to leave a board that doesnt joined to");
                throw new ArgumentException("board does not exist in this user boards!");
            }
        }
        /// <summary>
        /// this method is to assign tasks
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="boardName">the board the task in</param>
        /// <param name="columnOrdinal">the column the task in</param>
        /// <param name="taskID">task's ID</param>
        /// <param name="emailAssignee">assignee's email</param>
        /// <exception cref="ArgumentException"></exception>
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            checkOrd(columnOrdinal);
            checkBoardName(boardName);
            checkStatus(email);
            checkStatus(emailAssignee);
            if (checkMail(email) && checkMail(emailAssignee))
            {
                foreach (var board in usersboards[email])
                {
                    if (board.BoardName.Equals(boardName))
                    {
                        Task t = getTask(email, boardName, columnOrdinal, taskID);
                        if (usersboards[emailAssignee].Contains(board))
                        {
                            if (t.assignee.Equals("unassigned"))
                            {
                                t.assignee = emailAssignee;
                                t.convrtToDTO().Assignee = emailAssignee;
                                return;
                            }
                            else
                            {
                                if (t.assignee.Equals(email))
                                {
                                    t.assignee = emailAssignee;
                                    t.convrtToDTO().Assignee = emailAssignee;
                                    return;
                                }
                                throw new ArgumentException("this user is not the assignee fot this task");

                            }
                        }
                        throw new ArgumentException("the user with emailAssignee is not a member of this board!");
                    }
                }
                throw new ArgumentException("this user is not a member in this board!");
            }
        }
        /// <summary>
        /// returns board's ID
        /// </summary>
        /// <param name="boardId"> board's ID</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GetBoardName(int boardId)
        {
            
            if (boards.ContainsKey(boardId))
            {
                 return boards[boardId].BoardName;
            }
            log.Warn("board does not exists");
            throw new ArgumentException("there is no board with this id");
        }

        /// <summary>
        /// this method is for transfering the owner of a board
        /// </summary>
        /// <param name="currentOwnerEmail">the current owner's email</param>
        /// <param name="newOwnerEmail"> the new owner's mail</param>
        /// <param name="boardName">the board who has a new owner</param>
        /// <exception cref="ArgumentException"></exception>
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            checkBoardName(boardName);
            checkStatus(currentOwnerEmail); checkStatus(newOwnerEmail);
            if(checkMail(currentOwnerEmail) && checkMail(newOwnerEmail))
            {
                List<board> boards = new List<board>();
                foreach(var board in usersboards[currentOwnerEmail])
                {
                    if (board.BoardName.Equals(boardName))
                    {
                        boards.Add(board);
                    }
                }
                if(boards.Count == 0) { throw new ArgumentException("this user is not the owner of this board"); }
                foreach(var b in boards)
                {
                    if(b.UserEmail.Equals(currentOwnerEmail)) { b.UserEmail = newOwnerEmail; b.convrtToDTO().UserEmail = newOwnerEmail; return; }
                }
                throw new ArgumentException("this user is not the owner for this board!");
            }
        }
        
        /// <summary>
        /// This method adds a new user to the system
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddUser(String email)
        {
                usersboards.Add(email, new List<board>());
                usersStatus.Add(email, true);
                log.Info("usser added successfult");
        }

        /// <summary>
        ///  This method validates an email address format
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>true or false</returns>
        private bool ValidateEmail(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// This method checks if an email exists in the system
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>true or false </returns>
        /// <exception cref="ArgumentException"></exception>
        public bool checkMail(string email)
        {
            if (!ValidateEmail(email))
            {   
                log.Warn("not valid email");
                throw new ArgumentException("email is not valid !");
            }
            else if (!usersboards.ContainsKey(email))
            {
                log.Warn("user not exist!");
                throw new ArgumentException("There is no user with this email !");
            }
            return true;
            
        }

        /// <summary>
        /// Returns a specific task from the given user's board with the specified name and column ordinal.
        /// </summary>
        /// <param name="email">The email of the user who owns the board.</param>
        /// <param name="boardName">The name of the board containing the task.</param>
        /// <param name="columnOrdinal">The ordinal of the column containing the task.</param>
        /// <param name="taskId">The ID of the task to retrieve.</param>
        /// <returns>The task with the specified ID, or null if no such task exists.</returns>
        public Task getTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            List<board> boards = usersboards[email];
            foreach(var board in boards) 
            {
                if (board.BoardName.Equals(boardName))
                {
                    column col = board.getCol(columnOrdinal);
                    foreach(var task in col.getcolumn)
                    {
                        if(task.getTaskID == taskId)
                        {
                            return task;
                        }
                    }
                }
            }
            throw new ArgumentException("improper task id!");
        }

        /// <summary>
        /// Checks if the given board name is valid. Throws an ArgumentException if the name is null or empty.
        /// </summary>
        /// <param name="boardName">The name of the board to be checked.</param>
        /// <returns>Returns true if the board name is valid.</returns>
        public bool checkBoardName(string boardName)
        {
            if (String.IsNullOrEmpty(boardName))
            {
                log.Warn("invalid boardname");
                throw new ArgumentException("board name is not valid!");
            }
            return true;
        }

        /// <summary>
        /// Checks the status of the user with the given email.
        /// </summary>
        /// <param name="email">The email of the user to check.</param>
        /// <returns>True if the user is registered and logged in, false otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if the user is not registered or is logged out.</exception>
        public bool checkStatus(string email)
        {
            if (!usersStatus.ContainsKey(email))
            {
                log.Warn("user nor exist!");
                throw new ArgumentException("user not registed!");
            }
            if (!usersStatus[email])
            {
                log.Warn("user already logget out!");
                throw new ArgumentException("user have been logged out!");
            }
            return true;
        }
        /// <summary>
        /// this function checks if the columnordinal is valid 
        /// </summary>
        /// <param name="a">presents the ordinal num</param>
        /// <returns>false if the number not valid</returns>
        /// <exception cref="ArgumentException">if the ordinal not valid</exception>
        public bool checkOrd(int a)
        {
            if ( a < 0 || a > 2)
            {
                throw new ArgumentException("columnOrdinal is not valid!");
            }
            else if (a == 2)
            {
                throw new ArgumentException("cannot change tasks that have been done!");
            }
            return false;
        }

        /// <summary>
        /// this function checks the task validity
        /// </summary>
        /// <param name="task"> task variable</param>
        /// <returns> true if the task are valid</returns>
        public bool checktask(Task task)
        {
            task.checkDescription(task.getDescription);
            task.checkDueDate(task.getDuedate);
            task.checkTitle(task.getTitle);
            return true;
        }
    }
}
