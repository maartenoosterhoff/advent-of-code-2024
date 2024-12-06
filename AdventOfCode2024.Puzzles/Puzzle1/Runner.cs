using AdventOfCode2024.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2024.Puzzles.Puzzle1;

public class Runner
{
    [Theory]
    [InlineData("TestInput", 11)]
    [InlineData("Input", 2057374)]
    public void RunAlpha(string filename, int expected)
    {
        var (points1, points2) = Execute(filename);
        var totalDistance = points1.OrderBy(x => x).Zip(points2.OrderBy(x => x), (p1, p2) => p1 - p2).Select(x => x > 0 ? x : x * -1).Sum();
        totalDistance.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput", 31)]
    [InlineData("Input", 23177084)]
    public void RunBeta(string filename, int expected)
    {
        var (points1, points2) = Execute(filename);
        var map = points2.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        var sum = 0;

        foreach (var point1 in points1)
        {
            if (map.TryGetValue(point1, out var scoreIncreaser))
            {
                sum += point1 * scoreIncreaser;
            }
        }

        sum.Should().Be(expected);
    }

    private static (int[] points1, int[] point2) Execute(string filename)
    {
        var lines = EmbeddedResourceReader.Read<Runner>(filename);

        List<int> list1 = new();
        List<int> list2 = new();

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var points = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (points.Length == 2 &&
                int.TryParse(points[0], out var point1) &&
                int.TryParse(points[1], out var point2))
            {
                list1.Add(point1);
                list2.Add(point2);
            }
        }

        return (list1.ToArray(), list2.ToArray());

    }
}