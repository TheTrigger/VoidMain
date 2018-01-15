using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using VoidMain.Application.Commands.Arguments;
using VoidMain.Application.Commands.Model;
using Xunit;

namespace VoidMain.Application.Commands.Tests
{
    public class ArgumentsParser_IsParserShould
    {
        #region Ctor tests

        [Fact]
        public void RequireServices()
        {
            Assert.Throws<ArgumentNullException>(() => new ArgumentsParser(null));
        }

        #endregion

        #region Services tests

        [Fact]
        public void GetRegisteredService()
        {
            // Arrange
            var parser = ParserWithServices();
            var arg = Service<IRegisteredService>("s");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
        }

        [Fact]
        public void GetNullForOptionalUnregisteredService()
        {
            // Arrange
            var parser = ParserWithServices();
            var arg = Service<IUnregisteredService>("s").IsOptional();

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands);

            // Assert
            Assert.Null(parsed[0]);
        }

        [Fact]
        public void ThrowForNonOptionalUnregisteredService()
        {
            // Arrange
            var parser = ParserWithServices();
            var arg = Service<IUnregisteredService>("s").IsOptional(false);

            // Act, Assert
            Assert.Throws<ArgumentParseException>(() => parser.Parse(Model(arg), EmptyOptions, EmptyOperands));
        }

        #endregion

        #region Options tests

        [Fact]
        public void ParseOptionByName()
        {
            // Arrange
            var parser = Parser();
            int value = 1;
            var arg = Option<int>("name");
            var options = OptionValues(
                arg.Name, new[] { value.ToString() },
                "another", new[] { "a" });

            // Act
            var parsed = parser.Parse(Model(arg), options, EmptyOperands);

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
            var arg = Option<int>("name").WithAlias("n");
            var options = OptionValues(
                arg.Alias, new[] { value.ToString() },
                "another", new[] { "a" });

            // Act
            var parsed = parser.Parse(Model(arg), options, EmptyOperands);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(value, (int)parsed[0]);
        }

        [Fact]
        public void DoNotThrowIfAliasIsNull()
        {
            // Arrange
            var parser = Parser();
            var arg = Option<int>("name").IsOptional();
            var options = OptionValues("another", new[] { "a" });

            // Act
            var parsed = parser.Parse(Model(arg), options, EmptyOperands);

            // Assert
            Assert.Equal(default(int), parsed[0]);
        }

        [Fact]
        public void GetLastValueForOption()
        {
            // Arrange
            var parser = Parser();
            int value = 1;
            var arg = Option<int>("name");
            var options = OptionValues(arg.Name, new[] { "a", "b", value.ToString() });

            // Act
            var parsed = parser.Parse(Model(arg), options, EmptyOperands);

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
            var arg = Operand<int>("name");
            string[] operands = { values[0].ToString(), values[1].ToString() };

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands);

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
            var arg0 = Operand<int>("name1");
            var arg1 = Operand<int>("name2");
            string[] operands = { values[0].ToString(), values[1].ToString() };

            // Act
            var parsed = parser.Parse(Model(arg0, arg1), EmptyOptions, operands);

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
            var arg = Operand<int>("name").IsOptional();

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(default(int), (int)parsed[0]);
        }

        [Fact]
        public void ThrowIfNoDefaultAndNotOptional()
        {
            // Arrange
            var parser = Parser();
            var arg = Operand<int>("name").IsOptional(false);

            // Act, Assert
            Assert.Throws<ArgumentParseException>(() => parser.Parse(Model(arg), EmptyOptions, EmptyOperands));
        }

        [Fact]
        public void ReturnSingleDefaultValueIfTypeIsMatch()
        {
            // Arrange
            var parser = Parser();
            int defaultValue = 11;
            var arg = Operand<int>("name").HasDefaultValue(defaultValue);

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands);

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
            var arg = Operand<int[]>("name").HasDefaultValue(defaultValue);

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, EmptyOperands);
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
            var arg = Operand<int>("name").HasDefaultValue(new string[0]);

            // Act, Assert
            Assert.Throws<ArgumentParseException>(() => parser.Parse(Model(arg), EmptyOptions, EmptyOperands));
        }

        [Fact]
        public void ThrowIfDefaultValueTypeNotMatch()
        {
            // Arrange
            var parser = Parser();
            var arg = Operand<int>("name").HasDefaultValue(new double[0]);

            // Act, Assert
            Assert.Throws<ArgumentParseException>(() => parser.Parse(Model(arg), EmptyOptions, EmptyOperands));
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
            var arg = Operand<string[]>("name");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands);
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
            var arg = Operand<int[]>("name");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands);

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
            var arg = Operand<List<int>>("name");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands);

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
            var arg = Operand<string>("name");

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands);

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
            var arg = Operand<int>("name");
            string[] operands = { value.ToString() };

            // Act
            var parsed = parser.Parse(Model(arg), EmptyOptions, operands);

            // Assert
            Assert.IsAssignableFrom(arg.Type, parsed[0]);
            Assert.Equal(value, (int)parsed[0]);
        }

        #endregion

        #region Helpers

        private static ArgumentsParser Parser()
        {
            var collection = new ServiceCollection();
            var services = collection.BuildServiceProvider();
            return new ArgumentsParser(services);
        }

        private static ArgumentsParser ParserWithServices()
        {
            var collection = new ServiceCollection();
            collection.AddTransient<IRegisteredService, ServiceImpl>();
            var services = collection.BuildServiceProvider();
            return new ArgumentsParser(services);
        }

        public interface IRegisteredService { }
        public interface IUnregisteredService { }
        public class ServiceImpl : IRegisteredService, IUnregisteredService { }

        private static readonly Dictionary<string, string[]> EmptyOptions
            = new Dictionary<string, string[]>();
        private static readonly string[] EmptyOperands = new string[0];

        private static Dictionary<string, string[]> OptionValues(
            string name, string[] values)
        {
            var options = new Dictionary<string, string[]>();
            options.Add(name, values);
            return options;
        }

        private static Dictionary<string, string[]> OptionValues(
            string name1, string[] values1,
            string name2, string[] values2)
        {
            var options = new Dictionary<string, string[]>();
            options.Add(name1, values1);
            options.Add(name2, values2);
            return options;
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
