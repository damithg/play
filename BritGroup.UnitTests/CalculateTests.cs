using System;
using BritGroup.Application;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace BritGroup.UnitTests
{
    public class CalculateTests
    {
        [Theory(DisplayName = "GIVEN a valid Instruction, " +
                              "WHEN applying the instruction, " +
                              "THEN calculate the given instructions ")]
        [InlineData("addition 2 apply 3", "3", 5)]
        [InlineData("addition 2 multiplication 2 apply 3", "3", 10)]
        public void Calculate_ValidInstructions(string input, string initialValue, double expected)
        {
            var parser = new ParseNumbers(initialValue);

            var result = parser.Parse(input);
            result.ShouldBe(expected);
        }

        [Theory(DisplayName = "GIVEN an invalid Instruction, " +
                              "WHEN applying the instruction, " +
                              "THEN an exception is thrown. ")]
        [InlineData("addition 2 addition 3", "3")]
        public void Calculate_InvalidInstructions(string input, string initialValue)
        {
            var parser = new ParseNumbers(initialValue);

            var exception = Record.Exception(() => parser.Parse(input));
            exception.Should().BeOfType<ArgumentException>();
            exception!.Message.Should().Contain("Invalid request");
        }
    }
}
