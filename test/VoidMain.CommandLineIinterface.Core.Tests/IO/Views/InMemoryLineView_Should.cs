using System;
using Xunit;

namespace VoidMain.CommandLineIinterface.IO.Views.Tests
{
    public class InMemoryLineView_Should
    {
        #region Move & MoveTo tests

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void NotMoveOutsideBoundaries(int offset)
        {
            // Arrange
            var line = new InMemoryLineView();

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => line.Move(offset));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void NotMoveToOutsideBoundaries(int newPos)
        {
            // Arrange
            var line = new InMemoryLineView();

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => line.MoveTo(newPos));
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(1, 1)]
        public void Move(int startPos, int offset)
        {
            // Arrange
            var line = new InMemoryLineView();
            line.Type("test");
            line.MoveTo(startPos);

            // Act
            line.Move(offset);

            // Assert
            Assert.Equal(startPos + offset, line.Position);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 2)]
        public void MoveTo(int startPos, int newPos)
        {
            // Arrange
            var line = new InMemoryLineView();
            line.Type("test");
            line.MoveTo(startPos);

            // Act
            line.MoveTo(newPos);

            // Assert
            Assert.Equal(newPos, line.Position);
        }

        #endregion

        #region Insert tests

        [Theory]
        [InlineData("", 0, 'a', "a", 1)]
        [InlineData("test", 0, 'a', "atest", 1)]
        [InlineData("test", 4, 'a', "testa", 5)]
        [InlineData("test", 2, 'a', "teast", 3)]
        public void TypeCharValue(string startValue, int startPos,
            char value, string expextedValue, int expextedPos)
        {
            // Arrange
            var line = new InMemoryLineView();
            line.Type(startValue);
            line.MoveTo(startPos);

            // Act
            line.Type(value);

            // Assert
            Assert.Equal(expextedValue, line.ToString());
            Assert.Equal(expextedPos, line.Position);
        }

        [Theory]
        [InlineData("", 0, "ab", "ab", 2)]
        [InlineData("test", 0, "ab", "abtest", 2)]
        [InlineData("test", 4, "ab", "testab", 6)]
        [InlineData("test", 2, "ab", "teabst", 4)]
        public void TypeStringValue(string startValue, int startPos,
            string value, string expextedValue, int expextedPos)
        {
            // Arrange
            var line = new InMemoryLineView();
            line.Type(startValue);
            line.MoveTo(startPos);

            // Act
            line.Type(value);

            // Assert
            Assert.Equal(expextedValue, line.ToString());
            Assert.Equal(expextedPos, line.Position);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NotTypeEmptyStringValue(string value)
        {
            // Arrange
            var line = new InMemoryLineView();

            // Act
            line.Type(value);

            // Assert
            Assert.Equal("", line.ToString());
        }

        #endregion

        #region Delete & Clear tests

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void NotDeleteOutsideOfBoundaries(int count)
        {
            // Arrange
            var line = new InMemoryLineView();

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => line.Delete(count));
        }

        [Theory]
        [InlineData("test", 4, -1, "tes", 3)]
        [InlineData("test", 4, -2, "te", 2)]
        [InlineData("test", 0, 1, "est", 0)]
        [InlineData("test", 0, 2, "st", 0)]
        [InlineData("test", 2, -1, "tst", 1)]
        [InlineData("test", 2, 1, "tet", 2)]
        public void Delete(string startValue, int startPos,
            int deleteCount, string expextedValue, int expextedPos)
        {
            // Arrange
            var line = new InMemoryLineView();
            line.Type(startValue);
            line.MoveTo(startPos);

            // Act
            line.Delete(deleteCount);

            // Assert
            Assert.Equal(expextedValue, line.ToString());
            Assert.Equal(expextedPos, line.Position);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test")]
        public void Clear(string startValue)
        {
            // Arrange
            var line = new InMemoryLineView();
            line.Type(startValue);

            // Act
            line.Clear();

            // Assert
            Assert.Equal("", line.ToString());
            Assert.Equal(0, line.Position);
        }

        #endregion

        #region ToString tests

        [Fact]
        public void ReturnValidString()
        {
            // Arrange
            var line = new InMemoryLineView();
            string expected = "test";
            line.Type(expected);

            // Act
            string actual = line.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", 0, "test")]
        [InlineData("test", 2, "st")]
        public void ReturnValidStringStartedAt(
            string startValue, int start, string expected)
        {
            // Arrange
            var line = new InMemoryLineView();
            line.Type(startValue);

            // Act
            string actual = line.ToString(start);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", 0, 4, "test")]
        [InlineData("test", 2, 2, "st")]
        public void ReturnValidStringStartedAtWithLength(
            string startValue, int start, int length, string expected)
        {
            // Arrange
            var line = new InMemoryLineView();
            line.Type(startValue);

            // Act
            string actual = line.ToString(start, length);

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
