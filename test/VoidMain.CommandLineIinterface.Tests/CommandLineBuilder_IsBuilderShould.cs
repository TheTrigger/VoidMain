using System;
using Xunit;

namespace VoidMain.CommandLineIinterface.Tests
{
    public class CommandLineBuilder_IsBuilderShould
    {
        [Fact]
        public void HaveValidCapacity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommandLineBuilder(-1));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void NotMoveOutsideBoundaries(int offset)
        {
            // Arrange
            var builder = new CommandLineBuilder();

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.Move(offset));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void NotMoveToOutsideBoundaries(int newPos)
        {
            // Arrange
            var builder = new CommandLineBuilder();

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.MoveTo(newPos));
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(1, 1)]
        public void Move(int startPos, int offset)
        {
            // Arrange
            var builder = new CommandLineBuilder();
            builder.Insert("test");
            builder.MoveTo(startPos);

            // Act
            builder.Move(offset);

            // Assert
            Assert.Equal(startPos + offset, builder.Position);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 2)]
        public void MoveTo(int startPos, int newPos)
        {
            // Arrange
            var builder = new CommandLineBuilder();
            builder.Insert("test");
            builder.MoveTo(startPos);

            // Act
            builder.MoveTo(newPos);

            // Assert
            Assert.Equal(newPos, builder.Position);
        }

        [Theory]
        [InlineData("", 0, 'a', "a", 1)]
        [InlineData("test", 0, 'a', "atest", 1)]
        [InlineData("test", 4, 'a', "testa", 5)]
        [InlineData("test", 2, 'a', "teast", 3)]
        public void InsertCharValue(string startValue, int startPos,
            char value, string expextedValue, int expextedPos)
        {
            // Arrange
            var builder = new CommandLineBuilder();
            builder.Insert(startValue);
            builder.MoveTo(startPos);

            // Act
            builder.Insert(value);

            // Assert
            Assert.Equal(expextedValue, builder.ToString());
            Assert.Equal(expextedPos, builder.Position);
        }

        [Theory]
        [InlineData("", 0, "ab", "ab", 2)]
        [InlineData("test", 0, "ab", "abtest", 2)]
        [InlineData("test", 4, "ab", "testab", 6)]
        [InlineData("test", 2, "ab", "teabst", 4)]
        public void InsertStringValue(string startValue, int startPos,
            string value, string expextedValue, int expextedPos)
        {
            // Arrange
            var builder = new CommandLineBuilder();
            builder.Insert(startValue);
            builder.MoveTo(startPos);

            // Act
            builder.Insert(value);

            // Assert
            Assert.Equal(expextedValue, builder.ToString());
            Assert.Equal(expextedPos, builder.Position);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NotInsertEmptyStringValue(string value)
        {
            // Arrange
            var builder = new CommandLineBuilder();

            // Act
            builder.Insert(value);

            // Assert
            Assert.Equal("", builder.ToString());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void NotDeleteOutsideBoundaries(int count)
        {
            // Arrange
            var builder = new CommandLineBuilder();

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.Delete(count));
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
            var builder = new CommandLineBuilder();
            builder.Insert(startValue);
            builder.MoveTo(startPos);

            // Act
            builder.Delete(deleteCount);

            // Assert
            Assert.Equal(expextedValue, builder.ToString());
            Assert.Equal(expextedPos, builder.Position);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test")]
        public void Clear(string startValue)
        {
            // Arrange
            var builder = new CommandLineBuilder();
            builder.Insert(startValue);

            // Act
            builder.Clear();

            // Assert
            Assert.Equal("", builder.ToString());
            Assert.Equal(0, builder.Position);
        }

        [Theory]
        [InlineData("test", 0, "test")]
        [InlineData("test", 2, "st")]
        public void ReturnValidStringStartedAt(
            string startValue, int start, string expected)
        {
            // Arrange
            var builder = new CommandLineBuilder();
            builder.Insert(startValue);

            // Act
            string actual = builder.ToString(start);

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
            var builder = new CommandLineBuilder();
            builder.Insert(startValue);

            // Act
            string actual = builder.ToString(start, length);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
