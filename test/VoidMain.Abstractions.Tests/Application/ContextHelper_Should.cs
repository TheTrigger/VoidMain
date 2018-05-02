using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace VoidMain.Application.Tests
{
    public class ContextHelper_Should
    {
        #region CommandLine

        [Fact]
        public void SetAndGetCommandLine()
        {
            // Arrange
            var context = new Dictionary<string, object>();
            var expected = "";

            // Act
            ContextHelper.SetCommandLine(context, expected);
            bool hasValue = ContextHelper.TryGetCommandLine(context, out var actual);

            // Assert
            Assert.True(hasValue);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotGetMissingCommandLine()
        {
            // Arrange
            var context = new Dictionary<string, object>();

            // Act
            bool hasValue = ContextHelper.TryGetCommandLine(context, out var actual);

            // Assert
            Assert.False(hasValue);
        }

        #endregion

        #region CommandName

        [Fact]
        public void SetAndGetCommandName()
        {
            // Arrange
            var context = new Dictionary<string, object>();
            var expected = ContextHelper.EmptyCommandName;

            // Act
            ContextHelper.SetCommandName(context, expected);
            bool hasValue = ContextHelper.TryGetCommandName(context, out var actual);

            // Assert
            Assert.True(hasValue);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotGetMissingCommandName()
        {
            // Arrange
            var context = new Dictionary<string, object>();

            // Act
            bool hasValue = ContextHelper.TryGetCommandName(context, out var actual);

            // Assert
            Assert.False(hasValue);
        }

        #endregion

        #region Options

        [Fact]
        public void SetAndGetOptions()
        {
            // Arrange
            var context = new Dictionary<string, object>();
            var expected = ContextHelper.EmptyOptions;

            // Act
            ContextHelper.SetOptions(context, expected);
            bool hasValue = ContextHelper.TryGetOptions(context, out var actual);

            // Assert
            Assert.True(hasValue);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotGetMissingOptions()
        {
            // Arrange
            var context = new Dictionary<string, object>();

            // Act
            bool hasValue = ContextHelper.TryGetOptions(context, out var actual);

            // Assert
            Assert.False(hasValue);
        }

        #endregion

        #region Operands

        [Fact]
        public void SetAndGetOperands()
        {
            // Arrange
            var context = new Dictionary<string, object>();
            var expected = ContextHelper.EmptyOperands;

            // Act
            ContextHelper.SetOperands(context, expected);
            bool hasValue = ContextHelper.TryGetOperands(context, out var actual);

            // Assert
            Assert.True(hasValue);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotGetMissingOperands()
        {
            // Arrange
            var context = new Dictionary<string, object>();

            // Act
            bool hasValue = ContextHelper.TryGetOperands(context, out var actual);

            // Assert
            Assert.False(hasValue);
        }

        #endregion

        #region CancelToken

        [Fact]
        public void SetAndGetCancelToken()
        {
            // Arrange
            var context = new Dictionary<string, object>();
            var expected = CancellationToken.None;

            // Act
            ContextHelper.SetCancelToken(context, expected);
            bool hasValue = ContextHelper.TryGetCancelToken(context, out var actual);

            // Assert
            Assert.True(hasValue);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotGetMissingCancelToken()
        {
            // Arrange
            var context = new Dictionary<string, object>();

            // Act
            bool hasValue = ContextHelper.TryGetCancelToken(context, out var actual);

            // Assert
            Assert.False(hasValue);
        }

        #endregion
    }
}
