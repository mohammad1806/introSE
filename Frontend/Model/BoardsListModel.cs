using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Frontend.Model
{
    public class BoardsListModel : NotifiableModelObject
    {
        public ObservableCollection<BoardModel> Boards { get ; set; }
        private UserModel _user;


        public BoardsListModel(BackendController controller, ObservableCollection<BoardModel> boards) : base(controller)
        {
            this.Boards = boards;
            Boards.CollectionChanged += HandleChange;

        }
        public BoardsListModel(BackendController controller, UserModel user) : base(controller)
        {
            _user = user;
            Boards = new ObservableCollection<BoardModel>();
            List<int> IDs = controller.getUsersBoardsIds(user.Email);
            foreach (int id in IDs)
            {
                Boards.Add(new BoardModel(controller, id.ToString(), controller.getBoardName(id), _user.Email));
            }
            Boards.CollectionChanged += HandleChange;

        }
        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {

            }
        }
    }
}