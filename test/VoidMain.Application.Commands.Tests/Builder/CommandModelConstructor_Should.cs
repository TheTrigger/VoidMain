using Moq;
using System;
using System.Reflection;
using VoidMain.Application.Commands.Model;
using Xunit;

namespace VoidMain.Application.Commands.Builder.Tests
{
    public class CommandModelConstructor_Should
    {
        #region Ctor tests

        [Fact]
        public void RequireCommandNameParser()
        {
            var argumentConstructor = new Mock<IArgumentModelConstructor>().Object;
            Assert.Throws<ArgumentNullException>(() => new CommandModelConstructor(null, argumentConstructor));
        }

        [Fact]
        public void RequireArgumentModelConstructor()
        {
            var commandNameParser = new Mock<ICommandNameParser>().Object;
            Assert.Throws<ArgumentNullException>(() => new CommandModelConstructor(commandNameParser, null));
        }

        #endregion

        #region Method tests

        [Fact]
        public void InitializeMethod()
        {
            // Arrange
            var ctor = NewCommandModelConstructor();
            var module = GetModule(typeof(SignatureModule));
            Action<string> func = new SignatureModule().Command;

            // Act
            var command = ctor.Create(func.Method, module);

            // Assert
            Assert.Equal(func.Method, command.Method);
        }

        [Theory]
        [InlineData(nameof(InvalidMethodsModule.Private), "The command must be a public method.")]
        [InlineData(nameof(InvalidMethodsModule.Static), "The command must be a non-static method.")]
        [InlineData(nameof(InvalidMethodsModule.Abstract), "The command must be a non-abstract method.")]
        [InlineData(nameof(InvalidMethodsModule.Generic), "The command must be a non-generic method.")]
        [InlineData("get_" + nameof(InvalidMethodsModule.Value), "The command must not be a compile generated method.")]
        [InlineData(nameof(InvalidMethodsModule.Equals), "The command must not be one of the object's base methods.")]
        [InlineData(nameof(InvalidMethodsModule.Dispose), "The command must not be the IDisposable.Dispose method.")]
        [InlineData(nameof(InvalidMethodsModule.NonCommand), "The command must not have a NonCommand attribute.")]
        public void AcceptValidMethod(string methodName, string expectedReason)
        {
            // Arrange
            var ctor = NewCommandModelConstructor();
            var moduleType = typeof(InvalidMethodsModule);
            var module = GetModule(moduleType);
            var method = GetMethod(moduleType, methodName);

            // Act, Assert
            var ex = Assert.Throws<ArgumentException>(() => ctor.Create(method, module));
            string errorMessage = ex.Message;
            int start = errorMessage.IndexOf(':') + 2;
            int length = errorMessage.IndexOf(Environment.NewLine) - start;
            string actualReason = errorMessage.Substring(start, length);
            Assert.Equal(expectedReason, actualReason);
        }

