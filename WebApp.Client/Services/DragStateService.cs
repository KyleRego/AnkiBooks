using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp.Client.Services;

public class DragStateService
{
    public IOrderedElement? DraggedElement { get; set; }
}