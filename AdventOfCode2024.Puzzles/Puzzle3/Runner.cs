using AdventOfCode2024.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2024.Puzzles.Puzzle3;

public class Runner
{
    [Theory]
    [InlineData("TestInput1", 161)]
    [InlineData("Input", 178886550)]
    public void RunAlpha(string filename, int expected)
    {
        var muls = Execute(filename, false);
        var actual = muls.Select(x => x.Calculate()).Sum();
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput2", 48)]
    [InlineData("Input", 87163705)]
    public void RunBeta(string filename, int expected)
    {
        var muls = Execute(filename, true);
        var actual = muls.Select(x => x.Calculate()).Sum();
        actual.Should().Be(expected);
    }

    public sealed record MulInstruction(int a, int b)
    {
        public int Calculate() => a * b;
    }

    private static IEnumerable<MulInstruction> Execute(string filename, bool enableDoDont)
    {
        var lines = EmbeddedResourceReader.Read<Runner>(filename);

        return ParseLine(string.Join("", lines), enableDoDont).ToList();
    }

    private static IEnumerable<MulInstruction> ParseLine(string line, bool enableDoDont)
    {
        char[] numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

        var pos = -1;
        var enabled = true;
        while (true)
        {
            pos++;
            if (pos >= line.Length)
            {
                break;
            }

            if (ParseChar(line, pos, 'd') != null &&
                ParseChar(line, pos + 1, 'o') != null &&
                ParseChar(line, pos + 2, '(') != null &&
                ParseChar(line, pos + 3, ')') != null)
            {
                enabled = true;
                continue;
            }

            if (ParseChar(line, pos, 'd') != null &&
                ParseChar(line, pos + 1, 'o') != null &&
                ParseChar(line, pos + 2, 'n') != null &&
                ParseChar(line, pos + 3, '\'') != null &&
                ParseChar(line, pos + 4, 't') != null &&
                ParseChar(line, pos + 5, '(') != null &&
                ParseChar(line, pos + 6, ')') != null)
            {
                enabled = false;
                continue;
            }

            if (!enabled && enableDoDont)
            {
                continue;
            }

            var m = ParseChar(line, pos, 'm');
            var u = ParseChar(line, pos + 1, 'u');
            var l = ParseChar(line, pos + 2, 'l');
            var parenthesisOpen = ParseChar(line, pos + 3, '(');
            var number1 = ParseToken(line, pos + 4, numbers);
            var comma = ParseChar(line, pos + 4 + (number1?.Length ?? 0), ',');
            var number2 = ParseToken(line, pos + 5 + (number1?.Length ?? 0), numbers);
            var parenthesisClose = ParseChar(line, pos + 5 + (number1?.Length ?? 0) + (number2?.Length ?? 0), ')');

            if (m != null &&
                u != null &&
                l != null &&
                parenthesisOpen != null &&
                number1 != null &&
                int.TryParse(number1, out var no1) &&
                comma != null &&
                number2 != null &&
                int.TryParse(number2, out var no2) &&
                parenthesisClose != null)
            {
                yield return new(no1, no2);
            }
        }

        yield break;

        static string? ParseChar(string line, int position, char @char)
        {
            if (position >= line.Length)
            {
                return null;
            }

            if (line[position] == @char)
            {
                return new string([@char]);
            }

            return null;
        }

        static string? ParseToken(string line, int position, char[] chars)
        {
            var pos = position;
            List<char> foundChars = new();
            while (true)
            {
                if (pos >= line.Length)
                {
                    break;
                }

                if (!chars.Contains(line[pos]))
                {
                    break;
                }

                foundChars.Add(line[pos]);
                pos++;

            }

            if (foundChars.Any())
            {
                return new(foundChars.ToArray());
            }

            return null;
        }
    }
}