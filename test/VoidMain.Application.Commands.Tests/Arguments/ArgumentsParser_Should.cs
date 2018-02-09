using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using VoidMain.Application.Commands.Model;
using Xunit;

namespace VoidMain.Application.Commands.Arguments.Tests
{
    public class ArgumentsParser_Should
    {
        #region Ctor tests

        [Fact]
        public void RequireCollectionConstructorProvider()
        {
            var parserProvider = new Mock<IValueParserProvider>().Object;
            Assert.Throws<ArgumentNullException>(() => new ArgumentsParser(null, parserProvider));
        }

        [Fact]
        public void RequireValueParserProvider()
        {
            var colCtorProvider = new Mock<ICollectionConstructorProvider>().Object;
            Assert.Throws<ArgumentNullException>(() => new ArgumentsParser(colCtorProvider, null));
        }

        #endregion

        #region Valid parameters

        [Fact]
        public void RequireValidArgumentsModel()
        {
            var parser = Parser();
            Assert.Throws<ArgumentNullException>(() => parser.Parse(null, EmptyOptions, EmptyOperands, EmptyServices));
        }

        [Fact]
        public void RequireValidOptions()
        {
            var parser = Parser();
            Assert.Throws<ArgumentNullException>(() => parser.Parse(Model(), null, EmptyOperands, EmptyServices));
        }

        [Fact]
        public void RequireValidOperands()
        {
            var parser = Parser();
            Assert.Throws<ArgumentNullException>(() => parser.Parse(Model(), EmptyOptions, null, EmptyServices));
        }

        [Fact]
        public void RequireValidServices()
        {
            var parser = Parser();
            Assert.Throws<ArgumentNullException>(() => parser.Parse(Model(), EmptyOptions, EmptyOperands, null));
        }

        #endregion

        #region Services tests

        [Fact]
        public void GetRegisteredService()
        {
            // Arrange
            var parser = Parser();
            var services = Services();
            var arg = Service<IRegisteredService>("s");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands, services);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
        }

        [Fact]
        public void GetNullForOptionalUnregisteredService()
        {
            // Arrange
            var parser = Parser();
            var services = Services();
            var arg = Service<IUnregisteredService>("s").IsOptional();

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands, services);

