using System;
using System.Collections.Generic;
using Xunit;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors.Tests
{
    public class ListConstructor_Should
    {
        #region GetElementType

        [Theory]
        [InlineData(typeof(IEnumerable<int>), typeof(int))]
        [InlineData(typeof(ICollection<int>), typeof(int))]
        [InlineData(typeof(IReadOnlyCollection<int>), typeof(int))]
        [InlineData(typeof(IList<int>), typeof(int))]
        [InlineData(typeof(IReadOnlyList<int>), typeof(int))]
        [InlineData(typeof(List<int>), typeof(int))]
        public void GetElementTypeOfValidCollectionType(Type collectionType, Type elementType)
        {
            // Arrange
            var ctor = new ListConstructor();

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
            var ctor = new ListConstructor();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => ctor.GetElementType(collectionType));
        }

        #endregion

        #region Create

        [Fact]
        public void CreateListWithSpecifiedElementType()
        {
            // Arrange
            var ctor = new ListConstructor();

            // Act
            var list = ctor.Create(typeof(int), 2).Collection;

            // Assert
            Assert.IsType<List<int>>(list);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateListWithSpecifiedCount(int count)
        {
            // Arrange
            var ctor = new ListConstructor();

            // Act
            var list = (List<int>)ctor.Create(typeof(int), count).Collection;

            // Assert
            Assert.Equal(count, list.Count);
        }

        #endregion

        #region Wrap

        [Fact]
        public void WrapValidList()
        {
            // Arrange
            var ctor = new ListConstructor();
            var list = new List<int>();

            // Act
            var adapter = ctor.Wrap(list);

            // Assert
            Assert.NotNull(adapter.Collection);
        }

        [Fact]
        public void ThrowOnWrapInvalidList()
        {
            // Arrange
            var ctor = new ListConstructor();
            var notList = new object();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => ctor.Wrap(notList));
        }

        #endregion

        #region Adapter

        #region GetValue

        public static IEnumerable<object[]> Lists = new[]
        {
            new object[]{new List<int> { 1, 2, 3 }, 0, 1 },
            new object[]{new List<int> { 1, 2, 3 }, 2, 3 }
        };

        [Theory]
        [MemberData(nameof(Lists))]
        public void GetValue(List<int> list, int index, int expected)
        {
            // Arrange
            var ctor = new ListConstructor();
            var adapter = ctor.Wrap(list);

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
            var ctor = new ListConstructor();
            var adapter = ctor.Create(typeof(int), 3);

            // Act
            adapter.SetValue(index, expected);
            var value = ((List<int>)adapter.Collection)[index];

            // Assert
            Assert.Equal(expected, value);
        }

        #endregion

        #endregion
    }
}
