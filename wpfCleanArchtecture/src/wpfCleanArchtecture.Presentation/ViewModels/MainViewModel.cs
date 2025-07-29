using System.Collections.ObjectModel;
using System.Windows;
using wpfCleanArchtecture.Application;
using wpfCleanArchtecture.Application.Dtos;
using wpfCleanArchtecture.Domain;

namespace wpfCleanArchtecture.Presentation.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ITodoService _todoService;
    private string _newTodoTitle = string.Empty;
    private string _newTodoDescription = string.Empty;
    private Priority _newTodoPriority = Priority.Medium;
    private string _filterText = string.Empty;
    private bool _showCompleted = true;
    private bool _showPending = true;
    private bool _isLoading = false;
    public int CompletedCount => Todos.Count(t => t.IsCompleted);
    public int PendingCount => Todos.Count(t => !t.IsCompleted);
    public MainViewModel(ITodoService todoService)
    {
        _todoService = todoService;

        Todos = new ObservableCollection<TodoItemViewModel>();
        FilteredTodos = new ObservableCollection<TodoItemViewModel>();

        AddTodoCommand = new RelayCommand(async () => await AddTodoAsync(), () => !string.IsNullOrWhiteSpace(NewTodoTitle));
        RefreshCommand = new RelayCommand(async () => await LoadTodosAsync());
        ClearCompletedCommand = new RelayCommand(async () => await ClearCompletedTodosAsync());

        _ = LoadTodosAsync();
    }

    public ObservableCollection<TodoItemViewModel> Todos { get; }
    public ObservableCollection<TodoItemViewModel> FilteredTodos { get; }

    public string NewTodoTitle
    {
        get => _newTodoTitle;
        set
        {
            SetProperty(ref _newTodoTitle, value);
            AddTodoCommand.CanExecute(null);
        }
    }

    public string NewTodoDescription
    {
        get => _newTodoDescription;
        set => SetProperty(ref _newTodoDescription, value);
    }

    public Priority NewTodoPriority
    {
        get => _newTodoPriority;
        set => SetProperty(ref _newTodoPriority, value);
    }

    public string FilterText
    {
        get => _filterText;
        set
        {
            SetProperty(ref _filterText, value);
            ApplyFilter();
        }
    }

    public bool ShowCompleted
    {
        get => _showCompleted;
        set
        {
            SetProperty(ref _showCompleted, value);
            ApplyFilter();
        }
    }

    public bool ShowPending
    {
        get => _showPending;
        set
        {
            SetProperty(ref _showPending, value);
            ApplyFilter();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public RelayCommand AddTodoCommand { get; }
    public RelayCommand RefreshCommand { get; }
    public RelayCommand ClearCompletedCommand { get; }

    private async Task LoadTodosAsync()
    {
        IsLoading = true;

        try
        {
            var result = await _todoService.GetAllTodosAsync();

            if (result.IsSuccess)
            {
                Todos.Clear();

                foreach (var todoDto in result.Value)
                {
                    var todoViewModel = new TodoItemViewModel(todoDto);
                    todoViewModel.ToggleCompletionRequested += OnToggleCompletionRequested;
                    todoViewModel.EditRequested += OnEditRequested;
                    todoViewModel.DeleteRequested += OnDeleteRequested;

                    Todos.Add(todoViewModel);
                }

                ApplyFilter();
            }
            else
            {
                MessageBox.Show($"할일 목록을 불러오는데 실패했습니다: {result.Error}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AddTodoAsync()
    {
        var createDto = new CreateTodoItemDto
        {
            Title = NewTodoTitle.Trim(),
            Description = NewTodoDescription?.Trim() ?? string.Empty,
            Priority = NewTodoPriority
        };

        var result = await _todoService.CreateTodoAsync(createDto);

        if (result.IsSuccess)
        {
            var todoViewModel = new TodoItemViewModel(result.Value);
            todoViewModel.ToggleCompletionRequested += OnToggleCompletionRequested;
            todoViewModel.EditRequested += OnEditRequested;
            todoViewModel.DeleteRequested += OnDeleteRequested;

            Todos.Add(todoViewModel);
            ApplyFilter();

            // 입력 필드 초기화
            NewTodoTitle = string.Empty;
            NewTodoDescription = string.Empty;
            NewTodoPriority = Priority.Medium;
        }
        else
        {
            MessageBox.Show($"할일 추가에 실패했습니다: {result.Error}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void OnToggleCompletionRequested(TodoItemViewModel todoViewModel)
    {
        var result = await _todoService.ToggleCompletionAsync(todoViewModel.Id);

        if (result.IsSuccess)
        {
            todoViewModel.UpdateFromDto(result.Value);
            ApplyFilter();
        }
        else
        {
            MessageBox.Show($"상태 변경에 실패했습니다: {result.Error}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnEditRequested(TodoItemViewModel todoViewModel)
    {
        // 편집 다이얼로그 표시 (구현 생략)
        MessageBox.Show($"편집 기능: {todoViewModel.Title}", "정보", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void OnDeleteRequested(TodoItemViewModel todoViewModel)
    {
        var result = MessageBox.Show($"'{todoViewModel.Title}' 항목을 삭제하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            var deleteResult = await _todoService.DeleteTodoAsync(todoViewModel.Id);

            if (deleteResult.IsSuccess)
            {
                Todos.Remove(todoViewModel);
                ApplyFilter();
            }
            else
            {
                MessageBox.Show($"삭제에 실패했습니다: {deleteResult.Error}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async Task ClearCompletedTodosAsync()
    {
        var completedTodos = Todos.Where(t => t.IsCompleted).ToList();

        if (completedTodos.Count == 0)
        {
            MessageBox.Show("완료된 항목이 없습니다.", "정보", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show($"{completedTodos.Count}개의 완료된 항목을 모두 삭제하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            foreach (var todo in completedTodos)
            {
                await _todoService.DeleteTodoAsync(todo.Id);
                Todos.Remove(todo);
            }

            ApplyFilter();
        }
    }

    private void ApplyFilter()
    {
        FilteredTodos.Clear();

        var filtered = Todos.Where(todo =>
        {
            // 완료 상태 필터
            if (!ShowCompleted && todo.IsCompleted) return false;
            if (!ShowPending && !todo.IsCompleted) return false;

            // 텍스트 필터
            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                var filter = FilterText.ToLower();
                return todo.Title.ToLower().Contains(filter) ||
                       todo.Description.ToLower().Contains(filter);
            }

            return true;
        });

        foreach (var todo in filtered.OrderByDescending(t => t.CreatedAt))
        {
            FilteredTodos.Add(todo);
        }
    }
}
