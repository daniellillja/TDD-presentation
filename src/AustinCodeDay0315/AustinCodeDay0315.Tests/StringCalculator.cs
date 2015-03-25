using System;
using System.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace AustinCodeDay0315.Tests
{
    // Any object that allows you to add given a string expression
    public interface IStringCalculator
    {
        int Add(string numbers);
    }

    // StringCalculator must implement all methods declared in interface
  
    public class StringCalculator:IStringCalculator
    {
        // What must our calculator do? We need to be able to add numbers from an input string
        public int Add(string numbers)
        {
            if (String.IsNullOrWhiteSpace(numbers))
            {
                throw new ArgumentException("Expression should not be null or empty.");
            }

            var sum = 0;
            var symbols = numbers.Split('+');
            foreach (var symbol in symbols)
            {
                int parsed = 0;
                Int32.TryParse(symbol, out parsed);
                sum += parsed;
            }

            return sum;
        }
    }

    [TestClass]
    public class StringCalculatorTests
    {
        // Let's start with the simplest test case - what happens when a user enters an empty string?
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyStringShouldThrowErrorOnAdd()
        {
            // Arrange - setup your test
            var fixture = new StringCalculatorTestFixture();
            var sut = fixture.BuildSut();
            var input = string.Empty;

            // Act - lets see what the sut (system under test) returns to us
            var output = sut.Add(input);

            // Assert - what should happen under these conditions?
            // not necessary - we have covered our assert with the ExpectedExceptionAttribute above

        }

        [TestMethod]
        // Lets do a successful test case - adding two numbers
        public void OnePlusTwoEqualsThree()
        {
            // Arrange
            var fixture = new StringCalculatorTestFixture();
            var sut = fixture.BuildSut();
            var input = "1+2";

            // Act
            var output = sut.Add(input);

            // Assert
            // Lets try a cleaner way of asserting - introducting the Should library
            output.ShouldEqual(3);


        }

    }


    public class StringCalculatorTestFixture
    {
        public StringCalculator StringCalculator { get; set; }

        public StringCalculatorTestFixture()
        {
            StringCalculator = new StringCalculator();
        }

        public StringCalculator BuildSut()
        {
            return new StringCalculator();
        }
    }
}
