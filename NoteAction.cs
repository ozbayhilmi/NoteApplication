public class NoteAction : INoteAction
{
    public static string BasePath = Environment.CurrentDirectory;

    public void AddNote(string username, string noteContent)
    {
        try
        {
            string filePath = FilePathGen(username);
            string noteWithDate = $"{noteContent}| {DateTime.Now}";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(noteWithDate);
            }
            Console.WriteLine("Not başarıyla eklendi.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public List<string> GetNoteList(string username)
    {
        List<string> notes = new List<string>();
        try
        {
            string filePath = FilePathGen(username);
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    notes.Add(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata!: {ex.Message}");
        }
        return notes;
    }

    public void DeleteNote(string username, int noteIndex)
    {
        try
        {
            string filePath = FilePathGen(username);
            var notes = File.ReadAllLines(filePath).ToList();
            if (noteIndex >= 0 && noteIndex < notes.Count)
            {
                notes.RemoveAt(noteIndex);
                File.WriteAllLines(filePath, notes);
                Console.WriteLine("Not başarıyla silindi.");
            }
            else
            {
                Console.WriteLine("Geçersiz not numarası.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata!: {ex.Message}");
        }
    }

    private string FilePathGen(string username)
    {
        string filePath = Path.Combine(BasePath, $"{username}_notes.txt");
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
        return filePath;
    }
}