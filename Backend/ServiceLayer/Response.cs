using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    ///<summary>Class <c>Response</c> represents the result of a call to a void function. 
    ///If an exception was thrown, <c>ErrorOccured = true</c> and <c>ErrorMessage != null</c>. 
    ///Otherwise, <c>ErrorOccured = false</c> and <c>ErrorMessage = null</c>.</summary>
    public class Response
    {
        public  string ErrorMessage { get; set; }
        public Object ReturnValue { get; set; }

        /// <summary>
        /// Gets or sets the error message associated with the method call.
        /// </summary> 
        public bool ErrorOccured { get => ErrorMessage != null; }

        /// <summary>
        /// Initializes a new instance of the Response class.
        /// </summary>
        public Response() { }

        public Response(string ErrorMessage, Object toReturn)
        {
            this.ErrorMessage = ErrorMessage;
            this.ReturnValue = toReturn;
        }

        /// <summary>
        /// Initializes a new instance of the Response class with an error message.
        /// </summary>
        /// <param name="msg">The error message to be associated with the Response.</param>
        public Response(string msg)
        {
            this.ErrorMessage = msg;
        }
        
        public Response(object toReturn)
        {
            this.ReturnValue = toReturn;
        }
    }
}
