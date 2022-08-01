using FluentAssertions;
using Xunit;

namespace Zxcvbn.Tests.Core
{
    public class CoreTests
    {
        [Fact]
        public void GoodScoreShallHaveNoSuggestions()
        {
            var result = Zxcvbn.Core.EvaluatePassword("turtledogspicemagic");

            result.Score.Should().BeGreaterOrEqualTo(3);
            result.Feedback.Suggestions.Count.Should().Be(0);
        }

        [Fact]
        public void GoodScoreShallHaveNoWarning()
        {
            var result = Zxcvbn.Core.EvaluatePassword("turtleturtledogspicemagic");
            result.Score.Should().BeGreaterOrEqualTo(3);
            result.Feedback.Warning.Should().Be(string.Empty);
        }

        [Fact]
        public void EmptyPasswordShallHaveZeroScore()
        {
            var result = Zxcvbn.Core.EvaluatePassword(string.Empty);
            result.Score.Should().Be(0);
        }

        [Fact]
        public void EmptyPasswordShallYieldDefaultSuggestions()
        {
            var result = Zxcvbn.Core.EvaluatePassword(string.Empty);
            var defaultFeedback = new[]
            {
                "Use a few words, avoid common phrases",
                "No need for symbols, digits, or uppercase letters",
            };
            result.Feedback.Suggestions.Should().BeEquivalentTo(defaultFeedback);
        }

        [Fact]
        public void EnglishNounsShallBeRecognisedAsSuch()
        {
            var result = Zxcvbn.Core.EvaluatePassword("geologic");
            var warning = "A word by itself is easy to guess";
            result.Feedback.Warning.Should().BeEquivalentTo(warning);
        }

        [Fact]
        public void SurnamesShallBeRecognisedAsSuch()
        {
            var result = Zxcvbn.Core.EvaluatePassword("grajeda");
            var warning = "Names and surnames by themselves are easy to guess";
            result.Feedback.Warning.Should().BeEquivalentTo(warning);
        }
        
        public static object[][] RecentYearsShallBeRecognisedAsSuchData()
        {
            return new object[][]
            {
                new object[] { 1950 },
                new object[] { 1975 },
                new object[] { 1999 },
                new object[] { 2012 },
                new object[] { 2021 },
            };
        }

        [Theory]
        [MemberData(nameof(RecentYearsShallBeRecognisedAsSuchData))]
        public void RecentYearsShallBeRecognisedAsSuch(int year)
        {
            var result = Zxcvbn.Core.EvaluatePassword($"john{year}");
            var warning = "Recent years are easy to guess";
            result.Feedback.Warning.Should().BeEquivalentTo(warning);
        }
    }
}
