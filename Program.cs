
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        UserAction userAction = new UserAction();
        NoteAction noteAction = new NoteAction();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("NOT DEFTERİ UYGULAMASINA HOŞ GELDİNİZ\n");
            Console.Write("Mail: ");
            string email = Console.ReadLine();
            Console.Write("Şifre: ");
            string password = Console.ReadLine();
            Console.WriteLine();

            User loggedInUser = null;
            foreach (var user in userAction.GetUserList())
            {
                if (user.Email == email && user.Password == password)
                {
                    loggedInUser = user;
                    break;
                }
            }
            if (loggedInUser != null)
            {
                if (loggedInUser.UserRole == Role.Admin)
                {
                    Console.WriteLine("Admin Girişi");
                    Console.WriteLine();
                    AdminMenu(userAction, noteAction);
                }
                else if (loggedInUser.UserRole == Role.User)
                {
                    Console.WriteLine("Kullanıcı girişi.");
                    Console.WriteLine();
                    UserMenu(loggedInUser, noteAction);
                }
            }
            else
            {
                Console.WriteLine("Geçersiz kullanıcı adı veya şifre.");
                Console.Write("Kayıt olmak ister misiniz? (E / H): ");
                string response = Console.ReadLine();

                if (response.ToLower() == "e")
                {
                    RegisterUser(userAction);
                }
                else if (response.ToLower() == "h")
                {
                    Console.WriteLine("Görüşmek üzere");
                }
                else
                {
                    Console.WriteLine("Geçersiz işlem!");
                }
            }
        }
    }

    static void RegisterUser(UserAction userAction)
    {
        Console.Write("İsim: ");
        string newUsername = Console.ReadLine();

        Console.Write("Soyisim: ");
        string newLastName = Console.ReadLine();

        Console.Write("Yeni şifreyi giriniz: ");
        string newPassword = Console.ReadLine();

        Console.Write("Email: ");
        string email = Console.ReadLine();

        Console.Write("Telefon Numarası: ");
        string phoneNumber = Console.ReadLine();

        if (phoneNumber.StartsWith("0")) // phoneNumber[1].equals("0") 
        {
            phoneNumber = phoneNumber.Substring(1);
        }

        Console.Write("Rolünüzü giriniz(Admin/User): ");
        Role newRole = (Role)Enum.Parse(typeof(Role), Console.ReadLine(), true);

        userAction.AddUser(new User(newUsername, newLastName, newPassword, $"{newUsername}_notes.txt", newRole, email, phoneNumber));
    }

    static void AdminMenu(UserAction userAction, NoteAction noteAction)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine();
            Console.WriteLine("---ADMİN İŞLEM MENÜSÜ---\n");
            Console.WriteLine("1. Kullanıcıları listele.");
            Console.WriteLine("2. Kullanıcı ekle.");
            Console.WriteLine("3. Kullanıcıları ara.");
            Console.WriteLine("4. Kullanıcı sil.");
            Console.WriteLine("5. Çıkış yap.");
            Console.Write("İşlem numarasını giriniz: ");

            string adminInput = Console.ReadLine();
            switch (adminInput)
            {
                case "1":
                    {
                        var users = userAction.GetUserList();
                        Console.WriteLine();
                        Console.WriteLine("Kullanıcılar:");
                        foreach (var user in users)
                        {
                            Console.WriteLine($"İsim: {user.Username}, Soyisim: {user.LastName}, Email: {user.Email}, Telefon Numarası: {user.PhoneNumber}");
                        }
                        break;
                    }
                case "2":
                    RegisterUser(userAction);
                    break;

                case "3":
                    {
                        Console.Write("Arama terimini girin: ");
                        string searchTerm = Console.ReadLine();
                        var foundUser = userAction.GetUserByFilter(searchTerm);
                        if (foundUser != null)
                        {
                            Console.WriteLine($"İsim: {foundUser.Username}, Soyisim: {foundUser.LastName}, Email: {foundUser.Email}, Telefon Numarası: {foundUser.PhoneNumber}");
                        }
                        else
                        {
                            Console.WriteLine("Kullanıcı bulunamadı.");
                        }
                        break;
                    }
                case "4":
                    {
                        Console.Write("Silmek istediğiniz kullanıcının kullanıcı adını girin: ");
                        string username = Console.ReadLine();
                        userAction.DeleteUser(username);
                        break;
                    }
                case "5":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Geçersiz işlem numarası.");
                    break;
            }
        }
    }

    static void UserMenu(User loggedInUser, NoteAction noteAction)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine();
            Console.WriteLine("---NOT DEFTERİ---\n");
            Console.WriteLine("1. Not Ekle.");
            Console.WriteLine("2. Notları Listele.");
            Console.WriteLine("3. Not Sil.");
            Console.WriteLine("4. Çıkış yap.");
            Console.Write("İşlem numarasını giriniz: ");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    Console.Write("Yeni notunuzu giriniz: ");
                    string note = Console.ReadLine();
                    noteAction.AddNote(loggedInUser.Username, note);
                    break;

                case "2":
                    var notes = noteAction.GetNoteList(loggedInUser.Username);
                    Console.WriteLine();
                    Console.WriteLine("---NOTLAR---\n");
                    foreach (var n in notes)
                    {
                        Console.WriteLine(n);
                    }
                    break;

                case "3":
                    Console.WriteLine();
                    Console.WriteLine("---NOTLAR---\n");
                    var noteList = noteAction.GetNoteList(loggedInUser.Username);
                    for (int i = 0; i < noteList.Count; i++)
                    {
                        Console.WriteLine($"{i}. {noteList[i]}");
                    }
                    Console.Write("Silmek istediğiniz notun numarasını giriniz: ");
                    int noteIndex = Convert.ToInt32(Console.ReadLine());
                    noteAction.DeleteNote(loggedInUser.Username, noteIndex);
                    break;

                case "4":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Geçersiz işlem numarası.");
                    break;
            }
        }
    }
}
