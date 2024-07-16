public interface INoteAction
{
    void AddNote(string username, string noteContent);
    List<string> GetNoteList(string username);
    void DeleteNote(string username, int noteIndex);
}