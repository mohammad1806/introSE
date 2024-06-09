using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.businessLayer.board
{
    public class column
    {

        private int brdID;
        private int columnOrdinal;
        private string columnName;
        private List<Task> tasks;
        private int columnLimit;

        private ColumnDALcontroller CDALcon;
        private TaskDALcontroller TDALcon;

        //constructor
        public column(string name, int columnOrdinal, int id)
        {
            CDALcon = new ColumnDALcontroller();
            TDALcon = new TaskDALcontroller();
            this.brdID = id;
            columnName = name;
            this.columnOrdinal = columnOrdinal;
            tasks = new List<Task>();
            columnLimit = -1;
        }

        public column(ColumnDTO other)
        {
            this.brdID = other.BrdID;
            this.columnOrdinal = other.ColumnOrdinal;
            this.columnName = other.ColumnName;
            tasks = new List<Task>();
            this.columnLimit = other.ColumnLimit;
        }

        public ColumnDTO convrtToDTO()
        {
            return new ColumnDTO(brdID, columnName, columnLimit, columnOrdinal);
        }


        public void loadColumnsData()
        {
            columnLimit = CDALcon.getColumnlimit(brdID, columnOrdinal);
            List<TaskDTO> tsks =   TDALcon.listTasksForClumn(brdID,columnOrdinal);
            foreach (TaskDTO t in tsks) 
            {
                tasks.Add(new Task(t));
            }
        }
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <returns>The name of the column.</returns>
        public string getColumnName
        {
            get { return columnName; }
        }

        /// <summary>
        /// Gets the limit of the column.
        /// </summary>
        /// <returns>The limit of the column.</returns>
        public int ColumnLimit
        {
            get { return columnLimit; }
            set { columnLimit = value; }
        }

        /// <summary>
        /// Gets the list of tasks in the column.
        /// </summary>
        public List<Task> getcolumn
        {
            get { return tasks; }
        }

        /// <summary>
        /// Get the column ordinal of the current column object
        /// </summary>
        public int getColummnOrdinal
        {
            get { return  columnOrdinal; } 
        }

        /// <summary>
        /// Limits the number of tasks that can be added to the column.
        /// </summary>
        /// <param name="limit">The maximum number of tasks allowed in the column.</param>
        /// <exception cref="ArgumentException">Thrown when an invalid limit is provided.</exception>
        /// <exception cref="Exception">Thrown when the column already has more tasks than the new limit.</exception>

        public void LimitColumn(int limit)
        {
            if (columnLimit< -1) 
            {
                throw new ArgumentException("not valid limit !");
            }
            else if ( limit < tasks.Count) 
            {
                throw new Exception(" the column contains tasks more than limit!!!");
            }
            else
            {
                columnLimit = limit;
                convrtToDTO().ColumnLimit=limit;
            }
        }
        /// <summary>
        /// adding a new task
        /// </summary>
        /// <param name="task">the task we want to add</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddTask(Task task)
        {
            if (tasks.Count == columnLimit && columnLimit != -1)
                throw new ArgumentException("this column reaches his limit!");
            tasks.Add(task);
            TDALcon.AddTask(task.convrtToDTO());
        }
    }
}
