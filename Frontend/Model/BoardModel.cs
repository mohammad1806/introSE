using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private string _id;
        private string _name;
        private string _email;

        public BoardModel(BackendController controller, string id, string name, string email) : base(controller)
        {
            _id = id;
            Id = id;
            _name = name;
            Name = name;
            _email = email;
        }

        public string Id
        {
            get { return _id; }
            set { _id = "Board ID :" + value; RaisePropertyChanged("Id"); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = "Board Name :" + value; RaisePropertyChanged("Name"); }
        }
    }
}