public class UserAction : IUserAction
{
    private List<User> users;

    public UserAction()
    {
        users = new List<User>();
        LoadUsersFromFile();
    }

    public void AddUser(User user)
    {
        if (users.Any(u => u.PhoneNumber == user.PhoneNumber))
        {
            Console.WriteLine("Bu telefon numarasıyla kayıtlı bir kullanıcı zaten var. Kullanıcı eklenemedi.");
            return;
        }
        users.Add(user);
        SaveUsersToFile();
    }

    public List<User> GetUserList()
    {
        return users;
    }

    public void DeleteUser(string username)
    {
        User userToDelete = GetUserByFilter(username);
        if (userToDelete != null)
        {
            users.Remove(userToDelete);
            Console.WriteLine($"Kullanıcı {username} başarıyla silindi.");
            if (File.Exists(userToDelete.NoteFilePath))
            {
                File.Delete(userToDelete.NoteFilePath);
                Console.WriteLine($"Not dosyası {userToDelete.NoteFilePath} başarıyla silindi.");
            }
            SaveUsersToFile();
        }
        else
        {
            Console.WriteLine("Kullanıcı bulunamadı.");
        }
    }

    public User GetUserByFilter(string searchTerm)
    {
        if (searchTerm.Length < 3)
        {
            Console.WriteLine("Arama terimi en az 3 karakter uzunluğunda olmalıdır.");
            return null;
        }

        foreach (var user in users) // Or işlemi ile kullanıcı bulma işlemi
        {
            if (user.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                user.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                user.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                user.PhoneNumber.Contains(searchTerm))
            {
                return user;
            }
        }
        return null;
    }

    private void SaveUsersToFile()
    {
        using (StreamWriter writer = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "users.txt")))
        {
            foreach (var user in users)
            {
                writer.WriteLine($"{user.Username},{user.LastName},{user.Password},{user.NoteFilePath},{user.UserRole},{user.Email},{user.PhoneNumber}");
            }
        }
    }

    private void LoadUsersFromFile()
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "users.txt");
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    string username = parts[0];
                    string lastName = parts[1];
                    string password = parts[2];
                    string noteFilePath = parts[3];
                    Role role = (Role)Enum.Parse(typeof(Role), parts[4]);
                    string email = parts[5];
                    string phoneNumber = parts[6];
                    users.Add(new User(username, lastName, password, noteFilePath, role, email, phoneNumber));
                }
            }
        }
    }
}