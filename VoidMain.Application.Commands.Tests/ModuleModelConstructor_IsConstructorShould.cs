using System;
using System.Reflection;
using VoidMain.Application.Commands.Builder;
using Xunit;

namespace VoidMain.Application.Commands.Tests
{
    public class ModuleModelConstructor_IsConstructorShould
    {
        [Fact]
        public void InitializeType()
        {
            // Arrange
            var ctor = new ModuleModelConstructor();
            var moduleType = typeof(SignatureModule);

            // Act
            var module = ctor.Create(moduleType.GetTypeInfo());

            // Assert
            Assert.Equal(moduleType, module.Type);
        }

        [Fact]
        public void InitializeEmptyCommandsCollection()
        {
            // Arrange
            var ctor = new ModuleModelConstructor();
            var moduleType = typeof(SignatureModule);

            // Act
            var module = ctor.Create(moduleType.GetTypeInfo());

            // Assert
            Assert.NotNull(module.Commands);
            Assert.Empty(module.Commands);
        }

        [Theory]
        [InlineData(typeof(SignatureModule), "Signature")]
        [InlineData(typeof(EmptyAttributesModule), "EmptyAttributes")]
        [InlineData(typeof(AttributesModule), AttributesModule.ModuleName)]
        public void InitializeName(Type moduleType, string moduleName)
        {
            // Arrange
            var ctor = new ModuleModelConstructor();

            // Act
            var module = ctor.Create(moduleType.GetTypeInfo());

            // Assert
            Assert.Equal(moduleName, module.Name);
        }

        [Fact]
        public void InitializeDescription()
        {
            // Arrange
            var ctor = new ModuleModelConstructor();
            var moduleType = typeof(AttributesModule);

            // Act
            var module = ctor.Create(moduleType.GetTypeInfo());

            // Assert
            Assert.Equal(AttributesModule.ModuleDescription, module.Description);
        }

        [Theory]
        [InlineData(typeof(StructModule), "The module must be a class.")]
        [InlineData(typeof(PrivateModule), "The module must be a public class.")]
        [InlineData(typeof(AbstractModule), "The module must be a non-abstract class.")]
        [InlineData(typeof(StaticModule), "The module must be a non-abstract class.")]
        [InlineData(typeof(GenericModule<>), "All generic parameters for the module must be specified.")]
        [InlineData(typeof(NonModule), "The module must not have a NonModule attribute.")]
        public void AcceptValidTypes(Type moduleType, string expectedReason)
        {
            // Arrange
            var ctor = new ModuleModelConstructor();

            // Act, Assert
            var ex = Assert.Throws<ArgumentException>(() => ctor.Create(moduleType.GetTypeInfo()));
            string errorMessage = ex.Message;
            int start = errorMessage.IndexOf(':') + 2;
            int length = errorMessage.IndexOf(Environment.NewLine) - start;
            string actualReason = errorMessage.Substring(start,length);
            Assert.Equal(expectedReason, actualReason);
        }

        [Theory]
        [InlineData(typeof(SignatureModule), true)]
        [InlineData(typeof(AttributesModule), true)]
        [InlineData(typeof(GenericModule<object>), true)]
        [InlineData(typeof(StructModule), false)]
        [InlineData(typeof(PrivateModule), false)]
        [InlineData(typeof(AbstractModule), false)]
        [InlineData(typeof(StaticModule), false)]
        [InlineData(typeof(GenericModule<>), false)]
        [InlineData(typeof(NonModule), false)]
        public void DetermineValidType(Type moduleType, bool expected)
        {
            // Arrange
            var ctor = new ModuleModelConstructor();

            // Act
            var actual = ctor.IsModule(moduleType.GetTypeInfo());

            // Assert
            Assert.Equal(expected, actual);
        }

        public class SignatureModule
        {
        }

        [Module(Name = ModuleName, Description = ModuleDescription)]
        public class AttributesModule
        {
            public const string ModuleName = "Name";
            public const string ModuleDescription = "Description";
        }

        [Module]
        public class EmptyAttributesModule
        {
        }

        public struct StructModule { }
        private class PrivateModule { }
        public abstract class AbstractModule { }
        public static class StaticModule { }
        public class GenericModule<T> { }
        [NonModule]
        public class NonModule { }
    }
}
