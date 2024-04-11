using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp.Client.Services;

public class DragStateService
{
    public IArticleElement? DraggedElement { get; set; }
}