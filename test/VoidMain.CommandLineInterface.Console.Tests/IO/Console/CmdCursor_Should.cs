using Xunit;

namespace VoidMain.CommandLineInterface.IO.Console.Tests
{
    public class CmdCursor_Should
    {
        [Theory]
        [InlineData(13, 0, 0, 0, 0, 0)]
        [InlineData(13, 1, 6, 0, 1, 6)]

        [InlineData(13, 0, 0, 1, 0, 1)]
        [InlineData(13, 0, 0, 12, 0, 12)]
        [InlineData(13, 0, 0, 13, 1, 0)]
        [InlineData(13, 0, 0, 26, 2, 0)]

        [InlineData(13, 2, 0, -1, 1, 12)]
        [InlineData(13, 2, 0, -12, 1, 1)]
        [InlineData(13, 2, 0, -13, 1, 0)]
        [InlineData(13, 2, 0, -26, 0, 0)]

        [InlineData(13, 1, 6, +7, 2, 0)]
        [InlineData(13, 1, 6, -7, 0, 12)]

        [InlineData(13, 1, 6, +13, 2, 6)]
        [InlineData(13, 1, 6, -13, 0, 6)]

        [InlineData(13, 1, 12, 1, 2, 0)]
        [InlineData(13, 1, 12, -1, 1, 11)]
        [InlineData(13, 1, 12, 13, 2, 12)]
        [InlineData(13, 1, 12, 14, 3, 0)]
        public void MoveCursor(int width, int top, int left, int offset, int actualTop, int actualLeft)
        {
            // Arrange
            var console = new CmdCursorConsole(width, top, left);
            var cursor = new CmdCursor(console);

            // Act
            cursor.Move(offset);

            // Assert
            Assert.Equal(console.CursorTop, actualTop);
            Assert.Equal(console.CursorLeft, actualLeft);
        }

        public class CmdCursorConsole : MockConsole
        {
            public CmdCursorConsole(int width, int top, int left)
            {
                BufferWidth = width;
                CursorTop = top;
                CursorLeft = left;
            }
        }
    }
}
