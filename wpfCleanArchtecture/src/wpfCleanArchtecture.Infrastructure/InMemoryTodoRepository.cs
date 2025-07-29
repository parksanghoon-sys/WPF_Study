using wpfCleanArchtecture.Domain;

namespace wpfCleanArchtecture.Infrastructure
{
    public class InMemoryTodoRepository : ITodoRepository
    {
        private readonly List<TodoItem> _todos = new();
        private int _nextId = 1;

        public InMemoryTodoRepository()
        {
            // 샘플 데이터 추가
            SeedData();
        }

        public Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return Task.FromResult(_todos.AsEnumerable());
        }

        public Task<TodoItem?> GetByIdAsync(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(todo);
        }

        public Task<TodoItem> AddAsync(TodoItem todoItem)
        {
            todoItem.Id = _nextId++;
            _todos.Add(todoItem);
            return Task.FromResult(todoItem);
        }

        public Task<TodoItem> UpdateAsync(TodoItem todoItem)
        {
            var existingTodo = _todos.FirstOrDefault(t => t.Id == todoItem.Id);
            if (existingTodo != null)
            {
                existingTodo.Title = todoItem.Title;
                existingTodo.Description = todoItem.Description;
                existingTodo.IsCompleted = todoItem.IsCompleted;
                existingTodo.CompletedAt = todoItem.CompletedAt;
                existingTodo.Priority = todoItem.Priority;
                return Task.FromResult(existingTodo);
            }
            return Task.FromResult(todoItem);
        }

        public Task DeleteAsync(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _todos.Remove(todo);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TodoItem>> GetCompletedAsync()
        {
            var completedTodos = _todos.Where(t => t.IsCompleted);
            return Task.FromResult(completedTodos);
        }

        public Task<IEnumerable<TodoItem>> GetPendingAsync()
        {
            var pendingTodos = _todos.Where(t => !t.IsCompleted);
            return Task.FromResult(pendingTodos);
        }

        private void SeedData()
        {
            _todos.AddRange(new[]
            {
                new TodoItem
                {
                    Id = _nextId++,
                    Title = "WPF 클린 아키텍처 구현하기",
                    Description = "MVVM 패턴과 클린 아키텍처를 결합한 Todo 앱 만들기",
                    Priority = Priority.High,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    IsCompleted = false
                },
                new TodoItem
                {
                    Id = _nextId++,
                    Title = "장보기",
                    Description = "우유, 빵, 계란 구매",
                    Priority = Priority.Medium,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    IsCompleted = true,
                    CompletedAt = DateTime.Now.AddHours(-3)
                },
                new TodoItem
                {
                    Id = _nextId++,
                    Title = "운동하기",
                    Description = "30분 조깅",
                    Priority = Priority.Low,
                    CreatedAt = DateTime.Now,
                    IsCompleted = false
                }
            });
        }
    }
}
