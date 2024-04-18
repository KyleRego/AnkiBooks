using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp.Client.Services;

public class DraggedItemHolder<T>
{
    public T? DraggedItem { get; set; }
}