using System;
using IntroSE.Kanban.Backend.businessLayer;
namespace IntroSE.Kanban.Backend.ServiceLayer;

public class TaskToSend
{
    public int Id { get; set; }
    public DateTime CreationTime { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Assignee { get; set; }

    public TaskToSend() { }
    public TaskToSend(businessLayer.board.Task task)
    {
        
        this.Title = task.getTitle;
        this.Description = task.getDescription;
        this.CreationTime = task.getCreationTime;
        this.DueDate = task.getDuedate;
        this.Id = task.getTaskID;
    }
}