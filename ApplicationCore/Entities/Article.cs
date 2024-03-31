namespace AnkiBooks.ApplicationCore.Entities;

public class Article(string title) : EntityBase
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    public string? ParentArticleId { get; set; }
    public Article? ParentArticle { get; set; }

    public List<BasicNote> BasicNotes { get; set; } = [];

    public List<ClozeNote> ClozeNotes { get; set; } = [];

    /// <summary>
    /// Returns count of article's dependent IArticleElement objects.
    /// </summary>
    /// <returns></returns>
    public int ElementsCount()
    {
        return BasicNotes.Count + ClozeNotes.Count;
    }

    /// <summary>
    /// Checks if article's dependent IArticleElement objects have
    /// OrdinalPosition from 0 through ElementsCount() - 1,
    /// and not missing any integer between.
    /// </summary>
    /// <returns></returns>
    public bool ElementOrdinalPositionsAreCorrect()
    {
        List<int> ordinalPositions = [];
        foreach (BasicNote bn in BasicNotes) { ordinalPositions.Add(bn.OrdinalPosition); };
        foreach (ClozeNote cn in ClozeNotes) { ordinalPositions.Add(cn.OrdinalPosition); };
        ordinalPositions.Sort();

        for (int i = 0; i < ElementsCount(); i++ )
        {
            if (ordinalPositions[i] != i)
            {
                return false;
            }
        }

        return true;
    }
}