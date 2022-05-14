﻿using BigCalculator.Service;
using Xunit;

namespace BigCalculator.UnitTests.Service
{
    public class ParserUnitTests
    {
        [Theory]
        [InlineData("a+b*(c+d)+e", "abcd+*+e+")]
        [InlineData("a^(b-c)+d", "abc-^d+")]
        [InlineData("((a+b)/(#(c-d)))", "ab+cd-#/")]
        public void Given_Parser_When_ExpressionIsValidated_then_ExpressionIsPostfixed(string input, string expected)
        {
            //Arange
            var parser = new Parser();

            //Act
            var actualExpression = parser.MakePostfix(input);

            //Asert
            Assert.Equal(expected, actualExpression);
        }
    }
}