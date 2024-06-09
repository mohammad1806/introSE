using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.businessLayer.board;
public class Task
{
	
	public int brdID;
	public int colOrd;
	public int TaskID { get; set; }
	public String title { get; set; }
	public String description { get; set; }
	public readonly DateTime creationTime;
	public DateTime dueDate { get; set; }
	public const int maxDesc = 300;
	public const int maxTitle = 50;
	public String assignee = "unassigned";
	public readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


	public Task(int id, String title, String desc, DateTime due, int brdid, int colOrd)
	{
		this.brdID = brdid;
		this.colOrd = colOrd;
		this.TaskID = id;
		this.title = title;
		this.description = desc;
		this.dueDate = due;
		this.creationTime = DateTime.Now;
	}

	public Task(TaskDTO other)
	{
		this.TaskID = other.TaskID;
		this.brdID = other.BoardID;
		this.title = other.Title;
		this.description = other.Description;
		this.dueDate = other.DueDate;
		this.creationTime = other.CreationTime;
		this.assignee = other.Assignee;
		this.colOrd = other.ColOrd;
	}

	public TaskDTO convrtToDTO()
	{
		return new TaskDTO(TaskID, brdID, title, description, creationTime, dueDate, colOrd, assignee);
	}

	/// <summary>
	/// Gets the ID of the task.
	/// </summary>
	public int getTaskID
	{
		get { return this.TaskID; }
	}

	/// <summary>
	/// Gets or sets the title of the task.
	/// </summary>
	public String getTitle
	{
		get { return title; }
		set
		{
			if (checkTitle(value))
				title = value;
		}
	}

	/// <summary>
	/// Gets or sets the description of the task.
	/// </summary>
	public String getDescription
	{
		get { return description; }
		set
		{
			if (checkDescription(value))
				description = value;
		}
	}

	/// <summary>
	/// Gets or sets the due date of the task.
	/// </summary>
	public DateTime getDuedate
	{
		get { return dueDate; }
		set
		{
			if (checkDueDate(value))
				dueDate = value;
		}
	}
	public DateTime getCreationTime
	{
		get { return creationTime; }
	}

	/// <summary>
	/// Checks whether the given title is valid
	/// </summary>
	/// <param name="title">The title to check</param>
	/// <returns>True if the title is valid</returns>
	public bool checkTitle(String title)
	{
		if (String.IsNullOrEmpty(title))
		{
			log.Warn("unvalid title");
			throw new ArgumentException("title must not be empty!");
		}
		else if (title.Length > maxTitle)
		{
			log.Warn("unvalid title");
			throw new ArgumentException("title is too long!");
		}
		return true;
	}

	/// <summary>
	/// Checks whether the given description is valid
	/// </summary>
	/// <param name="description">The description to check</param>
	/// <returns>True if the description is valid</returns>
	public bool checkDescription(String description)
	{
		if (description.Length > maxDesc || description == null)
		{
			log.Warn("unvalid desc");
			throw new ArgumentException("description is too long!");
		}
		else
		{
			return true;
		}
	}

	/// <summary>
	/// Checks whether the given due date is valid
	/// </summary>
	/// <param name="dueDate">The due date to check</param>
	/// <returns>True if the due date is valid</returns>
	public bool checkDueDate(DateTime dueDate)
	{
		if (dueDate == null)
		{
			log.Warn("un valid duedate!");
			throw new ArgumentException("dueDate is null!");
		}
		else if (DateTime.Now > dueDate)
		{
			log.Warn("un valid duedate!");
			throw new ArgumentException("due date must not be expired!");
		}
		else { return true; }
	}

	public string Assignee
	{
		set { assignee = value; }
	}
}

		

