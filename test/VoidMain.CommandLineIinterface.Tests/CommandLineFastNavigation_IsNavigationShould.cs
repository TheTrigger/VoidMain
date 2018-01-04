using VoidMain.CommandLineIinterface.Console.Views;
using Xunit;

namespace VoidMain.CommandLineIinterface.Tests
{
    public class CommandLineFastNavigation_IsNavigationShould
    {
        [Theory]
        [InlineData("", 0, 0)]
        [InlineData("test", 0, 4)]
        [InlineData("test", 4, 4)]
        [InlineData("    ", 0, 4)]
        [InlineData("    ", 4, 4)]
        [InlineData("test    ", 0, 4)]
        [InlineData("test    ", 4, 8)]
        [InlineData("    test", 0, 4)]
        [InlineData("    test", 4, 8)]
        public void FindNext(string startValue, int startPos, int expectedPos)
        {
            // Arrange
            var navigation = new CommandLineFastNavigation();
            var lineView = new ConsoleCommandLineHiddenView();
            lineView.Type(startValue);
            lineView.MoveTo(startPos);

            // Act
            int actualPos = navigation.FindNext(lineView);

            // Assert
            Assert.Equal(expectedPos, actualPos);
        }

        [Theory]
        [InlineData("", 0, 0)]
        [InlineData("test", 4, 0)]
        [InlineData("test", 0, 0)]
        [InlineData("    ", 4, 0)]
        [InlineData("    ", 0, 0)]
        [InlineData("test    ", 8, 4)]
        [InlineData("test    ", 4, 0)]
        [InlineData("    test", 8, 4)]
        [InlineData("    test", 4, 0)]
        public void FindPrev(string startValue, int startPos, int expectedPos)
        {
            // Arrange
            var navigation = new CommandLineFastNavigation();
            var lineView = new ConsoleCommandLineHiddenView();
            lineView.Type(startValue);
            lineView.MoveTo(startPos);

            // Act
            int actualPos = navigation.FindPrev(lineView);

            // Assert
            Assert.Equal(expectedPos, actualPos);
        }
    }
}
