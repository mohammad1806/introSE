using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {

        public TaskModel(BackendController controller, int id, string title, string desc, string assigne, DateTime crtime, DateTime duodate) : base(controller)
        {
            ID = id;
            Title = title;
            Description = desc;
            Assignee = assigne;
            CreationTime = crtime;
            DueDate = duodate;
        }
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("ID");
            }
        }

        private DateTime _creationtime;
        public DateTime CreationTime
        {
            get { return _creationtime; }
            set
            {
                _creationtime = value;
                RaisePropertyChanged("CreationTime");
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }


        private string _assignee;
        public string Assignee
        {
            get { return _assignee; }
            set
            {
                _assignee = value;
                RaisePropertyChanged("Assignee");
            }
        }

        private DateTime _duedate;
        public DateTime DueDate
        {
            get { return _duedate; }
            set
            {
                _duedate = value;
                RaisePropertyChanged("DueDate");
            }
        }
    }
}