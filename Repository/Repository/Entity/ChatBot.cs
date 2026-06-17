namespace Repository;

public class ChatBot
{
    public long Id { get; set; }
    public string Category{get;set;}
    public long? UserId { get; set; }
    public string Name { get; set; }
    public string Answer { get; set; }
    
    public User User { get; set; }
}