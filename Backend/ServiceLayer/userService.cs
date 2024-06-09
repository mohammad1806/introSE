using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.businessLayer.board;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class userService
    {

        private UserController UController;

        public userService()
        {
            UController = new UserController();
        }

        public userService(userService other)
        {
            this.UController = other.UController;
        }

        public UserController getCon()
        {
            return UController;
        }

        public string loadData()
        {
            Response res;
            String Json;
            try
            {
                UController.loadData();
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
                UController.DeleteData();
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
        /// Registers a new user with the given email and password.
        /// </summary>
        /// <param name="email">The email of the user to register.</param>
        /// <param name="password">The password of the user to register.</param>
        /// <returns>A JSON string representing the response from the server.</returns>
        public string Register(string email, string password)
        {
            Response res;
            String Json;
            try
            {
                UController.Register(email, password);
                res = new Response();
                
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);   
            }
            Json = JsonSerializer.Serialize<Response>(res);
            return Json;
        }                                                               

        /// <summary>
        /// Logs in the user with the given email and password.
        /// </summary>
        /// <param name="email">The email of the user to log in.</param>
        /// <param name="password">The password of the user to log in.</param>
        /// <returns>A JSON string representing the response from the server.</returns>
        public string Login(string email, string password)
        {
            Response res = new Response();
            String Json;
            try
            {
                String e = UController.Login(email, password);
                res = new Response(null,e);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// Logs out the user with the given email.
        /// </summary>
        /// <param name="email">The email of the user to log out.</param>
        /// <returns>A JSON string representing the response from the server.</returns>
        public string Logout(string email)
        {
            Response res = new Response();
            String Json;
            try
            {
                UController.Logout(email);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            Json = JsonSerializer.Serialize(res);
            return Json;
        }

        /// <summary>
        /// Changes the password of the user with the given email.
        /// </summary>
        /// <param name="email">The email of the user to change the password for.</param>
        /// <param name="password">The current password of the user.</param>
        /// <param name="newPass">The new password to set for the user.</param>
        /// <returns>A JSON string representing the response from the server.</returns>
        public string changePassword(string email, string Password, string newPass)
        {
            Response res = new Response();
            String Json;
            try
            {
                UController.changePassword(email, Password, newPass);
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

