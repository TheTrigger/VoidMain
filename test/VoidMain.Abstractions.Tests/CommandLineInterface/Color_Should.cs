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
        [InlineData("00000000", UInt32.MinValue)]
        [InlineData("0000000F", 15)]
        [InlineData("000000F0", 240)]
        [InlineData("00000F00", 3_840)]
        [InlineData("0000F000", 61_440)]
        [InlineData("000F0000", 983_040)]
        [InlineData("00F00000", 15_728_640)]
        [InlineData("0F000000", 251_658_240)]
        [InlineData("F0000000", 4_026_531_840)]
        [InlineData("FFFFFFFF", UInt32.MaxValue)]
        [InlineData("01234567", 19_088_743)]
        [InlineData("89abcdef", 2_309_737_967)]
        [InlineData("89ABCDEF", 2_309_737_967)]
        [InlineData("#01234567", 19_088_743)]
        [InlineData("012", 4_278_194_466)]
        [InlineData("0123", 1_122_867)]
        [InlineData("012345", 4_278_264_645)]
        public void Parse(string value, uint expected)
        {
            // Arrange, Act
            var color = Color.Parse(value);

            // Assert
            Assert.Equal(expected, color.Value);
        }

        [Fact]
        public void ThrowOnNullHexString()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => Color.Parse(null));
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
            Assert.Throws<FormatException>(() => Color.Parse(hexString));
        }

        #endregion

        [Theory]
        [InlineData("012", "#FF001122")]
        [InlineData("3456", "#33445566")]
        [InlineData("789ABC", "#FF789ABC")]
        [InlineData("DEF01234", "#DEF01234")]
        [InlineData("#012", "#FF001122")]
        [InlineData("#3456", "#33445566")]
        [InlineData("#789ABC", "#FF789ABC")]
        [InlineData("#DEF01234", "#DEF01234")]
        public void ToHexString(string hexString, string expected)
        {
            // Arrange
            var color = Color.Parse(hexString);

            // Act
            string actual = color.ToString("argb-hex");

            // Assert
            Assert.Equal(expected, actual, ignoreCase: true);
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
