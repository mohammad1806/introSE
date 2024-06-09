using IntroSE.Kanban.Backend.businessLayer.board;
using IntroSE.Kanban.Backend.BackendTest;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTest;

public class boardTest
{

    /// <summary>
    /// this class is used to test the board service class.
    /// </summary>
    private boardService boardService;

    public boardTest()
    {
        boardService = new boardService();
    }

    public boardTest(boardService boardService)
    {
        this.boardService = boardService;
    }
    public void runAllTests()
    {

        CreateBoardTest();
        DeleteBoardTest();


    }
    
    /// <summary>
    /// this function test the CreateBoard function
    /// </summary>
    public void CreateBoardTest()
    {

        // this should print board created successfuly
        String json = boardService.CreateBoard("maya@gmail.com", "kerenG");
        Response res = JsonSerializer.Deserialize<Response>(json);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("board created successfuly");
        }

        // creating a new board with the same name for the same user
        String json2 = boardService.CreateBoard("maya@gmail.com", "kerenG");
        Response res2 = JsonSerializer.Deserialize<Response>(json);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("board created with an existing name");
        }

        // creating a new board with the same name but for another user, the function should print board created successfuly. 
        String json3 = boardService.CreateBoard("ahmad4ever@gmail.com", "yousef");
        Response res3 = JsonSerializer.Deserialize<Response>(json);
        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("board created successfuly");
        }
    }
    /// <summary>
    /// this function test the DeleteBoard function
    /// </summary>
    public void DeleteBoardTest()
    {
        //this function should print The board deleted successfuly
        String json = boardService.CreateBoard("blackstone@gmail.com", "morshed");
        String json1 = boardService.DeleteBoard("blackstone@gmail.com", "morshed");
        Response res = JsonSerializer.Deserialize<Response>(json1);
        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("The board deleted successfuly");
        }

        //this function should throw an exeption because there is no board created for this user .
        String json2 = boardService.DeleteBoard("aboaltaha@gmail.com", "morshed");
        Response res2 = JsonSerializer.Deserialize<Response>(json2);
        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("deleting a board that is not existing");
        }
    }

    public void GetUserBoardsTest()
    {
        // Create a board for the user
        boardService.CreateBoard("daniel@gmail.com", "daniel");
        Response res;

        String json = boardService.GetUserBoards("daniel@gmail.com");
        res = JsonSerializer.Deserialize<Response>(json);

        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Correct list");
        }
    }

    public void JoinBoardTest()
    {
        //should print The user successfully joined the board
        string json = boardService.JoinBoard("daniel@gmail.com", 1);
        Response res = JsonSerializer.Deserialize<Response>(json);

        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("The user successfully joined the board");
        }

        //This test should should throw an exception because board doesn't exist
        string json2 = boardService.JoinBoard("daniel@gmail.com", 100);
        Response res2 = JsonSerializer.Deserialize<Response>(json2);

        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("test failed - attempt to join board that dosent exist ");
        }

        //This test should should throw an exeption because the user already member at this board 
        string json3 = boardService.JoinBoard("daniel@gmail.com", 1);
        Response res3 = JsonSerializer.Deserialize<Response>(json3);

        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("test failed - attempt to join user already that already a member");
        }
    }

    public void LeaveBoardTest()
    {

        //This test should print The user successfully left the board
        string json = boardService.LeaveBoard("keren@gmail.com", 1);
        Response res = JsonSerializer.Deserialize<Response>(json);

        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("The user successfully left the board");
        }

        //This test should throw an exception for attempt to leave a board by a user who was not a member
        string json2 = boardService.LeaveBoard("mhmd@gmail.com", 1);
        Response res2 = JsonSerializer.Deserialize<Response>(json2);

        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - attempt to leave a board by a user who was not a member ");
        }

        //This test should throw an exception for attempt to leave the board by the board owner
        string json3 = boardService.LeaveBoard("daniel@gmail.com", 1);
        Response res3 = JsonSerializer.Deserialize<Response>(json3);

        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - ttempt to leave the board by the board owner");
        }

        //This test should throw an exception attempt to leave a board that does not exist
        string json4 = boardService.LeaveBoard("daniel@gmail.com", 100);
        Response res4 = JsonSerializer.Deserialize<Response>(json4);

        if (res4.ErrorOccured)
        {
            Console.WriteLine(res4.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - attempt to leave a board that does not exist");
        }
        }
    
    public void GetBoardNameTest()
    {
        //This test should print Return the Board name successfully
        string json = boardService.GetBoardName(1);
        Response res = JsonSerializer.Deserialize<Response>(json);

        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Return the Board name successfully");
        }

        //This test should throw an exception - boardID does not exist 
        string json2 = boardService.GetBoardName(100);
        Response res2 = JsonSerializer.Deserialize<Response>(json2);

        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - boardID does not exist");
        }
    }

    public void TransferOwnershipTest()
    {
        //This test should print Return the Board name successfully
        string json = boardService.TransferOwnership("mhmd@gmail.com", "keren@gmail.com", "MyBoard");
        Response res = JsonSerializer.Deserialize<Response>(json);

        if (res.ErrorOccured)
        {
            Console.WriteLine(res.ErrorMessage);
        }
        else
        {
            Console.WriteLine("ownership transfered successfully");
        }

        //This test should throw an exception - current owner email does not exist 
        string json2 = boardService.TransferOwnership("unknown@gmail.com", "keren@gmail.com", "MyBoard");
        Response res2 = JsonSerializer.Deserialize<Response>(json2);

        if (res2.ErrorOccured)
        {
            Console.WriteLine(res2.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - new owner email does not exist ");
        }

        //This test should throw an exception - current owner email does not exist 
        string json3 = boardService.TransferOwnership("daniel@gmail.com", "unknown@gmail.com", "MyBoard");
        Response res3 = JsonSerializer.Deserialize<Response>(json3);

        if (res3.ErrorOccured)
        {
            Console.WriteLine(res3.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - current owner email does not exist ");
        }

        //This test should throw an exception - boardName does not exist 
        string json4 = boardService.TransferOwnership("daniel@gmail.com", "unknown@gmail.com", "MyBoard");
        Response res4 = JsonSerializer.Deserialize<Response>(json4);

        if (res4.ErrorOccured)
        {
            Console.WriteLine(res4.ErrorMessage);
        }
        else
        {
            Console.WriteLine("Test failed - boardName does not exist");
        }

    }





}



        
           


