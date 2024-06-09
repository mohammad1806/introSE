using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using System;

public class User
{
    private string email { get; set; }
    private string password { get; set; }
    private bool status { get; set; }

    //constructor
    public User(string email, string password, bool is_logged)
    { 
        this.email = email;
        this.password = password;
        this.status = is_logged;
    }

    public User(UserDTO other)
    {
        this.email = other.Email;
        this.password = other.Password;
        this.status = other.Status;
    }

    public UserDTO convrtToDTO()
    {
        return new UserDTO(Email,Password,false);
    }


    /// <summary>
    /// Logs in the user if the given password matches the account password.
    /// </summary>
    /// <param name="password">The password to check.</param>
    /// <exception cref="ArgumentException">Thrown if the password is incorrect.</exception>
    public void Login(string password)
    {
        if (this.password.Equals(password))
        {
            status = true;
        }
        else
        {
            throw new ArgumentException("password in incorrect!");
        }
    }

    /// <summary>
    /// Changes the password for the account if the old password matches.
    /// </summary>
    /// <param name="password">The old password.</param>
    /// <param name="newPass">The new password.</param>
    public void changePassword(string password, string newPass)
    {
        if (this.password.Equals(password))
        {
            this.password = newPass;
        }
    }

    /// <summary>
    /// Logs out the user by setting the status to false.
    /// </summary>
    public void Logout()
    {
            this.status = false;
    }


    public string Email
    {
        get { return this.email; }
        set { this.email = value; }
    }
    /// <summary>
    /// Gets the current login status of the account.
    /// </summary>
    public bool Status
    {
        get { return status; }
        set { status = value; }
    }

    /// <summary>
    /// Gets the password of the account.
    /// </summary>
    public String Password
    {
        get { return password; }
        set { password = value; }
    }


}