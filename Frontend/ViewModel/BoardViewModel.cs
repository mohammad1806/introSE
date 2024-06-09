using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        private readonly UserModel user;

        public ColumnsModel BoardColumns { get; private set; }

        private string _title;
        private UserModel u;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }
        public BoardViewModel(UserModel user, string boardname, int id)
        {
            this.user = user;
            this.controller = user.Controller;
            Title = boardname;
            BoardColumns = user.GetColumn(id);
        }

        public BoardViewModel(UserModel u)
        {
            this.u = u;
        }


    }
}