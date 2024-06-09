using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.BackendTest;
using System.Net.NetworkInformation;

namespace IntroSE.Kanban.Backend.BackendTest
{
    public class UserTest
    {


        private userService userService;

        public UserTest()
        {
            this.userService = new userService();
        }

        public UserTest(userService userService)
        {
            this.userService = userService;
        }
        public void runAllTests()
        {
            RegisterTests();
            loginTest();
            logoutTest();
        }

        /// <summary>
        /// This is a test function for Register.
        /// </summary>
        public void RegisterTests()
        {
            // email should be Registed successfuly
            String json = userService.Register("maya@gmail.com", "Coop123");
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The user registerd successfully");
            }

            // if attempted to regist with an already exist email 
            String json2 = userService.Register("maya@gmail.com", "Coop756");
            Response res2 = JsonSerializer.Deserialize<Response>(json2);
            
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - existing user email registed again!");
            }

            // if the email was null
            String json3 = userService.Register(null, "Coop123");
            Response res3 = JsonSerializer.Deserialize<Response>(json3);
            
            if (res3.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - registed with a (null) email");
            }

            // without uppercase letter in the password
            String json4 = userService.Register("Keren@gmail.com", "coop123");
            Response res4 = JsonSerializer.Deserialize<Response>(json4);
            
            if (res4.ErrorOccured)
            {
                Console.WriteLine(res4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - The user registerd with Password without uppercase letter");
            }

            //without lowercase letter in the password
            String json5 = userService.Register("danial@gmail.com", "COOP123");
            Response res5 = JsonSerializer.Deserialize<Response>(json5);
            
            if (res5.ErrorOccured)
            {
                Console.WriteLine(res5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - The user registerd with Password whithout lowercase letter");
            }

            // shorter password than required
            String json6 = userService.Register("ibraheem@gmail.com", "Coop1");
            Response res6 = JsonSerializer.Deserialize<Response>(json6);
           
            if (res6.ErrorOccured)
            {
                Console.WriteLine(res6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - The user registerd with Password shorter than requierd");
            }

            //longer password than requierd
            String json7 = userService.Register("mohammad@gmail.com", "Coop12322jvndkfmjdkxllii999");
            Response res7 = JsonSerializer.Deserialize<Response>(json7);
            
            if (res7.ErrorOccured)
            {
                Console.WriteLine(res7.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - The user registerd with Password longer than requierd");
            }

            // password without number
            String json8 = userService.Register("maya@gmail.com", "Coopttu");
            Response res8 = JsonSerializer.Deserialize<Response>(json8);
            
            if (res8.ErrorOccured)
            {
                Console.WriteLine(res8.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - The user registerd with Password whithout a number");
            }
        }

        /// <summary>
        /// This function test the logIn
        /// </summary>
        public void loginTest()
        {
            //set up
            userService.Register("kereren@gmail.com", "Avvv123");
            userService.Logout("kereren@gmail.com");
            // this function should print "logIn successfuly"
            String json = userService.Login("kereren@gmail.com", "Avvv123");
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The user logged in successfully");
            }
            // trying to login for an logged in account
            String jsn = userService.Login("maya@gmail.com", "Bhedutj765");
            Response rs = JsonSerializer.Deserialize<Response>(jsn);
            if (rs.ErrorOccured)
            {
                Console.WriteLine(rs.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - The user already logged in");
            }
            // password is not correct for this specific email
            String json2 = userService.Login("maya@gmail.com", "Bhedutj765");
            Response res2 = JsonSerializer.Deserialize<Response>(json2);
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - The user logged whith incorrect Pass");
            }


            // email is not registed in the system 
            String json3 = userService.Login("cristiano@gmail.com", "Coop123");
            Response res3 = JsonSerializer.Deserialize<Response>(json3);
            if (res3.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Test failed - The user logged in whithout registering");
            }
        }
        /// <summary>
        /// This function tests the logout
        /// </summary>
        public void logoutTest()
        {
            // it should print "logged out successfuly"
            String json = userService.Logout("maya@gmail.com");
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("The user logged out successfully");
            }
        }

        public static void main(String[] args)
        {
            ServiceHup s = new ServiceHup();
            s.Register("mohamd@gmail.com", "Annn1234");
        }
    }
}
