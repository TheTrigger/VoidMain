using System;
using System.Collections.Generic;
using Xunit;

namespace VoidMain.Application.Commands.Arguments.Tests
{
    public class ArrayConstructor_Should
    {
        #region GetElementType

        [Theory]
        [InlineData(typeof(IEnumerable<int>), typeof(int))]
        [InlineData(typeof(ICollection<int>), typeof(int))]
        [InlineData(typeof(IReadOnlyCollection<int>), typeof(int))]
        [InlineData(typeof(IList<int>), typeof(int))]
        [InlineData(typeof(IReadOnlyList<int>), typeof(int))]
        [InlineData(typeof(int[]), typeof(int))]
        public void GetElementTypeOfValidCollectionType(Type collectionType, Type elementType)
        {
            // Arrange
            var ctor = new ArrayConstructor();

            // Act
            var actual = ctor.GetElementType(collectionType);

            // Assert
            Assert.Equal(elementType, actual);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void ThrowOnGetElementTypeOfInvalidCollectionType(Type collectionType)
        {
            // Arrange
            var ctor = new ArrayConstructor();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => ctor.GetElementType(collectionType));
        }

        #endregion

        #region Create

        [Fact]
        public void CreateArrayWithSpecifiedElementType()
        {
            // Arrange
            var ctor = new ArrayConstructor();

            // Act
            var array = ctor.Create(typeof(int), 2).Collection;

            // Assert
            Assert.IsType<int[]>(array);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateArrayWithSpecifiedCount(int count)
        {
            // Arrange
            var ctor = new ArrayConstructor();

            // Act
            var array = (int[])ctor.Create(typeof(int), count).Collection;

            // Assert
            Assert.Equal(count, array.Length);
        }

        #endregion

        #region Wrap

        [Fact]
        public void WrapValidArray()
        {
            // Arrange
            var ctor = new ArrayConstructor();
            var array = Array.Empty<int>();

            // Act
            var adapter = ctor.Wrap(array);

            // Assert
            Assert.NotNull(adapter.Collection);
        }

        [Fact]
        public void ThrowOnWrapInvalidArray()
        {
            // Arrange
            var ctor = new ArrayConstructor();
            var notArray = new object();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => ctor.Wrap(notArray));
        }

        #endregion

        #region Adapter

        #region GetValue

        public static IEnumerable<object[]> Arrays = new[]
        {
            new object[]{new[] { 1, 2, 3 }, 0, 1 },
            new object[]{new[] { 1, 2, 3 }, 2, 3 }
        };

        [Theory]
        [MemberData(nameof(Arrays))]
        public void GetValue(int[] array, int index, int expected)
        {
            // Arrange
            var ctor = new ArrayConstructor();
            var adapter = ctor.Wrap(array);

            // Act
            var value = adapter.GetValue(index);

            // Assert
            Assert.Equal(expected, value);
        }

        #endregion

        #region SetValue

        [Theory]
        [InlineData(0, 1)]
        [InlineData(2, 1)]
        public void SetValue(int index, int expected)
        {
            // Arrange
            var ctor = new ArrayConstructor();
            var adapter = ctor.Create(typeof(int), 3);

            // Act
            adapter.SetValue(index, expected);
            var value = ((int[])adapter.Collection).GetValue(index);

            // Assert
            Assert.Equal(expected, value);
        }

        #endregion

        #endregion
    }
}
