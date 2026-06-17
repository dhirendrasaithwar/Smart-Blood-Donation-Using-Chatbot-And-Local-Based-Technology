using System.ComponentModel.DataAnnotations;

namespace Common;

public class ChatbotViewModel
{
    
}
public class QuestionDto
{
    public long Id { get; set; }
    public string Question { get; set; }
}
public class AddChatbotViewModel
{
    [Required]
    public string Category { get; set; }
    [Required]
    public string Question { get; set; }
    [Required]
    public string Answer { get; set; }
}