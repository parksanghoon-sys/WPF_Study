using wpfCleanArchtecture.Application.Dtos;
using wpfCleanArchtecture.Domain;

namespace wpfCleanArchtecture.Presentation.ViewModels;

public class TodoItemViewModel : BaseViewModel
{
    private readonly TodoItemDto _todoItem;
    private bool _isCompleted;

    public int Id => _todoItem.Id;
    public string Title => _todoItem.Title;
    public string Description => _todoItem.Description;
    public Priority Priority => _todoItem.Priority;
    public DateTime CreatedAt => _todoItem.CreatedAt;
    public DateTime? CompletedAt => _todoItem.CompletedAt;
    public bool IsCompleted
    {
        get => _isCompleted;
        set => SetProperty(ref _isCompleted, value);
    }
    public string PriorityText => Priority.ToString();
    public string StatusText => IsCompleted ? "완료" : "진행중";
    public string CreatedAtText => CreatedAt.ToString("yyyy-MM-dd HH:mm");

    public RelayCommand ToggleCompletionCommand { get; }
    public RelayCommand EditCommand { get; }
    public RelayCommand DeleteCommand { get; }

    public event Action<TodoItemViewModel>? ToggleCompletionRequested;
    public event Action<TodoItemViewModel>? EditRequested;
    public event Action<TodoItemViewModel>? DeleteRequested;
    public TodoItemViewModel(TodoItemDto todoItem)
    {
        _todoItem = todoItem;
        _isCompleted = todoItem.IsCompleted;

        ToggleCompletionCommand = new RelayCommand(() => ToggleCompletionRequested?.Invoke(this));
        EditCommand = new RelayCommand(() => EditRequested?.Invoke(this));
        DeleteCommand = new RelayCommand(() => DeleteRequested?.Invoke(this));
    }
    public void UpdateFromDto(TodoItemDto dto)
    {
        IsCompleted = dto.IsCompleted;
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Description));
        OnPropertyChanged(nameof(Priority));
        OnPropertyChanged(nameof(PriorityText));
        OnPropertyChanged(nameof(StatusText));
        OnPropertyChanged(nameof(CompletedAt));
    }

}
