using AdventOfCode2024.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2024.Puzzles.Puzzle4;

public class Runner
{
    [Theory]
    [InlineData("TestInput", 18)]
    [InlineData("Input", 2468)]
    public void RunAlpha(string filename, int expected)
    {
        var wordSearch = Execute(filename);
        var actual = wordSearch.CountWords("XMAS");
        actual.Should().Be(expected);
    }

    public sealed class WordSearch
    {
        private readonly string[] _contents;

        private readonly int _left = 0;
        private readonly int _right;
        private readonly int _top = 0;
        private readonly int _bottom;

        public WordSearch(string[] contents)
        {
            _contents = contents;
            _contents.Should().HaveCountGreaterThan(0);
            _contents.Select(x => x.Length).GroupBy(x => x).Should().HaveCount(1);

            _right = _contents[0].Length - 1;
            _bottom = _contents.Length - 1;
        }

        public int CountWords(string word)
        {
            var count = 0;
            for (var y = _top; y <= _bottom; y++)
            {
                for (var x = _left; x <= _right; x++)
                {
                    count += CountWordsAtPosition(word, x, y);
                }
            }

            return count;
        }

        private int CountWordsAtPosition(string word, int x, int y)
        {
            if (GetChar(x, y) != word[0])
            {
                return 0;
            }

            return
                TryWordAtPosition(word, x, y, -1, -1) +
                TryWordAtPosition(word, x, y, -1, 0) +
                TryWordAtPosition(word, x, y, -1, 1) +
                TryWordAtPosition(word, x, y, 1, -1) +
                TryWordAtPosition(word, x, y, 1, 0) +
                TryWordAtPosition(word, x, y, 1, 1) +
                TryWordAtPosition(word, x, y, 0, -1) +
                TryWordAtPosition(word, x, y, 0, 1);
        }

        private int TryWordAtPosition(string word, int x, int y, int stepx, int stepy)
        {
            foreach (var c in word)
            {
                if (GetChar(x, y) != c)
                {
                    return 0;
                }

                x += stepx;
                y += stepy;
            }

            return 1;
        }

        private char GetChar(int x, int y)
        {
            if (x < _left || x > _right || y < _top || y > _bottom)
            {
                return '\0';
            }

            return _contents[y][x];
        }
    }

    private static WordSearch Execute(string filename)
    {
        var lines = EmbeddedResourceReader.Read<Runner>(filename);

        return new(lines);
    }
}