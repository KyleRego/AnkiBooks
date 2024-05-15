using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.Entities;

public class ClozeNoteTests
{
    // ClozeValid() tests 

    [Fact]
    public void ClozeNoteIsNotValidWithoutClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A sentence without cloze markers"
        };

        Assert.False(cn.ValidCloze());
    }

    [Fact]
    public void ClozeNoteIsValidWithClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze marker}} is in this one so it is valid"
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

    [Fact]
    public void ClozeBackShowsTheInsideOfClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze marker}} is in this one"
        };

        Assert.Equal("A cloze marker is in this one", cn.ClozeBackText());
    }

    [Fact]
    public void ClozeBackShowsInsidesOfAllClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "Multiple {{c1::cloze markers}} are in {{c2::this one}}."
        };

        Assert.Equal("Multiple cloze markers are in this one.", cn.ClozeBackText());
    }
}