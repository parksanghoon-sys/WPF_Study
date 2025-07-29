using wpfCleanArchtecture.Application.Dtos;
using wpfCleanArchtecture.Domain;

namespace wpfCleanArchtecture.Application
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Result<IEnumerable<TodoItemDto>>> GetAllTodosAsync()
        {
            try
            {
                var todos = await _todoRepository.GetAllAsync();
                var todoDtos = todos.Select(MapToDto);
                return Result<IEnumerable<TodoItemDto>>.Success(todoDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TodoItemDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<TodoItemDto>> GetTodoByIdAsync(int id)
        {
            try
            {
                var todo = await _todoRepository.GetByIdAsync(id);
                if (todo == null)
                    return Result<TodoItemDto>.Failure("Todo not found");

                return Result<TodoItemDto>.Success(MapToDto(todo));
            }
            catch (Exception ex)
            {
                return Result<TodoItemDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<TodoItemDto>> CreateTodoAsync(CreateTodoItemDto createDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createDto.Title))
                    return Result<TodoItemDto>.Failure("Title is required");

                var todoItem = new TodoItem
                {
                    Title = createDto.Title.Trim(),
                    Description = createDto.Description?.Trim() ?? string.Empty,
                    Priority = createDto.Priority,
                    CreatedAt = DateTime.Now,
                    IsCompleted = false
                };

                var createdTodo = await _todoRepository.AddAsync(todoItem);
                return Result<TodoItemDto>.Success(MapToDto(createdTodo));
            }
            catch (Exception ex)
            {
                return Result<TodoItemDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<TodoItemDto>> UpdateTodoAsync(UpdateTodoItemDto updateDto)
        {
            try
            {
                var existingTodo = await _todoRepository.GetByIdAsync(updateDto.Id);
                if (existingTodo == null)
                    return Result<TodoItemDto>.Failure("Todo not found");

                if (string.IsNullOrWhiteSpace(updateDto.Title))
                    return Result<TodoItemDto>.Failure("Title is required");

                existingTodo.Title = updateDto.Title.Trim();
                existingTodo.Description = updateDto.Description?.Trim() ?? string.Empty;
                existingTodo.Priority = updateDto.Priority;

                var updatedTodo = await _todoRepository.UpdateAsync(existingTodo);
                return Result<TodoItemDto>.Success(MapToDto(updatedTodo));
            }
            catch (Exception ex)
            {
                return Result<TodoItemDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteTodoAsync(int id)
        {
            try
            {
                var existingTodo = await _todoRepository.GetByIdAsync(id);
                if (existingTodo == null)
                    return Result.Failure("Todo not found");

                await _todoRepository.DeleteAsync(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result<TodoItemDto>> ToggleCompletionAsync(int id)
        {
            try
            {
                var todo = await _todoRepository.GetByIdAsync(id);
                if (todo == null)
                    return Result<TodoItemDto>.Failure("Todo not found");

                if (todo.IsCompleted)
                    todo.MarkAsIncomplete();
                else
                    todo.MarkAsCompleted();

                var updatedTodo = await _todoRepository.UpdateAsync(todo);
                return Result<TodoItemDto>.Success(MapToDto(updatedTodo));
            }
            catch (Exception ex)
            {
                return Result<TodoItemDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<TodoItemDto>>> GetCompletedTodosAsync()
        {
            try
            {
                var todos = await _todoRepository.GetCompletedAsync();
                var todoDtos = todos.Select(MapToDto);
                return Result<IEnumerable<TodoItemDto>>.Success(todoDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TodoItemDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<TodoItemDto>>> GetPendingTodosAsync()
        {
            try
            {
                var todos = await _todoRepository.GetPendingAsync();
                var todoDtos = todos.Select(MapToDto);
                return Result<IEnumerable<TodoItemDto>>.Success(todoDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TodoItemDto>>.Failure(ex.Message);
            }
        }

        private static TodoItemDto MapToDto(TodoItem todoItem)
        {
            return new TodoItemDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted,
                CreatedAt = todoItem.CreatedAt,
                CompletedAt = todoItem.CompletedAt,
                Priority = todoItem.Priority
            };
        }
    }
}
