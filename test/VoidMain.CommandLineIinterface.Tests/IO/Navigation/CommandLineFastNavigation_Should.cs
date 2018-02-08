using VoidMain.CommandLineIinterface.IO.Views;
using Xunit;

namespace VoidMain.CommandLineIinterface.IO.Navigation.Tests
{
    public class CommandLineFastNavigation_Should
    {
        #region FindNext tests

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
            var lineView = InitView(startValue, startPos);

            // Act
            int actualPos = navigation.FindNext(lineView);

            // Assert
            Assert.Equal(expectedPos, actualPos);
        }

        #endregion

        #region FindPrev tests

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
            var lineView = InitView(startValue, startPos);

            // Act
            int actualPos = navigation.FindPrev(lineView);

            // Assert
            Assert.Equal(expectedPos, actualPos);
        }

        #endregion

        #region Helpers

        private static CommandLineHiddenView InitView(string startValue, int startPos)
        {
            var lineView = new CommandLineHiddenView();
            lineView.Type(startValue);
            lineView.MoveTo(startPos);
            return lineView;
        }

        #endregion
    }
}
