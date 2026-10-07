using System;
using NUnit.Framework;

/// <summary>
/// EditMode coverage for ArraysHandler index cycling (P0.E7.T1). Weapon use-rate cycling,
/// weapon-slot switching and after-image rotation all rely on these wrap semantics.
/// </summary>
public class ArraysHandlerTests
{
    [Test]
    public void GetNextIndex_ReturnsFollowingIndex()
    {
        Assert.AreEqual(1, ArraysHandler.GetNextIndex(0, 5));
        Assert.AreEqual(3, ArraysHandler.GetNextIndex(2, 5));
    }

    [Test]
    public void GetNextIndex_WrapsFromLastToFirst()
    {
        Assert.AreEqual(0, ArraysHandler.GetNextIndex(4, 5));
    }

    [Test]
    public void GetPreviousIndex_ReturnsPrecedingIndex()
    {
        Assert.AreEqual(2, ArraysHandler.GetPreviousIndex(3, 5));
        Assert.AreEqual(0, ArraysHandler.GetPreviousIndex(1, 5));
    }

    [Test]
    public void GetPreviousIndex_WrapsFromFirstToLast()
    {
        Assert.AreEqual(4, ArraysHandler.GetPreviousIndex(0, 5));
    }

    [Test]
    public void GetNextIndex_WithSingleElement_StaysAtZero()
    {
        Assert.AreEqual(0, ArraysHandler.GetNextIndex(0, 1));
    }

    [Test]
    public void GetPreviousIndex_WithSingleElement_StaysAtZero()
    {
        Assert.AreEqual(0, ArraysHandler.GetPreviousIndex(0, 1));
    }

    [Test]
    public void NextThenPrevious_ReturnsOriginalIndex()
    {
        for (int index = 0; index < 5; index++)
        {
            int cycled = ArraysHandler.GetPreviousIndex(ArraysHandler.GetNextIndex(index, 5), 5);
            Assert.AreEqual(index, cycled, "index " + index);
        }
    }

    [Test]
    public void PreviousThenNext_ReturnsOriginalIndex()
    {
        for (int index = 0; index < 5; index++)
        {
            int cycled = ArraysHandler.GetNextIndex(ArraysHandler.GetPreviousIndex(index, 5), 5);
            Assert.AreEqual(index, cycled, "index " + index);
        }
    }

    [Test]
    public void WalkingNext_AcrossFullCycle_VisitsEveryIndexExactlyOnce()
    {
        bool[] visited = new bool[5];
        int current = 0;

        for (int step = 0; step < 5; step++)
        {
            Assert.IsFalse(visited[current], "index visited twice: " + current);
            visited[current] = true;
            current = ArraysHandler.GetNextIndex(current, 5);
        }

        Assert.AreEqual(0, current, "walk should return to the start");
        for (int index = 0; index < 5; index++)
            Assert.IsTrue(visited[index], "index never visited: " + index);
    }

    [Test]
    public void WalkingPrevious_AcrossFullCycle_VisitsEveryIndexExactlyOnce()
    {
        bool[] visited = new bool[5];
        int current = 0;

        for (int step = 0; step < 5; step++)
        {
            Assert.IsFalse(visited[current], "index visited twice: " + current);
            visited[current] = true;
            current = ArraysHandler.GetPreviousIndex(current, 5);
        }

        Assert.AreEqual(0, current, "walk should return to the start");
        for (int index = 0; index < 5; index++)
            Assert.IsTrue(visited[index], "index never visited: " + index);
    }

    [Test]
    public void GetNextIndex_WithEmptyRange_ThrowsDivideByZero()
    {
        Assert.Throws<DivideByZeroException>(() => ArraysHandler.GetNextIndex(0, 0));
    }
}
