using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class BoardsListViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        private UserModel _user;
        public BoardsListModel userBoards { get; private set; }

        public BoardModel board { get; private set; }
        public BoardsListViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this._user = user;
            this.Title = "Boards of: " + user.Email;
            this.userBoards = user.GetBoards();
        }

        public string Title { get; private set; }

        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        public void Logout()
        {
            try
            {
                controller.Logout(_user.Email);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public int GetBrdId()
        {
            int id = int.Parse(SelectedBoard.Id.Substring(10));
            return id;
        }

        public string getBrdName()
        {
            string n = SelectedBoard.Name.Substring(12);
            return n;
        }
    }
}