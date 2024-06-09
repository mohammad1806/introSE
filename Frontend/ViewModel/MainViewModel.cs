using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Frontend.ViewModel
{
    class MainViewModel : NotifiableObject
    {
        public BackendController BackendController { get; private set; }

        public MainViewModel()
        {
            this.BackendController = new BackendController();
            this._username = "";
            this._password = "";
            this._message = "";
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                this._username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public UserModel Login()
        {
            Message = "";
            try
            {
                return BackendController.Login(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
        public UserModel Register()
        {
            Message = "";
            try
            {
                return BackendController.Register(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }


    }
}