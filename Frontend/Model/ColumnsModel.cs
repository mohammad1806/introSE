using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class ColumnsModel : NotifiableModelObject
    {
        private UserModel _user;

        public ObservableCollection<TaskModel> column0 { get; set; }
        public ObservableCollection<TaskModel> column1 { get; set; }
        public ObservableCollection<TaskModel> column2 { get; set; }

        public ColumnsModel(BackendController controller, UserModel user, int brdid) : base(controller)
        {
            _user = user;
            column0 = new ObservableCollection<TaskModel>();
            column1 = new ObservableCollection<TaskModel>();
            column2 = new ObservableCollection<TaskModel>();

            int i = 0;
            while (i < 3)
            {
                IntroSE.Kanban.Backend.ServiceLayer.TaskToSend[] TaskArray = controller.GetColumn(_user.Email, brdid, i);

                for (int j = 0; j < TaskArray.Length; j++)
                {
                    IntroSE.Kanban.Backend.ServiceLayer.TaskToSend t = TaskArray[j];
                    if (i == 0)
                    {
                        column0.Add(new TaskModel(controller, t.Id, t.Title, t.Description, t.Assignee, t.CreationTime, t.DueDate));
                    }
                    else if (i == 1)
                    {
                        column1.Add(new TaskModel(controller, t.Id, t.Title, t.Description, t.Assignee, t.CreationTime, t.DueDate));
                    }
                    else
                    {
                        column2.Add(new TaskModel(controller, t.Id, t.Title, t.Description, t.Assignee, t.CreationTime, t.DueDate));
                    }
                }
                i++;
            }
        }


    }
}