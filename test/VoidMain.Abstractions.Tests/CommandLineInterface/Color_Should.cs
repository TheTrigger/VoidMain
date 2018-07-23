using Xunit;

namespace VoidMain.CommandLineInterface.Tests
{
    public class Color_Should
    {
        #region Ctor tests

        [Fact]
        public void SetDefaultAlpha()
        {
            // Arrange, Act
            var color = new Color(0, 0, 0);

            // Assert
            Assert.Equal(255, color.A);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(128)]
        [InlineData(255)]
        public void SetAlpha(byte alpha)
        {
            // Arrange, Act
            var color = new Color(alpha, 0, 0, 0);

            // Assert
            Assert.Equal(alpha, color.A);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(128)]
        [InlineData(255)]
        public void SetRed(byte red)
        {
            // Arrange, Act
            var color = new Color(red, 0, 0);

            // Assert
            Assert.Equal(red, color.R);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(128)]
        [InlineData(255)]
        public void SetGreen(byte green)
        {
            // Arrange, Act
            var color = new Color(0, green, 0);

            // Assert
            Assert.Equal(green, color.G);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(128)]
        [InlineData(255)]
        public void SetBlue(byte blue)
        {
            // Arrange, Act
            var color = new Color(0, 0, blue);

            // Assert
            Assert.Equal(blue, color.B);
        }

        #endregion

        #region Equals

        [Theory]
        [InlineData(255, 192, 128, 64)]
        [InlineData(0, 0, 0, 0)]
        public void EqualsToItself(byte alpha, byte red, byte green, byte blue)
        {
            // Arrange
            var color = new Color(alpha, red, green, blue);

            // Act, Assert
            Assert.Equal(color, color);
        }

        [Theory]
        [InlineData(255, 192, 128, 64)]
        [InlineData(0, 0, 0, 0)]
        public void EqualsToTheSame(byte alpha, byte red, byte green, byte blue)
        {
            // Arrange
            var a = new Color(alpha, red, green, blue);
            var b = new Color(alpha, red, green, blue);

            // Act, Assert
            Assert.Equal(a, b);
        }

        [Fact]
        public void NotEqualsToNull()
        {
            // Arrange
            var color = new Color(0, 0, 0, 0);
            Color nullColor = null;

            // Act, Assert
            Assert.NotEqual(nullColor, color);
        }

        [Theory]
        [InlineData(128, 0, 0, 0)]
        [InlineData(0, 128, 0, 0)]
        [InlineData(0, 0, 128, 0)]
        [InlineData(0, 0, 0, 128)]
        public void NotEqualsToDifferent(byte alpha, byte red, byte green, byte blue)
        {
            // Arrange
            var a = new Color(255, 255, 255, 255);
            var b = new Color(alpha, red, green, blue);

            // Act, Assert
            Assert.NotEqual(a, b);
        }

        #endregion
    }
}
