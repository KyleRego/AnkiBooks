using System.ComponentModel.DataAnnotations;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.Entities;

public class ClozeNoteTests
{
    [Fact]
    public void ClozeValidIsFalseWithoutClozeMarkers()
    {
        ClozeNote cn = new() { Text = "A sentence without cloze markers" };
        ValidationContext context = new(cn, null, null)
        {
            MemberName = "Text"
        };
        ICollection<ValidationResult> failedValidations = [];

        bool result = Validator.TryValidateProperty(cn.Text, context, failedValidations);

        Assert.False(result);
    }

    [Fact]
    public void ClozeValidIsTrueWithClozeMarkers()
    {
        ClozeNote cn = new() { Text = "A {{c1::cloze marker}} is in this one so it is valid" };
        ValidationContext context = new(cn, null, null)
        {
            MemberName = "Text"
        };
        ICollection<ValidationResult> failedValidations = [];

        bool result = Validator.TryValidateProperty(cn.Text, context, failedValidations);

        Assert.True(result);
    }

    [Fact]
    public void ClozeValidIsTrueWithMultipleClozeMarkers()
    {
        ClozeNote cn = new() { Text = "A {{c1::cloze marker}} is {{c2::in this}} one so it is valid" };
        ValidationContext context = new(cn, null, null)
        {
            MemberName = "Text"
        };
        ICollection<ValidationResult> failedValidations = [];

        bool result = Validator.TryValidateProperty(cn.Text, context, failedValidations);

        Assert.True(result);
    }

    // ClozeFrontText() tests

    [Fact]
    public void ClozeFrontTextReplacesOneClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze marker}} is in this one"
        };

        Assert.Equal("A [...] is in this one", cn.ClozeFrontText());
    }

    [Fact]
    public void ClozeFrontTextReplacesAllClozeMarkers()
    {
        ClozeNote cn = new()
        {
            Text = "Multiple {{c1::cloze markers}} are in {{c2::this one}}."
        };

        Assert.Equal("Multiple [...] are in [...].", cn.ClozeFrontText());
    }

    // ClozeFrontHtml() tests

    [Fact]
    public void ClozeFrontHtmlShowsClozePlaceholdersInSpans()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze }} ."
        };

        Assert.Equal("A <span class=\"cloze-question\">[...]</span> .", cn.ClozeFrontHtml());
    }

    [Fact]
    public void ClozeFrontHtmlSanitizesText()
    {
        ClozeNote cn = new()
        {
            Text = "A {{c1::cloze }}<script>alert(\"hello world\")</script>"
        };

        Assert.Equal("A <span class=\"cloze-question\">[...]</span>", cn.ClozeFrontHtml());
    }

    // ClozeBackText() tests

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

    [Fact]
    public void ClozeBackHtmlSanitizesText()
    {
        ClozeNote cn = new()
        {
            Text = "Test cloze {{c1::<script>'hello world'</script>}} 1"
        };

        Assert.Equal("Test cloze <span class=\"cloze-answer\"></span> 1", cn.ClozeBackHtml());
    }
}