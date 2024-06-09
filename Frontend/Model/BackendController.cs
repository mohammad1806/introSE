using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using System.Text.Json.Serialization;




namespace Frontend.Model
{
    public class BackendController
    {
        private ServiceHup ServiceHub { get; set; }

        public BackendController()
        {
            ServiceHub = new ServiceHup();
            ServiceHub.LoadData();
        }

        public UserModel Register(string username, string password)
        {
            string json = ServiceHub.Register(username, password);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(this, username);
        }

        public UserModel Login(string username, string password)
        {
            string json = ServiceHub.Login(username, password);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(this, username);
        }

        public void Logout(string email)
        {
            string json = ServiceHub.Logout(email);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }

        public string getBoardName(int brdid)
        {
            string json = ServiceHub.GetBoardName(brdid);
            Response<string> res = JsonSerializer.Deserialize<Response<string>>(json);
            string boardName = res.Value;
            return boardName;
        }

        public List<int> getUsersBoardsIds(string email)
        {
            IReadOnlyCollection<int> boardsids;
            string json = ServiceHub.GetUserBoards(email);
            Response<int[]> res = JsonSerializer.Deserialize<Response<int[]>>(json);
            boardsids = res.Value;
            if (boardsids != null)
            {
                return new List<int>(boardsids);
            }
            return new List<int>();
            
        }

        public IntroSE.Kanban.Backend.ServiceLayer.TaskToSend[] GetColumn(string email, int brdId, int Ordinal)
        {
            try
            {
                string boardName = this.getBoardName(brdId);
                string json = ServiceHub.GetColumn(email, boardName, Ordinal);
                Response<IntroSE.Kanban.Backend.ServiceLayer.TaskToSend[]> res = JsonSerializer.Deserialize<Response<IntroSE.Kanban.Backend.ServiceLayer.TaskToSend[]>>(json);
                return res.Value;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}