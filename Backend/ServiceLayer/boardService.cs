using IntroSE.Kanban.Backend.businessLayer.board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Cache;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class boardService
  {
        private boardController Bcontroller;
        // constructor
        public boardService()
        {
            this.Bcontroller = new boardController();
        }
        // copy constructor
        public boardService(boardController other)
        {
            this.Bcontroller = other;
        }

        public boardController getCon()
        {
            return Bcontroller;
        }
        public string loadData()
        {
            Response res;
            String Json;
            try
            {
                Bcontroller.loadData();
                res = new Response();
            }
            catch (Exception e)
            {
                res = new Response(e.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        public string DeleteData()
        {
            Response res;
            string Json;
            try
            {
                Bcontroller.DeleteData();
                res = new Response();
            }
            catch (Exception e)
            {
                res = new Response(e.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }
        /// <summary>
        /// Creates a new board for the specified user with the given name.
        /// </summary>
        /// <param name="email">The email address of the user who will own the board.</param>
        /// <param name="name">The name of the board to be created.</param>
        /// <returns>A JSON string representation of the Response object containing the result of the operation.</returns>
        public string CreateBoard(string email, string name)
    {
            Response res = new Response();
            String Json;
            try
            {   
                Bcontroller.CreateBoard(email, name);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
                Json = JsonSerializer.Serialize(res);
                return Json;
    }
        /// <summary>
        /// Deletes the board with the specified name owned by the specified user.
        /// </summary>
        /// <param name="email">The email address of the user who owns the board.</param>
        /// <param name="name">The name of the board to be deleted.</param>
        /// <returns>A JSON string representation of the Response object containing the result of the operation.</returns>
        public string DeleteBoard(string email, string name)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.DeleteBoard(email, name);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }
        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs</returns>
        public string GetUserBoards(string email)
        {
            Response<int[]> res ;
            String Json;
            try
            {
                List<int> usersBoards = Bcontroller.GetUserBoards(email);
                int[] ids = usersBoards.ToArray();
                res = new Response<int[]>(ids);
            }
            catch (Exception ex)
            {
                res = new Response<int[]>(null,ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string JoinBoard(string email, int boardID)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.JoinBoard(email,boardID);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string LeaveBoard(string email, int boardID)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.LeaveBoard(email,boardID);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }
        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs</returns>
        public string GetBoardName(int boardId)
        {
            Response<string> res;
            String Json;
            try
            {
                string boardName = Bcontroller.GetBoardName(boardId);
                res = new Response<string>(boardName,null);
            }
            catch (Exception ex)
            {
                res = new Response<string>(null,ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            Response res = new Response();
            String Json;
            try
            {
                Bcontroller.TransferOwnership(currentOwnerEmail,newOwnerEmail,boardName);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }
    }
}
