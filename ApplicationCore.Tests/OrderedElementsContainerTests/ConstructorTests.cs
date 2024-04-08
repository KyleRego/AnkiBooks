using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class ConstructorTests : Base
{
    [Fact]
    public void TestConstructorWithOnlyClozeNotes()
    {
        Article article = new("Test article")
        {
            BasicNotes = [],
            ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=1 }
            ]
        };

        OrderedElementsContainer manager = new(article.BasicNotes, article.ClozeNotes, []);
        
        Assert.True(NumberOfElementsIsCorrect(manager, 2));
        Assert.True(ElementsAreCorrectlyOrdered(manager));
    }

    [Fact]
    public void TestConstructorWithBasicAndClozeNotes()
    {
        Article article = new("Test article")
        {
            BasicNotes =
            [
                new() { Front="a", Back="b", OrdinalPosition=1 },
                new() { Front="a", Back="b", OrdinalPosition=3 },
                new() { Front="a", Back="b", OrdinalPosition=4 }
            ],
                ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=2 }
            ]
        };

        OrderedElementsContainer manager = new(article.BasicNotes, article.ClozeNotes, []);

        Assert.True(NumberOfElementsIsCorrect(manager, 5));
        Assert.True(ElementsAreCorrectlyOrdered(manager));
    }
}