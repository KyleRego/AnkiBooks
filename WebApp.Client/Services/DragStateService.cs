using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Client.Services;

public class DragStateService
{
    public ArticleElementBase? DraggedElement { get; set; }
}