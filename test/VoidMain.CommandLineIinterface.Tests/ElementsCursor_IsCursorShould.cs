using System;
using VoidMain.CommandLineIinterface.Parser;
using Xunit;

namespace VoidMain.CommandLineIinterface.Tests
{
    public class ElementsCursor_IsCursorShould
    {
        [Fact]
        public void RequireElementsList()
        {
            Assert.Throws<ArgumentNullException>(() => new ElementsCursor<int>(null, 0));
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2 })]
        public void BeAtTheEnd(int[] elements)
        {
            // Arrange
            var cursor = new ElementsCursor<int>(elements, 0);

            // Assert
            for (int i = 0; i < elements.Length; i++)
            {
                Assert.True(!cursor.IsAtTheEnd());
                cursor.MoveNext();
            }

            Assert.True(cursor.IsAtTheEnd());
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 0, 0)]
        [InlineData(new int[] { 1, 2, 3 }, 1, 1)]
        [InlineData(new int[] { 1, 2, 3 }, 2, 2)]
        public void MoveNext(int[] elements, int delta, int expected)
        {
            // Arrange
            var cursor = new ElementsCursor<int>(elements, 0);

            // Act
            cursor.MoveNext(delta);

            // Assert
            Assert.Equal(expected, cursor.Position);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 0, 1)]
        [InlineData(new int[] { 1, 2, 3 }, 1, 2)]
        [InlineData(new int[] { 1, 2, 3 }, 2, 3)]
        [InlineData(new int[] { 1, 2, 3 }, 3, 0)]
        public void PeekOrGetTerminal(int[] elements, int delta, int expected)
        {
            // Arrange
            var cursor = new ElementsCursor<int>(elements, 0);

            // Act
            int actual = cursor.Peek(delta);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
