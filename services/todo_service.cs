
namespace services;

internal sealed class TodoService
{
    private readonly List<TodoItem> _todos =
    [
        new(1, "Buy milk", false),
        new(2, "Read docs", true)
    ];

    public IReadOnlyList<TodoItem> GetAll() => _todos;

    public TodoItem Create(string title)
    {
        var nextId = _todos.Count == 0 ? 1 : _todos.Max(x => x.Id) + 1;
        var todo = new TodoItem(nextId, title, false);
        _todos.Add(todo);
        return todo;
    }
}