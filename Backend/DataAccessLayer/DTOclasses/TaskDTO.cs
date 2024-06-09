using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    public class TaskDTO
    {
        public const string TaskIDColumnName = "TaskID";
        public const string BoardIDColumnName = "BoardID";
        public const string TitleColumnName = "Title";
        public const string DescriptionColumnName = "Description";
        public const string CreationTimeColumnName = "creationTime";
        public const string DueDateColumnName = "DueDate";
        public const string AssigneeColumnName = "Assignee";
        public const string ColumnOrdinalColumnName = "ColumnOrdinal";


        public TaskDALcontroller TaskDALcontroller { get; }

        private int _taskid;
        private int _boardID;
        private string _title;
        private string _description;
        private DateTime _creationTime;
        private DateTime _duedate;
        private string _assignee;
        private int _colOrd;

        public TaskDTO(int tskid, int brdid, string title, string description, DateTime creationTime, DateTime duedate, int colOrd,string assignee)
        {
            TaskDALcontroller = new TaskDALcontroller();
            this._taskid = tskid;
            this._boardID = brdid;
            this._title = title;
            this._description = description;
            this._creationTime = creationTime;
            this._duedate = duedate;
            this._colOrd = colOrd;
            this._assignee = assignee;

        }

        public int ColOrd { get { return _colOrd; } set { _colOrd = value; TaskDALcontroller.Update(_taskid, _boardID, ColumnOrdinalColumnName, value.ToString()); } }
        public int TaskID { get { return _taskid; } set { _taskid = value; } }
        public int BoardID { get { return _boardID; } set { _boardID = value; } }
        public string Title { get { return _title; } set { _title = value; TaskDALcontroller.Update(_taskid, _boardID, TitleColumnName, value); } }
        public string Description { get { return _description; } set { _description = value; TaskDALcontroller.Update(_taskid, _boardID, DescriptionColumnName, value); } }
        public DateTime CreationTime { get { return _creationTime; } set { _creationTime = value; } }
        public DateTime DueDate { get { return _duedate; } set { _duedate = value; TaskDALcontroller.Update(_taskid, _boardID, DueDateColumnName, value.ToString()); } }
        public String Assignee { get { return _assignee; } set { _assignee = value; TaskDALcontroller.Update(_taskid, _boardID, AssigneeColumnName, value); } }

    }

    
}
