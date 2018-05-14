using Xunit;

namespace VoidMain.CommandLineIinterface.IO.Views.Tests
{
    public class ByWordLineViewNavigation_Should
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
            var navigation = new ByWordLineViewNavigation();
            var lineView = InitView(startValue, startPos);

            // Act
            int actualPos = navigation.FindNextPosition(lineView);

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
            var navigation = new ByWordLineViewNavigation();
            var lineView = InitView(startValue, startPos);

            // Act
            int actualPos = navigation.FindPrevPosition(lineView);

            // Assert
            Assert.Equal(expectedPos, actualPos);
        }

        #endregion

        #region Helpers

        private static InMemoryLineView InitView(string startValue, int startPos)
        {
            var lineView = new InMemoryLineView();
            lineView.Type(startValue);
            lineView.MoveTo(startPos);
            return lineView;
        }

        #endregion
    }
}
