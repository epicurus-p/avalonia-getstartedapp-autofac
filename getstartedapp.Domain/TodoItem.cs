namespace getstartedapp.Domain;

public enum TodoPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}
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

    public TodoPriority Priority { get; set; } = TodoPriority.Medium;
    
    public DateTime? DueDate { get; set; }
    
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedUtc { get; set; }
    
    public String? Notes { get; set; }
}