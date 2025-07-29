using wpfCleanArchtecture.Domain;
/// <summary>
/// IoRepository 인터페이스는 TodoItem 엔티티에 대한 CRUD 작업을 정의합니다.
/// </summary>
public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task<TodoItem> AddAsync(TodoItem todoItem);
    Task<TodoItem> UpdateAsync(TodoItem todoItem);
    Task DeleteAsync(int id);
    Task<IEnumerable<TodoItem>> GetCompletedAsync();
    Task<IEnumerable<TodoItem>> GetPendingAsync();
}