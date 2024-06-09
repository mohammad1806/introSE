using IntroSE.Kanban.Backend.businessLayer.board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    public class BoardDTO
    {

        public const string BoardIDColumnName = "BoardID";
        public const string BoardNameColumnName = "BoardName";
        public const string TaskIDColumnName = "TaskID";
        public const string userEmailColumnName = "UserEmail";

        public BoardDALcontroller BoardDALcontroller { get; }

        private int _brdID;
        private string _boardName;
        private string _userEmail;
        private int _taskID;

        public BoardDTO(int brdID, string boardName, string userEmail, int taskID)
        {
            this.BoardDALcontroller = new BoardDALcontroller();
            this._brdID = brdID;
            this._boardName = boardName;
            this._userEmail = userEmail;
            this._taskID = taskID;
        }

        public int BrdID { get { return _brdID; } set { _brdID = value; } }
        public string BoardName { get { return _boardName; } set { _boardName = value;} }
        public string UserEmail { get { return _userEmail; } set { _userEmail = value; BoardDALcontroller.Update(_brdID, userEmailColumnName, value); } }
        public int TaskID { get { return _taskID; } set { _taskID = value; BoardDALcontroller.Update(_brdID, TaskIDColumnName, value.ToString()); } }

    }
}
