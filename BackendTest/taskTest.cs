using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTest;

public class TasksTest
{
    /// <summary>
    /// This class is used to test the taskservice class functionality.
    /// </summary>

    private TasksService tasksService;

    public TasksTest()
    {
        this.tasksService = new TasksService();
    }

    public TasksTest(TasksService tasksService)
    {
        this.tasksService = tasksService;
    }
    public void runAllTests()
    {
        AddTaskTest();
        UpdateTaskDescriptionTest();
        UpdateTaskDueDateTest();
        UpdateTaskTitleTest();
        AdvanceTaskTest();
        InProgressTasksTest();
    }

    /// <summary>
    /// this function tests the AddTask function with multiple inputs
    /// </summary>
    public void AddTaskTest()
    {

        // this should print the task added successfuly
        String json = tasksService.AddTask("ahmad@gmail.com", "myBoard", "work duties", "I must finish the project", new DateTime(2023, 4, 20));
        Response res = JsonSerializer.Deserialize<Response>(json);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine(" The task added successfuly");
        }
        //adding task with exp date
        String json2 = tasksService.AddTask("ahmad@gmail.com", "myBoard", "work duties", "I must finish the project", new DateTime(2023, 1, 2));
        Response res2 = JsonSerializer.Deserialize<Response>(json);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - The task added with exp due date");
        }
        //adding task with an empty title
        String json3 = tasksService.AddTask("ahmad@gmail.com", "myBoard", "", "I must finish the project", new DateTime(2023, 4, 20));
        Response res3 = JsonSerializer.Deserialize<Response>(json);
        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - The task added with an empty title");
        }
        //task title longer than 50 character
        String json4 = tasksService.AddTask("ahmad@gmail.com", "myBoard", "work dutiessssssssssssssssssssssssssssssssssssssssssssssssssssssssssss", "I must finish the project", new DateTime(2023, 4, 20));
        Response res4 = JsonSerializer.Deserialize<Response>(json);
        if (res4.ErrorOccured)
        {
            Console.WriteLine(res4.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - The task added with longer than 50 character title");
        }
        // task description longer than 300 character
        String json5 = tasksService.AddTask("ahmad@gmail.com", "myBoard", "work duties", "I must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projecttttttI must finish the projectttttt", new DateTime(2023, 4, 20));
        Response res5 = JsonSerializer.Deserialize<Response>(json);
        if (res5.ErrorOccured)
        {
            Console.WriteLine(res5.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - The task added with longer than 300 character desc");
        }
        // with empty description should print the task added successfuly
        String json6 = tasksService.AddTask("ahmad@gmail.com", "myBoard", "work duties", "", new DateTime(2023, 4, 20));
        Response res6 = JsonSerializer.Deserialize<Response>(json);
        if (res6.ErrorOccured)
        {
            Console.WriteLine(res6.ErrorMessage);
        }
        else
        {
            Console.WriteLine("The task added successfuly");
        }
    }
    /// <summary>
    /// this function tests the UpdateTaskDueDate function
    /// </summary>
    public void UpdateTaskDueDateTest()
    {

        // thid should print Task duedate updated successfuly
        String json = tasksService.UpdateTaskDueDate("ahmad@gmail.com", "myBoard", 0, 1, new DateTime(2023, 4, 30));
        Response res = JsonSerializer.Deserialize<Response>(json);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Task duedate updated successfuly");
        }
        //trying to update the date with a passed date
        String json2 = tasksService.UpdateTaskDueDate("ahmad@gmail.com", "myBoard", 0, 1, new DateTime(2023, 1, 30));
        Response res2 = JsonSerializer.Deserialize<Response>(json);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - Task duedate updated to an expierd date");
        }
        // trying to update the duedate for a task that already done
        String json3 = tasksService.UpdateTaskDueDate("ahmad@gmail.com", "myBoard", 2, 1, new DateTime(2023, 4, 30));
        Response res3 = JsonSerializer.Deserialize<Response>(json);
        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - Task duedate updated for a task that already been done");
        }
    }
    /// <summary>
    /// this function test the UpdateTaskTitle function
    /// </summary>
    public void UpdateTaskTitleTest()
    {
        //this should print Task title updated successfuly
        String json = tasksService.UpdateTaskTitle("ahmad@gmail.com", "myBoard", 0, 1, "jobduties");
        Response res = JsonSerializer.Deserialize<Response>(json);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Task title updated successfuly");
        }
        // trying to update the task title with an empty title
        String json2 = tasksService.UpdateTaskTitle("ahmad@gmail.com", "myBoard", 0, 1, "");
        Response res2 = JsonSerializer.Deserialize<Response>(json);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - Task title updated to an empty title");
        }
        //trying to put a title that is longer than 50 character 
        String json3 = tasksService.UpdateTaskTitle("ahmad@gmail.com", "myBoard", 0, 1, "jobdutiessssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss");
        Response res3 = JsonSerializer.Deserialize<Response>(json);
        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - Task title updated to a longer than 50 character title");
        }
        //updating the title to a task that already done
        String json4 = tasksService.UpdateTaskTitle("ahmad@gmail.com", "myBoard", 2, 1, "jobduties");
        Response res4 = JsonSerializer.Deserialize<Response>(json);
        if (res4.ErrorOccured)
        {
            Console.WriteLine(res4.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - Task title updated fot a task that already been done");
        }
    }
    /// <summary>
    /// this function test the UpdateTaskDescription function
    /// </summary>
    public void UpdateTaskDescriptionTest()
    {
        // this should print Task description updated successfuly
        String json = tasksService.UpdateTaskDescription("ahmad@gmail.com", "myBoard", 0, 1, "changing the plan");
        Response res = JsonSerializer.Deserialize<Response>(json);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Task description updated successfuly");
        }
        //tring to put a description that is longer than 300 character
        String json2 = tasksService.UpdateTaskDescription("ahmad@gmail.com", "myBoard", 0, 1, "changing the plannnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnvvnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
        Response res2 = JsonSerializer.Deserialize<Response>(json);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - Task description updated to a longer than 300 desc");
        }
        //updating the description with an empty one that should print Task description updated successfuly
        String json3 = tasksService.UpdateTaskDescription("ahmad@gmail.com", "myBoard", 0, 1, "");
        Response res3 = JsonSerializer.Deserialize<Response>(json);
        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Task description updated successfuly");
        }
        //trying to update the description for a task that is already done
        String json4 = tasksService.UpdateTaskDescription("ahmad@gmail.com", "myBoard", 2, 1, "changing the plan");
        Response res4 = JsonSerializer.Deserialize<Response>(json);
        if (res4.ErrorOccured)
        {
            Console.WriteLine(res4.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - Task description updated for a task that already been done");
        }
    }
    /// <summary>
    /// this function test the AdvanceTask function
    /// </summary>
    public void AdvanceTaskTest()
    {

        // this should print Task advanced successfuly
        String json = tasksService.AdvanceTask("ahmad.@gmail.com", "myBoard", 0, 1);
        Response res = JsonSerializer.Deserialize<Response>(json);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Task advanced successfuly");
        }
        //trying to advance a task that is already done
        String json2 = tasksService.AdvanceTask("ahmad.@gmail.com", "myBoard", 2, 1);
        Response res2 = JsonSerializer.Deserialize<Response>(json);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - Task advanced and it already been done");
        }
    }
    /// <summary>
    /// this function tests the InProgressTasks function
    /// </summary>
    public void InProgressTasksTest()
    {
        // this should print test pass (returns a list for all tasks that is inprogress)
        String json = tasksService.InProgressTasks("ahmad@gmail.com");
        Response res = JsonSerializer.Deserialize<Response>(json);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("test pass");
        }
        //trying to get the list of in progress tasks with an empty email
        String json2 = tasksService.InProgressTasks("");
        Response res2 = JsonSerializer.Deserialize<Response>(json);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - returns inprogress tasks for an empty email");
        }
    }

    public void AssignTaskTest()
    {
        // this should print Task assignment succeeded
        String json = tasksService.AssignTask("daniel@gmail.com", "danielBoard", 1, 1,"mhmd@gmail.com");
        Response res = JsonSerializer.Deserialize<Response>(json);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Task assignment succeeded");
        }

        //this should throw an exception attempt to assign task to a user who is not a board member
        String json2 = tasksService.AssignTask("daniel@gmail.com", "danielBoard", 1, 1, "unknown@gmail.com");
        Response res2 = JsonSerializer.Deserialize<Response>(json2);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - attempt to assign taskt to a user who is not a board member");
        }

        //this should throw an exception attempt to assign Taske by a person who is not a member 
        String json3 = tasksService.AssignTask("unknown@gmail.com", "danielBoard", 1, 1, "daniel@gmail.com");
        Response res3 = JsonSerializer.Deserialize<Response>(json3);
        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - attempt to assign Taske by a person who is not a member");
        }

        //this should throw an exception board does not exist
        String json4 = tasksService.AssignTask("keren@gmail.com", "unknown", 1, 1, "mhmd@gmail.com");
        Response res4 = JsonSerializer.Deserialize<Response>(json4);
        if (res4.ErrorOccured)
        {
            Console.WriteLine(res4.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - board does not exist");
        }

        //this should throw an exception task does not exist
        String json5 = tasksService.AssignTask("keren@gmail.com", "board", 1, 100, "mhmd@gmail.com");
        Response res5 = JsonSerializer.Deserialize<Response>(json5);
        if (res5.ErrorOccured)
        {
            Console.WriteLine(res5.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - board does not exist");
        }


    }


}
