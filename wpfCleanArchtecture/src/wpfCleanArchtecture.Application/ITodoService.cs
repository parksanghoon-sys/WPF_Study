using wpfCleanArchtecture.Application.Dtos;

namespace wpfCleanArchtecture.Application
{
    /// <summary>
    /// 비지니스 로직 구현
    /// </summary>
    public interface ITodoService
    {
        Task<Result<IEnumerable<TodoItemDto>>> GetAllTodosAsync();
        Task<Result<TodoItemDto>> GetTodoByIdAsync(int id);
        Task<Result<TodoItemDto>> CreateTodoAsync(CreateTodoItemDto createDto);
        Task<Result<TodoItemDto>> UpdateTodoAsync(UpdateTodoItemDto updateDto);
        Task<Result> DeleteTodoAsync(int id);
        Task<Result<TodoItemDto>> ToggleCompletionAsync(int id);
        Task<Result<IEnumerable<TodoItemDto>>> GetCompletedTodosAsync();
        Task<Result<IEnumerable<TodoItemDto>>> GetPendingTodosAsync();
    }
}
