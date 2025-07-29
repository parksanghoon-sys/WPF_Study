namespace wpfCleanArchtecture.Domain
{
    /// <summary>
    /// Entity 의 TodoItem 클래스입니다.
    /// TodoItem 엔티티와 Priority 열거형 정의
    /// </summary>
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            CompletedAt = DateTime.Now;
        }

        public void MarkAsIncomplete()
        {
            IsCompleted = false;
            CompletedAt = null;
        }
    }

    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
