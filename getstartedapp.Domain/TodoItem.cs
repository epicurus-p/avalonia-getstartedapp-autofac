namespace getstartedapp.Domain;

public class TodoItem
{
    public TodoItem()
    {
    }
    public TodoItem(int id, string title, bool isDone)
    {
        Id = id;
        Title = title;
        IsDone = isDone;
    }
    public int Id { get; set; }  
    
    public string Title { get; set; } = ""; 
    
    public bool IsDone { get; set; }
}