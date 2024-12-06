using AdventOfCode2024.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2024.Puzzles.Puzzle2;

public class Runner
{
    [Theory]
    [InlineData("TestInput", 2)]
    [InlineData("Input", 326)]
    public void RunAlpha(string filename, int expected)
    {
        var reports = Execute(filename);
        var safeCount = reports.Count(r => r.IsSafe());
        safeCount.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput", 4)]
    [InlineData("Input", 381)]
    public void RunBeta(string filename, int expected)
    {
        var reports = Execute(filename);
        var safeCount = reports.Count(r => r.IsSafeWithDampener());
        safeCount.Should().Be(expected);
    }

    private sealed record Report(int[] Levels)
    {
        public bool IsSafe() => IsSafe(Levels);

        private static bool IsSafe(int[] levels)
        {
            var increases = false;
            var decreases = false;
            for (var i = 0; i < levels.Length - 1; i++)
            {
                var diff = levels[i] - levels[i + 1];
                if (diff < 0)
                {
                    diff *= -1;
                    decreases = true;
                }
                else
                {
                    increases = true;
                }

                if (diff is < 1 or > 3)
                {
                    return false;
                }
            }

            return decreases != increases;
        }

        public bool IsSafeWithDampener()
        {
            if (IsSafe())
            {
                return true;
            }

            for (var i = 0; i < Levels.Length; i++)
            {
                var dampenedLevels = Levels.ToList();
                dampenedLevels.RemoveAt(i);
                if (IsSafe(dampenedLevels.ToArray()))
                {
                    return true;
                }
            }

            return false;
        }
    }

    private static Report[] Execute(string filename)
    {
        var lines = EmbeddedResourceReader.Read<Runner>(filename);

        List<Report> reports = new();

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            reports.Add(new(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()));
        }

        return reports.ToArray();

    }
}