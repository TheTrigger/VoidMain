using System;
using System.Linq;
using System.Reflection;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;
using Xunit;

namespace VoidMain.Application.Commands.Builder.Tests
{
    public class ArgumentModelConstructor_Should
    {
        #region Parameter tests

        [Fact]
        public void InitializeParameter()
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(
                typeof(SignatureModule), nameof(SignatureModule.Option));

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(parameter, argument.Parameter);
        }

        #endregion

        #region Type tests

        [Fact]
        public void InitializeType()
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(
                typeof(SignatureModule), nameof(SignatureModule.Option));

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(typeof(object), argument.Type);
        }

        #endregion

        #region Name tests

        [Theory]
        [InlineData(typeof(SignatureModule), nameof(SignatureModule.Option), ArgumentName)]
        [InlineData(typeof(EmptyAttributesModule), nameof(EmptyAttributesModule.Option), ArgumentName)]
        [InlineData(typeof(AttributesModule), nameof(AttributesModule.Option), AttributesModule.ArgumentName)]
        public void InitializeName(Type moduleType, string methodName, string name)
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(moduleType, methodName);

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(name, argument.Name);
        }

        #endregion

        #region Alias tests

        [Fact]
        public void InitializeAlias()
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(
                typeof(AttributesModule), nameof(AttributesModule.Option));

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(AttributesModule.ArgumentAlias.ToString(), argument.Alias);
        }

        #endregion

        #region Description tests

        [Fact]
        public void InitializeDescription()
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(
                typeof(AttributesModule), nameof(AttributesModule.Option));

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(AttributesModule.ArgumentDescription, argument.Description);
        }

        #endregion

        #region DefaultValue tests

        [Theory]
        [InlineData(typeof(DefaultValueModule), nameof(DefaultValueModule.SignatureNone), null)]
        [InlineData(typeof(DefaultValueModule), nameof(DefaultValueModule.SignatureNull), null)]
        [InlineData(typeof(DefaultValueModule), nameof(DefaultValueModule.SignatureNotNull), "")]
        [InlineData(typeof(DefaultValueModule), nameof(DefaultValueModule.AttributeNone), null)]
        [InlineData(typeof(DefaultValueModule), nameof(DefaultValueModule.AttributeNull), null)]
        [InlineData(typeof(DefaultValueModule), nameof(DefaultValueModule.AttributeNotNull), "")]
        [InlineData(typeof(AttributesModule), nameof(AttributesModule.Option), AttributesModule.ArgumentDefaultValue)]
        public void InitializeDefaultValue(Type moduleType, string methodName, object defaultValue)
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(moduleType, methodName);

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(defaultValue, argument.DefaultValue);
        }

        #endregion

        #region ValueParser tests

        [Fact]
        public void InitializeValueParser()
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(
                typeof(AttributesModule), nameof(AttributesModule.Option));

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(typeof(object), argument.ValueParser);
        }

        #endregion

        #region Kind tests

        [Theory]
        [InlineData(typeof(SignatureModule), nameof(SignatureModule.Service), ArgumentKind.Service)]
        [InlineData(typeof(SignatureModule), nameof(SignatureModule.Operand), ArgumentKind.Operand)]
        [InlineData(typeof(SignatureModule), nameof(SignatureModule.Option), ArgumentKind.Option)]
        [InlineData(typeof(AttributesModule), nameof(AttributesModule.Service), ArgumentKind.Service)]
        [InlineData(typeof(AttributesModule), nameof(AttributesModule.Operand), ArgumentKind.Operand)]
        [InlineData(typeof(AttributesModule), nameof(AttributesModule.Option), ArgumentKind.Option)]
        public void InitializeKind(Type moduleType, string methodName, ArgumentKind kind)
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(moduleType, methodName);

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(kind, argument.Kind);
        }

        #endregion

        #region Optional 

        [Theory]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.SignatureNullDefaultValue), true)]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.SignatureNotNullDefaultValue), true)]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.SignatureNullable), true)]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.SignatureParamsArray), true)]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.AttributeOptional), true)]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.AttributeNotOptional), false)]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.AttributeServiceOptional), true)]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.AttributeNullDefaultValue), false)]
        [InlineData(typeof(OptionalModule), nameof(OptionalModule.AttributeNotNullDefaultValue), true)]
        public void InitializeOptional(Type moduleType, string methodName, bool isOptional)
        {
            // Arrange
            var ctor = NewArgumentModelConstructor();
            var command = new CommandModel();
            var parameter = GetFirstParameter(moduleType, methodName);

            // Act
            var argument = ctor.Create(parameter, command);

            // Assert
            Assert.Equal(isOptional, argument.Optional);
        }

        #endregion

        #region Helpers

        private ArgumentModelConstructor NewArgumentModelConstructor()
        {
            var colCtorProvider = new CollectionConstructorProvider(new CachedTypeActivator());
            return new ArgumentModelConstructor(colCtorProvider);
        }

        private ParameterInfo GetFirstParameter(Type type, string methodName)
        {
            var method = type.GetTypeInfo().GetMethod(methodName,
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static);
            return method.GetParameters().First();
        }

        private const string ArgumentName = "value";

        public class SignatureModule
        {
            public void Service(IDisposable value) { }
            public void Operand(object[] value) { }
            public void Option(object value) { }
        }

        public class AttributesModule
        {
            public void Service([Service(Optional = ArgumentOptional)] object value)
            { }

            public void Operand([Operand(Name = ArgumentName,
                Description = ArgumentDescription, Optional = ArgumentOptional,
                DefaultValue = ArgumentDefaultValue,
                ValueParser = typeof(object))] object value)
            { }

            public void Option([Option(Name = ArgumentName, Alias = ArgumentAlias,
                Description = ArgumentDescription, Optional = ArgumentOptional,
                DefaultValue = ArgumentDefaultValue,
                ValueParser = typeof(object))] object value)
            { }

            public const string ArgumentName = "Name";
            public const char ArgumentAlias = 'a';
            public const string ArgumentDescription = "Description";
            public const bool ArgumentOptional = true;
            public const string ArgumentDefaultValue = "Description";
        }

        public class EmptyAttributesModule
        {
            public void Service([Service] object value) { }
            public void Operand([Operand] object value) { }
            public void Option([Option] object value) { }
        }

        public class DefaultValueModule
        {
            public void SignatureNone(object value) { }
            public void SignatureNull(object value = null) { }
            public void SignatureNotNull(string value = "") { }
            public void AttributeNone([Option]string value) { }
            public void AttributeNull([Option(DefaultValue = null)] object value) { }
            public void AttributeNotNull([Option(DefaultValue = "")] string value) { }
        }

        public class OptionalModule
        {
            public void SignatureNullDefaultValue(object value = null) { }
            public void SignatureNotNullDefaultValue(string value = "") { }
            public void SignatureNullable(int? value) { }
            public void SignatureParamsArray(params object[] value) { }
            public void AttributeOptional([Option(Optional = true)] object value) { }
            public void AttributeNotOptional([Option(Optional = false)] object value) { }
            public void AttributeServiceOptional([Service(Optional = true)] string value) { }
            public void AttributeNullDefaultValue([Option(DefaultValue = null)] object value) { }
            public void AttributeNotNullDefaultValue([Option(DefaultValue = "")] string value) { }
        }

        #endregion
    }
}