            // Assert
            Assert.Null(parsed[0]);
        }

        [Fact]
        public void ThrowForNonOptionalUnregisteredService()
        {
            // Arrange
            var parser = Parser();
            var services = Services();
            var arg = Service<IUnregisteredService>("s").IsOptional(false);

            // Act, Assert
            Assert.Throws<ArgumentParseException>(() => parser.Parse(Model(arg), EmptyOptions, EmptyOperands, services));
        }

        #endregion

        #region Options tests

        [Fact]
        public void ParseOptionByName()
        {
            // Arrange
            var parser = Parser();
            int value = 1;
            var arg = Option<int>("value");
            var options = OptionValues(
                (arg.Name, value.ToString()),
                ("another", "a"));

            // Act
            var parsed = parser.Parse(Model(arg), options, EmptyOperands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(value, (int)parsed[0]);
        }

        [Fact]
        public void ParseOptionByAlias()
        {
            // Arrange
            var parser = Parser();
            int value = 1;
            var arg = Option<int>("value").WithAlias("n");
            var options = OptionValues(
                (arg.Alias, value.ToString()),
                ("another", "a"));

            // Act
            var parsed = parser.Parse(Model(arg), options, EmptyOperands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(value, (int)parsed[0]);
        }

        [Fact]
        public void DoNotThrowIfAliasIsNull()
        {
            // Arrange
            var parser = Parser();
            var arg = Option<int>("value").IsOptional();
            var options = OptionValues(("another", "a"));

            // Act
            var parsed = parser.Parse(Model(arg), options, EmptyOperands, EmptyServices);

            // Assert
            Assert.Equal(default(int), parsed[0]);
        }

        [Fact]
        public void GetLastValueForOption()
        {
            // Arrange
            var parser = Parser();
            int value = 1;
            var arg = Option<int>("value");
            var options = OptionValues(
                (arg.Name, "a"),
                (arg.Name, "b"),
                (arg.Name, value.ToString()));

            // Act
            var parsed = parser.Parse(Model(arg), options, EmptyOperands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(value, (int)parsed[0]);
        }

        #endregion

        #region Operands tests

        [Fact]
        public void GetFirstValueForOperand()
        {
            // Arrange
            var parser = Parser();
            int[] values = { 1, 2 };
            var arg = Operand<int>("value");
            string[] operands = { values[0].ToString(), values[1].ToString() };

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(values[0], (int)parsed[0]);
        }

        [Fact]
        public void GetNextValueForOperand()
        {
            // Arrange
            var parser = Parser();
            int[] values = { 1, 2 };
            var arg0 = Operand<int>("value1");
            var arg1 = Operand<int>("value2");
            string[] operands = { values[0].ToString(), values[1].ToString() };

            // Act
            var parsed = parser.Parse(Model(arg0, arg1), EmptyOptions, operands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg1.Type, parsed[1]);
            Assert.Equal(values[1], (int)parsed[1]);
        }

        #endregion

        #region DefaultValue tests

        [Fact]
        public void ReturnEmptyValueIfNoDefaultAndOptional()
        {
            // Arrange
            var parser = Parser();
            var arg = Operand<int>("value").IsOptional();

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(default(int), (int)parsed[0]);
        }

        [Fact]
        public void ThrowIfNoDefaultAndNotOptional()
        {
            // Arrange
            var parser = Parser();
            var arg = Operand<int>("value").IsOptional(false);

            // Act, Assert
            Assert.Throws<ArgumentParseException>(() => parser.Parse(Model(arg), EmptyOptions, EmptyOperands, EmptyServices));
        }

        [Fact]
        public void ReturnSingleDefaultValueIfTypeIsMatch()
        {
            // Arrange
            var parser = Parser();
            int defaultValue = 11;
            var arg = Operand<int>("value").HasDefaultValue(defaultValue);

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(defaultValue, (int)parsed[0]);
        }

        [Fact]
        public void ReturnArrayCopyOfDefaultValueIfTypeIsMatch()
        {
            // Arrange
            var parser = Parser();
            int[] defaultValue = { 1, 2, 3 };
            int[] unmodified = { 1, 2, 3 };
            int[] modified = { 5, 2, 3 };
            var arg = Operand<int[]>("value").HasDefaultValue(defaultValue);

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands, EmptyServices);
            int[] array = (int[])parsed[0];
            array[0] = 5;

            // Assert
            Assert.Equal(modified, array);
            Assert.Equal(unmodified, defaultValue);
        }

        [Fact]
        public void ThrowIfDefaultValueIsEmptyStringArrayAndTypeNotMatch()
        {
            // Arrange
            var parser = Parser();
            var arg = Operand<int>("value").HasDefaultValue(new string[0]);

            // Act, Assert
            Assert.Throws<ArgumentParseException>(() => parser.Parse(Model(arg), EmptyOptions, EmptyOperands, EmptyServices));
        }

        [Fact]
        public void ThrowIfDefaultValueTypeNotMatch()
        {
            // Arrange
            var parser = Parser();
            var arg = Operand<int>("value").HasDefaultValue(new double[0]);

            // Act, Assert
            Assert.Throws<ArgumentParseException>(() => parser.Parse(Model(arg), EmptyOptions, EmptyOperands, EmptyServices));
        }

        #endregion

        #region ParseValue tests

        [Fact]
        public void ReturnArrayCopyOfValuesIfTypeIsStringArray()
        {
            // Arrange
            var parser = Parser();
            string[] operands = { "a", "b", "c" };
            string[] unmodified = { "a", "b", "c" };
            string[] modified = { "X", "b", "c" };
            var arg = Operand<string[]>("value");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands, EmptyServices);
            string[] array = (string[])parsed[0];
            array[0] = "X";

            // Assert
            Assert.Equal(modified, array);
            Assert.Equal(unmodified, operands);
        }

        [Fact]
        public void ParseAndFillArrayArgument()
        {
            // Arrange
            var parser = Parser();
            string[] operands = { "1", "2", "3" };
            int[] expected = { 1, 2, 3 };
            var arg = Operand<int[]>("value");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(expected, (int[])parsed[0]);
        }

        [Fact]
        public void ParseAndFillGenericCollectionArgument()
        {
            // Arrange
            var parser = Parser();
            string[] operands = { "1", "2", "3" };
            var expected = new List<int>(new[] { 1, 2, 3 });
            var arg = Operand<List<int>>("value");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(expected, (List<int>)parsed[0]);
        }

        [Fact]
        public void ReturnSingleValueIfTypeIsString()
        {
            // Arrange
            var parser = Parser();
            string[] operands = { "a" };
            var arg = Operand<string>("value");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(operands[0], (string)parsed[0]);
        }

        [Fact]
        public void ParseSingleValue()
        {
            // Arrange
            var parser = Parser();
            int value = 1;
            var arg = Operand<int>("value");
            string[] operands = { value.ToString() };

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(value, (int)parsed[0]);
        }

        #endregion

        #region FormatProvider tests

        [Theory]
        [InlineData(3.14, "en-US")]
        [InlineData(3.14, "ru-RU")]
        public void ParseDoubleValuesWithDifferentDecimalSeparator(double value, string culture)
        {
            // Arrange
            var cultureInfo = new CultureInfo(culture);
            var parser = Parser(new ArgumentsParserOptions { FormatProvider = cultureInfo });
            var arg = Operand<double>("value");
            string[] operands = { value.ToString(cultureInfo) };

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands, EmptyServices);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(value, (double)parsed[0]);
        }

        #endregion

        #region Helpers

        private static ArgumentsParser Parser(ArgumentsParserOptions options = null)
        {
            var colCtorProvider = new CollectionConstructorProvider();
            var parserProvider = new ValueParserProvider();
            return new ArgumentsParser(colCtorProvider, parserProvider, options);
        }

        public interface IRegisteredService { }
        public interface IUnregisteredService { }
        public class ServiceImpl : IRegisteredService, IUnregisteredService { }

        private static readonly IServiceProvider EmptyServices = new Mock<IServiceProvider>().Object;

        private static IServiceProvider Services()
        {
            var collection = new ServiceCollection();
            collection.AddTransient<IRegisteredService, ServiceImpl>();
            return collection.BuildServiceProvider();
        }

        private static readonly KeyValuePair<string, string>[] EmptyOptions
            = Array.Empty<KeyValuePair<string, string>>();
        private static readonly string[] EmptyOperands = new string[0];

        private static Dictionary<string, string[]> OptionValues(
            string name, string[] values)
        {
            var options = new Dictionary<string, string[]>();
            options.Add(name, values);
            return options;
        }

        private static KeyValuePair<string, string>[] OptionValues(
            params (string name, string value)[] options)
        {
            var buffer = new List<KeyValuePair<string, string>>();

            foreach (var option in options)
            {
                buffer.Add(new KeyValuePair<string, string>(option.name, option.value));
            }

            return buffer.ToArray();
        }

        private static ArgumentModel[] Model(params ArgumentModel[] args)
        {
            return args;
        }

        private static ArgumentModel Service<T>(string name)
        {
            return new ArgumentModel
            {
                Kind = ArgumentKind.Service,
                Type = typeof(T),
                Name = name
            };
        }

        private static ArgumentModel Option<T>(string name)
        {
            return new ArgumentModel
            {
                Kind = ArgumentKind.Option,
                Type = typeof(T),
                Name = name
            };
        }

        private static ArgumentModel Operand<T>(string name)
        {
            return new ArgumentModel
            {
                Kind = ArgumentKind.Operand,
                Type = typeof(T),
                Name = name
            };
        }

        #endregion
    }

    public static class ModelExtensions
    {
        public static ArgumentModel WithAlias(
               this ArgumentModel model, string value)
        {
            model.Alias = value;
            return model;
        }

        public static ArgumentModel IsOptional(
            this ArgumentModel model, bool value = true)
        {
            model.Optional = value;
            return model;
        }

        public static ArgumentModel HasDefaultValue(
            this ArgumentModel model, object value)
        {
            model.DefaultValue = value;
            return model;
        }
    }
}
