using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    public class UserDTO
    {
        public const string EmailColumnName = "Email";
        public const string PasswordColumnName = "Password";

        public UserDALcontroller UserDALcontroller { get; }

        private string _email;
        private string _password;
        private bool _status;

        public UserDTO(string email, string password,bool status)
        {
            UserDALcontroller = new UserDALcontroller();
            this._email = email;
            this._password = password;
            this._status = status;
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
            
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}
