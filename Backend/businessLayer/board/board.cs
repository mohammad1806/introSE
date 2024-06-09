using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IntroSE.Kanban.Backend.businessLayer.board
{
    public class board
    {
        private int brdID { get; set; }
        private string boardName;
        private List<column> columns;
        private string userEmail;
        private int taskID;

        private ColumnDALcontroller CDALcon = new ColumnDALcontroller();


        //constructor
        public board(string email, string boardName, int ID)
        {
            this.brdID = ID;
            userEmail = email;
            this.boardName = boardName;
            columns = new List<column>();
            column col1 = new column("backlog", 0, ID);
            columns.Add(col1);
            CDALcon.addColumn(col1.convrtToDTO());
            column col2 = new column("in progress", 1, ID);
            columns.Add(col2);
            CDALcon.addColumn(col2.convrtToDTO());
            column col3 = new column("Done", 2, ID);
            columns.Add(col3);
            CDALcon.addColumn(col3.convrtToDTO());
            taskID = 0;
        }

       public board(BoardDTO board)
        {
            this.brdID = board.BrdID;
            this.boardName = board.BoardName;
            this.taskID = board.TaskID;
            this.columns = new List<column>();
            column col1 = new column("backlog", 0, brdID);
            columns.Add(col1);
            column col2 = new column("in progress", 1, brdID);
            columns.Add(col2);
            column col3 = new column("Done", 2, brdID);
            columns.Add(col3);
        }
        

        public BoardDTO convrtToDTO()
        {
            return new BoardDTO(brdID, boardName, userEmail, taskID);
        }


        public void loadBoardsData()
        {

        }
        /// getter and setter of Task ID
        public int getTaskID
        {
            get { return taskID; }
            set { taskID = value; }
        }

        /// getter and setter of Board Name
        public string BoardName 
        {
           get { return boardName; }
            set { boardName = value; }
          
        }

        /// getter and setter of User Email
        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        public List<column> Columns 
        {
            get { return columns; }
        }
        /// <summary>
        /// This method returns a column given its ordinal
        /// </summary>
        /// <param name="Ordinal">the Ordinal of the coulmn we want to get </param>
        /// <returns>coulmn by it's ordinal </returns>
        public column getCol(int Ordinal)
        {
            return columns[Ordinal];
        }

        /// <summary>
        /// This method checks if the Board name and the user's email are matching
        /// </summary>
        /// <param name="other">board</param>
        /// <returns>true or false </returns>
        public bool equal(board other)
        {
            return boardName.Equals(other.boardName) && userEmail.Equals(other.userEmail);
        }

        public int BrdID
        {
            get { return  brdID; }
            set { brdID = value; }
        }

        /// <summary>
        /// In this method we want to put a task in an already existed column
        /// 
        ///
        /// </summary>
        /// <param name="task">The task we want to add</param>
        /// <param name="ord">the column we want to add the task in</param>
        /// <exception cref="ArgumentException">if the number of clumns after the adding is higher than the limit</exception>
        public void AdvanceTask(Task task , int ord)
        {
            if (getCol(ord + 1).getcolumn.Count == getCol(ord + 1).ColumnLimit)
            {
                throw new ArgumentException("the column reachs his maximum number of tasks");
            }
            task.convrtToDTO().ColOrd++;
            getCol(ord).getcolumn.Remove(task);
            task.colOrd++;
            getCol(ord + 1).getcolumn.Add(task);
        }

        /// <summary>
        /// free all the tasks of a specific user
        /// </summary>
        /// <param name="email">the user's email</param>
        public void freeTheTasks(string email)
        {
            foreach (column col in Columns)
            {
                foreach (Task task in col.getcolumn)
                {
                    if (task.assignee.Equals(email)) { task.Assignee = "unassigned"; task.convrtToDTO().Assignee = "unassigned"; }
                }
            }
        }
    }
}