        [Theory]
        [InlineData(typeof(SignatureModule), nameof(SignatureModule.Command), true)]
        [InlineData(typeof(AttributesModule), nameof(AttributesModule.Command), true)]
        [InlineData(typeof(InvalidMethodsModule), nameof(InvalidMethodsModule.Private), false)]
        [InlineData(typeof(InvalidMethodsModule), nameof(InvalidMethodsModule.Static), false)]
        [InlineData(typeof(InvalidMethodsModule), nameof(InvalidMethodsModule.Abstract), false)]
        [InlineData(typeof(InvalidMethodsModule), nameof(InvalidMethodsModule.Generic), false)]
        [InlineData(typeof(InvalidMethodsModule), "get_" + nameof(InvalidMethodsModule.Value), false)]
        [InlineData(typeof(InvalidMethodsModule), nameof(InvalidMethodsModule.Equals), false)]
        [InlineData(typeof(InvalidMethodsModule), nameof(InvalidMethodsModule.Dispose), false)]
        [InlineData(typeof(InvalidMethodsModule), nameof(InvalidMethodsModule.NonCommand), false)]
        public void DetermineValidMethod(Type moduleType, string methodName, bool expected)
        {
            // Arrange
            var ctor = NewCommandModelConstructor();
            var module = GetModule(moduleType);
            var method = GetMethod(moduleType, methodName);

            // Act
            var actual = ctor.IsCommand(method, module);

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Module tests

        [Fact]
        public void InitializeModule()
        {
            // Arrange
            var ctor = NewCommandModelConstructor();
            var module = GetModule(typeof(SignatureModule));
            Action<string> func = new SignatureModule().Command;

            // Act
            var command = ctor.Create(func.Method, module);

            // Assert
            Assert.Equal(module, command.Module);
        }

        #endregion

        #region Arguments tests

        [Fact]
        public void InitializeArgumentsCollection()
        {
            // Arrange
            var ctor = NewCommandModelConstructor();
            var module = GetModule(typeof(SignatureModule));
            Action<string> func = new SignatureModule().Command;

            // Act
            var command = ctor.Create(func.Method, module);

            // Assert
            Assert.NotNull(command.Arguments);
        }

        #endregion

        #region Name tests

        [Theory]
        [InlineData(typeof(SignatureModule), nameof(SignatureModule.Command), "Signature", nameof(SignatureModule.Command))]
        [InlineData(typeof(EmptyAttributesModule), nameof(EmptyAttributesModule.Command), "EmptyAttributes", nameof(EmptyAttributesModule.Command))]
        [InlineData(typeof(AttributesModule), nameof(AttributesModule.Command), AttributesModule.ModuleName, AttributesModule.CommandName)]
        [InlineData(typeof(SignatureModule), nameof(SignatureModule.Command), "", nameof(SignatureModule.Command))]
        public void InitializeName(Type moduleType, string methodName, string moduleName, string commandName)
        {
            // Arrange
            var ctor = NewCommandModelConstructor();
            var module = GetModule(moduleType);
            module.Name = moduleName;
            var method = GetMethod(moduleType, methodName);
            var name = CommandNameParser.Parse(moduleName + " " + commandName);

            // Act
            var command = ctor.Create(method, module);

            // Assert
            Assert.Equal(name, command.Name, Comparer);
        }

        #endregion

        #region Description tests

        [Fact]
        public void InitializeDescription()
        {
            // Arrange
            var ctor = NewCommandModelConstructor();
            var moduleType = typeof(AttributesModule);
            var module = GetModule(moduleType);
            Action<string> func = new AttributesModule().Command;

            // Act
            var command = ctor.Create(func.Method, module);

            // Assert
            Assert.Equal(AttributesModule.CommandDescription, command.Description);
        }

        #endregion

        #region Helpers

        private static readonly CommandNameComparer Comparer
            = new CommandNameComparer(StringComparer.InvariantCultureIgnoreCase);
        private static readonly IModuleModelConstructor ModuleConstructor
            = new ModuleModelConstructor();
        private static readonly ICommandNameParser CommandNameParser
            = new CommandNameParser();

        private CommandModelConstructor NewCommandModelConstructor()
        {
            var ctor = new Mock<IArgumentModelConstructor>();
            ctor.Setup(c => c.Create(It.IsAny<ParameterInfo>(), It.IsAny<CommandModel>()))
                .Returns(new ArgumentModel());
            return new CommandModelConstructor(CommandNameParser, ctor.Object);
        }

        private ModuleModel GetModule(Type type)
        {
            return new ModuleModel() { Type = type };
        }

        private MethodInfo GetMethod(Type type, string methodName)
        {
            return type.GetTypeInfo().GetMethod(methodName,
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static);
        }

        public class SignatureModule
        {
            public void Command(string value) { }
        }

        [Module(Name = ModuleName)]
        public class AttributesModule
        {
            [Command(Name = CommandName, Description = CommandDescription)]
            public void Command(string value) { }

            public const string ModuleName = "ModuleName";
            public const string CommandName = "CommandName";
            public const string CommandDescription = "Description";
        }

        public class EmptyAttributesModule
        {
            [Command]
            public void Command(string value) { }
        }

        public abstract class InvalidMethodsModule : IDisposable
        {
            internal void Private() { }
            public static void Static() { }
            public abstract void Abstract();
            public void Generic<T>() { }
            public string Value { get; }
            public void Dispose() { }
            [NonCommand]
            public void NonCommand() { }
        }

        #endregion
    }
}
