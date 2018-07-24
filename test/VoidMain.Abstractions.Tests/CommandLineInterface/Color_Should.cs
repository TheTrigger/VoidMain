using System;
using System.Globalization;
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

        [Theory]
        [InlineData("00000000")]
        [InlineData("0000000A")]
        [InlineData("000000A0")]
        [InlineData("00000A00")]
        [InlineData("0000A000")]
        [InlineData("000A0000")]
        [InlineData("00A00000")]
        [InlineData("0A000000")]
        [InlineData("A0000000")]
        [InlineData("059afADF")]
        public void ParseHexString(string hexString)
        {
            // Arrange, Act
            var color = new Color(hexString);
            uint expected = UInt32.Parse(hexString, NumberStyles.HexNumber);

            // Assert
            Assert.Equal(expected, color.Value);
        }

        [Fact]
        public void ThrowOnNullHexString()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => new Color(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("AB")]
        [InlineData("XYZ")]
        [InlineData("####")]
        [InlineData("12345")]
        [InlineData("1234567")]
        [InlineData("123456789")]
        public void ThrowOnInvalidHexString(string hexString)
        {
            // Arrange, Act, Assert
            Assert.Throws<FormatException>(() => new Color(hexString));
        }

        #endregion

        [Theory]
        [InlineData("012", "FF001122")]
        [InlineData("3456", "33445566")]
        [InlineData("789ABC", "FF789ABC")]
        [InlineData("DEF01234", "DEF01234")]
        [InlineData("#012", "FF001122")]
        [InlineData("#3456", "33445566")]
        [InlineData("#789ABC", "FF789ABC")]
        [InlineData("#DEF01234", "DEF01234")]
        public void ToHexString(string hexString, string expected)
        {
            // Arrange, Act
            var color = new Color(hexString);

            // Assert
            Assert.Equal(expected, color.ToHexString(), ignoreCase: true);
        }

        #region Equals tests

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
