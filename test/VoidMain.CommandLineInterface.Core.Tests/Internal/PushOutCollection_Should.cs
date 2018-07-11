using System;
using Xunit;

namespace VoidMain.CommandLineInterface.Internal.Tests
{
    public class PushOutCollection_Should
    {
        #region Ctor tests

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void HaveValidCapacity(int capacity)
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new PushOutCollection<int>(capacity));
        }

        [Fact]
        public void NotAcceptNullCollection()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PushOutCollection<int>(null));
        }

        [Fact]
        public void NotAcceptEmptyCollection()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new PushOutCollection<int>(Array.Empty<int>()));
        }

        [Theory]
        [InlineData(new[] { 0 })]
        [InlineData(new[] { 0, 1, 2 })]
        public void HaveCapacityOfPassedCollection(int[] elements)
        {
            // Act
            var collection = new PushOutCollection<int>(elements);

            // Assert
            Assert.Equal(elements.Length, collection.MaxCount);
        }

        #endregion

        #region Add tests

        [Theory]
        [InlineData(3, 1)]
        [InlineData(3, 3)]
        public void IncreaseCount(int capacity, int newElements)
        {
            // Arrange
            var collection = new PushOutCollection<int>(capacity);

            // Act
            for (int i = 0; i < newElements; i++)
            {
                collection.Add(i);
            }

            // Assert
            Assert.Equal(newElements, collection.Count);
        }

        [Theory]
        [InlineData(3, 4)]
        [InlineData(3, 7)]
        public void NotExceedCapacity(int capacity, int newElements)
        {
            // Arrange
            var collection = new PushOutCollection<int>(capacity);

            // Act
            for (int i = 0; i < newElements; i++)
            {
                collection.Add(i);
            }

            // Assert
            Assert.Equal(capacity, collection.Count);
            Assert.Equal(capacity, collection.MaxCount);
        }

        [Theory]
        [InlineData(3, 1, new[] { 1, 2, 3 })]
        [InlineData(3, 5, new[] { 5, 6, 7 })]
        public void PushOutFirstElement(int capacity, int extraElements, int[] expected)
        {
            // Arrange
            var collection = new PushOutCollection<int>(capacity);

            // Act
            for (int i = 0; i < capacity + extraElements; i++)
            {
                collection.Add(i);
            }

            // Assert
            Assert.Equal(expected, collection);
        }

        #endregion

        #region Get tests

        [Theory]
        [InlineData(3, 0, 0, 0)]
        [InlineData(3, 0, 2, 2)]
        [InlineData(3, 1, 1, 2)]
        [InlineData(3, 5, 0, 5)]
        public void GetWithZeroBasedIndexing(int capacity, int extraElements, int index, int expected)
        {
            // Arrange
            var collection = new PushOutCollection<int>(capacity);
            PopulateCollection(collection, capacity + extraElements);

            // Act
            int actual = collection[index];

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Set tests

        [Theory]
        [InlineData(3, 0, new[] { -1, 1, 2 })]
        [InlineData(3, 1, new[] { -1, 2, 3 })]
        [InlineData(3, 5, new[] { -1, 6, 7 })]
        public void SetWithZeroBasedIndexing(int capacity, int extraElements, int[] expected)
        {
            // Arrange
            var collection = new PushOutCollection<int>(capacity);
            PopulateCollection(collection, capacity + extraElements);

            // Act
            collection[0] = -1;

            // Assert
            Assert.Equal(expected, collection);
        }

        #endregion

        #region Clear tests

        [Fact]
        public void ClearElements()
        {
            // Arrange
            var collection = new PushOutCollection<int>(new int[] { 0, 1, 2 });

            // Act
            collection.Clear();

            // Assert
            Assert.Empty(collection);
        }

        #endregion

        #region TrimTo tests

        [Theory]
        [InlineData(5, 0, 0, new int[0])]
        [InlineData(5, 2, 5, new[] { 2, 3, 4, 5, 6 })]
        [InlineData(5, 2, 2, new[] { 2, 3 })]
        [InlineData(5, 3, 4, new[] { 3, 4, 5, 6 })]
        public void TrimToCount(int capacity, int extraElements, int newCount, int[] expected)
        {
            // Arrange
            var collection = new PushOutCollection<int>(capacity);
            PopulateCollection(collection, capacity + extraElements);

            // Act
            collection.TrimTo(newCount);

            // Assert
            Assert.Equal(expected, collection);
        }

        #endregion

        #region Helpers

        private static void PopulateCollection(PushOutCollection<int> collection, int count)
        {
            for (int i = 0; i < count; i++)
            {
                collection.Add(i);
            }
        }

        #endregion
    }
}
