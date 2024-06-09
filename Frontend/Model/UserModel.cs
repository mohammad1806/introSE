using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public BoardsListModel GetBoards()
        {
            return new BoardsListModel(Controller, this);
        }
        public ColumnsModel GetColumn(int brdId)
        {
            return new ColumnsModel(Controller, this, brdId);
        }

        public UserModel(BackendController controller, string email) : base(controller)
        {
            _email = "";
            this.Email = email;
        }
    }
}