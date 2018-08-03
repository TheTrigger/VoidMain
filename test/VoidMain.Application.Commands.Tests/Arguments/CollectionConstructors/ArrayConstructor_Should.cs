using System;
using System.Collections.Generic;
using Xunit;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors.Tests
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
        [InlineData(typeof(IComparable<int>))]
        public void ThrowOnGetElementTypeOfInvalidCollectionType(Type collectionType)
        {
            // Arrange
            var ctor = new ArrayConstructor();

            // Act, Assert
            Assert.Throws<NotSupportedException>(() => ctor.GetElementType(collectionType));
        }

        #endregion
    }
}
