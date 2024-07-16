public class User
{
    public string Username { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string NoteFilePath { get; set; }
    public Role UserRole { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public User(string username, string lastName, string password, string noteFilePath, Role userRole, string email, string phoneNumber)
    {
        Username = username;
        LastName = lastName;
        Password = password;
        NoteFilePath = noteFilePath;
        UserRole = userRole;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}
