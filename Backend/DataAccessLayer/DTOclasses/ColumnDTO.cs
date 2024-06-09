using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    public class ColumnDTO
    {
        public const string BoardIDColumnName = "BoardID";
        public const string ColumnNameColumnName = "ColumnName";
        public const string ColumnOrdinalColumnName = "ColumnOrdinal";
        public const string ColumnLimitColumnName = "ColumnLimit";

        public ColumnDALcontroller ColumnDALcontroller { get; }

        private int _brdID;
        private int _columnOrdinal;
        private string _columnName;
        private int _columnLimit;

        public ColumnDTO(int brdID,  string columnName,int colLimit ,int columnOrdinal )
        {
            ColumnDALcontroller = new ColumnDALcontroller();
            this._brdID = brdID;
            this._columnOrdinal = columnOrdinal;
            this._columnName = columnName;
            this._columnLimit = colLimit;
        }

        public int BrdID { get { return _brdID; } set { _brdID = value; } }
        public int ColumnOrdinal { get {  return _columnOrdinal; } set {  _columnOrdinal = value; } }
        public string ColumnName { get { return _columnName; } set { _columnName = value; } }
        public int ColumnLimit { get {  return _columnLimit; } set {  _columnLimit = value; ColumnDALcontroller.Update(_brdID, _columnOrdinal, ColumnLimitColumnName, value); } }
    }
}
