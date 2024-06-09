using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.businessLayer.board;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using log4net;
using log4net.Config;


public class UserController
{
    public Dictionary<string, User> users;
    private static readonly ILog log = LogManager.GetLogger(typeof(UserController));

    private UserDALcontroller DALcon;
    public UserController()
    {
        this.users = new Dictionary<string, User>();
        DALcon = new UserDALcontroller();
    }
     

    public void loadData()
    {
        List<UserDTO> list = DALcon.getAllUsers();
        foreach (UserDTO usr in list)
        {
            users.Add(usr.Email , new User(usr));
        }

    }

    public void DeleteData()
    {
        DALcon.DeleteAll();
        users = new Dictionary<string, User>();
    }
    /// <summary>
    /// Registers a new user with the given email and password.
    /// </summary>
    /// <param name="email">The email of the user to register.</param>
    /// <param name="password">The password of the user to register.</param>
    /// <exception cref="Exception">Thrown when the email is invalid or already registered, or when the password is invalid.</exception>
    public string Register(string email, string password)
    {
        if (!IsValidEmail(email))
        {
            log.Warn("Attempt to register with invalid email");
            throw new Exception("Invalid email");
        }

        if (!IsValidPassword(password))
        {
            log.Warn("Attempt to register with invalid password");
            throw new Exception("Invalid password");
        }

        if (IsEmailTaken(email))
        {
            log.Warn("Attempt to register with existed email");
            throw new ArgumentException("Email already registered");
        }

        User u = new User(email, password, true);
        users.Add(email, u);
        DALcon.addUser(u.convrtToDTO());
        log.Info("registering was successful");
        return email;

    }

    /// <summary>
    /// Logs in the user with the given email and password.
    /// </summary>
    /// <param name="email">The email of the user to login.</param>
    /// <param name="password">The password of the user to login.</param>
    /// <exception cref="ArgumentException">Thrown when the email is invalid or when the user with the given email does not exist.</exception>
    /// <exception cref="Exception">Thrown when the user with the given email and password combination does not exist.</exception>
    public String Login(string email, string password)
    {
        if (!IsValidEmail(email))
        {
            log.Warn("Attempt to login with invalid email");
            throw new ArgumentException("Email not valid");
        }
        if (!users.ContainsKey(email)) // if email does not exisit
        {
            log.Warn("Attempt to login with unexisted email");
            throw new Exception("User does not exisit");
        }
        users[email].Login(password);
        log.Info("logging was successful");
        return email;

    }

    /// <summary>
    /// Logs out the user with the given email.
    /// </summary>
    /// <param name="email">The email of the user to logout.</param>
    /// <exception cref="ArgumentException">Thrown when the email is invalid or when the user with the given email does not exist.</exception>
    /// <exception cref="ArgumentException">Thrown when the user with the given email is already logged out.</exception>
    public string Logout(string email)
    {
        if (!IsValidEmail(email))
        {

            log.Warn("Attempt to logout with invalid email");
            throw new ArgumentException("Email not valid");
        }

        if (!users.ContainsKey(email)) // if email does not exisit
        {
            log.Warn("Attempt to logout with unexisted email");
            throw new Exception("User does not exisit");
        }
        users[email].Logout();
        return email;

    }

    /// <summary>
    /// Changes the password for a user with the given email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="password">The current password of the user.</param>
    /// <param name="newPass">The new password to set for the user.</param>
    /// <exception cref="ArgumentException">Thrown when the provided email is not a valid email address.</exception>
    /// <exception cref="Exception">Thrown when there is no user with the provided email address.</exception>
    public void changePassword(string email, string password, string newPass)
    {
        if (!IsValidEmail(email))
        {

            log.Warn("Attempt to cange password with invalid email");
            throw new ArgumentException("Email not valid");
        }
        if (!users.ContainsKey(email))
        {
            log.Warn("Attempt to login with unexisted email");
            throw new Exception("User does not exisit");
        }
        users[email].changePassword(password, newPass);
        log.Info("changing was successful");

    }

    /// <summary>
    /// Determines if an email address is valid.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True if the email address is valid, false otherwise.</returns>
    private bool IsValidEmail(string email)
    {

        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                var domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    /// <summary>
    /// Determines if a password is valid.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>True if the password is valid, false otherwise.</returns>
    private bool IsValidPassword(string password)
    {
        return password.Length >= 6 && password.Length <= 20
            && password.Any(c => char.IsUpper(c))
            && password.Any(c => char.IsLower(c))
            && password.Any(c => char.IsDigit(c));
    }

    /// <summary>
    /// Determines whether the given email address is already associated with a user account.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>True if the email address is already associated with a user account; otherwise, false.</returns>
    private bool IsEmailTaken(string email)
    {
        return users.ContainsKey(email);
    }

   
}