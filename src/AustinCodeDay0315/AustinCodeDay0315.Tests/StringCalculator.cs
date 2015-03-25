using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace AustinCodeDay0315.Tests
{
    // Any object that allows you to add given a string expression
    public interface IStringCalculator
    {
        int Add(string numbers);

        // What if we want multiply functionality?
        int Multiply(string numbers);

        // What if we want other operations - we must must continue to create new methods with no code reuse
        // With unit tests it is possible to refactor while maintaining confidence in our work
    }

    // StringCalculator must implement all methods declared in interface
  
    public class StringCalculator:IStringCalculator
    {
        // What must our calculator do? We need to be able to add numbers from an input string
        public int Add(string expression)
        {
            NumberCalculation calculation = new AddCalculation(expression);

            return calculation.Calculate();
        }

        public int Multiply(string expression)
        {
            NumberCalculation calculation = new MultiplyCalculation(expression);

            return calculation.Calculate();
        }
    }

    public class MultiplyCalculation : NumberCalculation
    {
        public MultiplyCalculation(string expression)
            : base(expression, '*')
        {
            
        }

        public override int Calculate()
        {
            var result = 1;
            foreach (var number in Numbers)
            {
                result *= number;
            }
            return result;
        }
    }

    public class AddCalculation : NumberCalculation
    {
        public AddCalculation(string expression) :
            base(expression, '+')
        {
            
        }

        public override int Calculate()
        {
            var result = 0;
            foreach (var number in Numbers)
            {
                result += number;
            }
            return result;
        }
    }

    public abstract class NumberCalculation
    {
        protected List<int> Numbers { get; set; }
        protected char Delimiter { get; set; }

        protected NumberCalculation(String expression, char delimiter)
        {
            if (String.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentException("Expression should not be null or empty.");
            }
            Delimiter = delimiter;
            CreateNumbersList(expression);
        }

        private void CreateNumbersList(string expression)
        {
            var tokens = expression.Split(Delimiter);
            Numbers = tokens.Where(t => CanParse(t))
                .Select(t => Convert.ToInt32(t))
                .ToList();
        }

        private bool CanParse(string s)
        {
            int value = 0;
            return Int32.TryParse(s, out value);
        }

        public abstract int Calculate();
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

        [TestMethod]
        // Lets try three numbers
        public void AddingThreeNumbers()
        {
            // Arrange
            var fixture = new StringCalculatorTestFixture();
            var sut = fixture.BuildSut();
            var input = "1+2+10";

            // Act
            var output = sut.Add(input);

            // Assert
            // Lets try a cleaner way of asserting - introducting the Should library
            output.ShouldEqual(13);


        }

        [TestMethod]
        public void FourTimesTwoEqualsEight()
        {
            // Arrange
            var fixture = new StringCalculatorTestFixture();
            var sut = fixture.BuildSut();
            var input = "4*2";

            // Act
            var output = sut.Multiply(input);

            // Assert
            // Lets try a cleaner way of asserting - introducting the Should library
            output.ShouldEqual(8);
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
