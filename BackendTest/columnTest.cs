using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTest
{
    public class columnTest
    {
        /// <summary>
        /// this class test the column class functionality
        /// </summary>
        private columnService columnService;

        public columnTest()
        {
            this.columnService = new columnService();
        }
     
        public columnTest(columnService columnService)
        {
            this.columnService = columnService;
        }
        public void runAllTests()
        {

            LimitColumnTest();
            GetColumnLimitTest();
            GetColumnNameTest();
            GetColumnTest();

        }
        /// <summary>
        /// this function test LimitColumn function
        /// </summary>
        public void LimitColumnTest()
        {
            // this should print column limited successfuly
            String json = columnService.LimitColumn("ahmad4ever@gmail.com", "yousef", 0, 20);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column limited successfuly");
            }

        }
        /// <summary>
        /// this function test GetColumnLimit function
        /// </summary>
        public void GetColumnLimitTest()
        {
            // this should print returns column limit successfuly
            String json = columnService.GetColumnLimit("ahmad4ever@gmail.com", "yousef", 0);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("returns column limit successfuly");
            }
        }
        /// <summary>
        /// this function test GetColumnName function
        /// </summary>
        public void GetColumnNameTest()
        {
            // this should print column name returns successfuly
            String json = columnService.GetColumnName("ahmad4ever@gmail.com", "yousef", 0);
            Response response = JsonSerializer.Deserialize<Response>(json);
            if (response.ErrorOccured)
            {
                Console.WriteLine(response.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column name returns successfuly");
            }
        }
        /// <summary>
        /// this function test the GetColumn function
        /// </summary>
        public void GetColumnTest()
        {
            // this should proint column returns successfuly
            String json = columnService.GetColumn("ahamd4ever@gmail.com", "yousef", 0);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorOccured)
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column returns successfuly");
            }
        }
    }
}
