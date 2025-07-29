using wpfCleanArchtecture.Domain;

namespace wpfCleanArchtecture.Application.Dtos
{
    /// <summary>
    /// 전송 객체들
    /// </summary>
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Priority Priority { get; set; }
    }

    public class CreateTodoItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; } = Priority.Medium;
    }

    public class UpdateTodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; }
    }
}
