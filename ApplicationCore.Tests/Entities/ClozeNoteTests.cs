using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.Entities;

public class ClozeNoteTests
{
    // ClozeValid() tests 

    [Fact]
    public void ClozeValidIsFalseWithoutClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A sentence without cloze markers"
        };

        Assert.False(cn.ValidCloze());
    }

    [Fact]
    public void ClozeValidIsTrueWithClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze marker}} is in this one so it is valid"
        };

        Assert.True(cn.ValidCloze());
    }

    [Fact]
    public void ClozeValidIsTrueWithMultipleClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze marker}} is {{c2::in this}} one so it is valid"
        };

        Assert.True(cn.ValidCloze());
    }

    // ClozeFrontText() tests

    [Fact]
    public void ClozeNoteFrontReplacesOneClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze marker}} is in this one"
        };

        Assert.Equal("A [...] is in this one", cn.ClozeFrontText());
    }

    [Fact]
    public void ClozeNoteFrontReplacesAllClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "Multiple {{c1::cloze markers}} are in {{c2::this one}}."
        };

        Assert.Equal("Multiple [...] are in [...].", cn.ClozeFrontText());
    }

    // ClozeBackText() Tests

    [Fact]
    public void ClozeBackTextShowsTheInsideOfClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze marker}} is in this one"
        };

        Assert.Equal("A cloze marker is in this one", cn.ClozeBackText());
    }

    [Fact]
    public void ClozeBackTextShowsInsidesOfAllClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "Multiple {{c1::cloze markers}} are in {{c2::this one}}."
        };

        Assert.Equal("Multiple cloze markers are in this one.", cn.ClozeBackText());
    }

    // ClozeBackHtml() tests

    [Fact]
    public void ClozeBackHtmlShowsAnswersInSpan()
    {
        ClozeNote cn = new()
        {
            Text = "Multiple {{c1::cloze markers}} are in {{c2::this one}}."
        };

        Assert.Equal("Multiple <span class=\"cloze-answer\">cloze markers</span> are in <span class=\"cloze-answer\">this one</span>.", cn.ClozeBackHtml());
    }
}