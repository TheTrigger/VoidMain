using Moq;
using System;
using Xunit;

namespace VoidMain.CommandLineIinterface.History.Tests
{
    public class CommandsHistoryManager_Should
    {
        #region Ctor tests

        [Fact]
        public void RequireStorage()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandsHistoryManager(null));
        }

        [Theory]
        [InlineData(-1)]
        public void HaveValidSavePeriod(int savePeriod)
        {
            var mock = new Mock<ICommandsHistoryStorage>();
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommandsHistoryManager(mock.Object,
                new CommandsHistoryOptions { SavePeriod = TimeSpan.FromSeconds(savePeriod) }));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void HaveValidMaxCount(int maxCount)
        {
            var storage = new Mock<ICommandsHistoryStorage>();
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommandsHistoryManager(storage.Object,
                new CommandsHistoryOptions { MaxCount = maxCount }));
        }

        #endregion

        #region EnsureCommandsLoaded tests

        [Theory]
        [InlineData("prev")]
        [InlineData("next")]
        [InlineData("add")]
        public void LoadCommandsBeforeUse(string command)
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(storage.Object);

            // Act
            switch (command)
            {
                case "prev":
                    manager.TryGetPrevCommand(out string prev);
                    break;
                case "next":
                    manager.TryGetNextCommand(out string next);
                    break;
                case "add":
                    manager.AddCommand("command");
                    break;
            }

            // Assert
            storage.Verify(s => s.Load(), Times.Once());
        }

        #endregion

        #region TryGetPrevCommand

        [Theory]
        [InlineData(1, "c")]
        [InlineData(2, "b")]
        public void GetPrevCommand(int times, string expected)
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(storage.Object);

            // Act
            string command = null;
            for (int i = 0; i < times; i++)
            {
                manager.TryGetPrevCommand(out command);
            }

            // Assert
            Assert.Equal(expected, command);
        }

        [Fact]
        public void StopAtFirstCommand()
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(storage.Object);

            // Act
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                if (!manager.TryGetPrevCommand(out string _))
                {
                    break;
                }
                count++;
            }

            // Assert
            Assert.Equal(3, count);
        }

        #endregion

        #region TryGetNextCommand

        [Theory]
        [InlineData(1, "b")]
        [InlineData(2, "c")]
        public void GetNextCommand(int times, string expected)
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(storage.Object);
            while (manager.TryGetPrevCommand(out string _)) { }

            // Act
            string command = null;
            for (int i = 0; i < times; i++)
            {
                manager.TryGetNextCommand(out command);
            }

            // Assert
            Assert.Equal(expected, command);
        }

        [Fact]
        public void StopAfterLastCommand()
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(storage.Object);
            while (manager.TryGetPrevCommand(out string _)) { }

            // Act
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                if (!manager.TryGetNextCommand(out string _))
                {
                    break;
                }
                count++;
            }

            // Assert
            Assert.Equal(2, count);
        }

        #endregion

        #region AddCommand tests

        [Fact]
        public void AddNewCommand()
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(storage.Object);
            var expected = "command";

            // Act
            manager.AddCommand(expected);
            manager.TryGetPrevCommand(out string command);

            // Assert
            Assert.Equal(expected, command);
        }

        [Fact]
        public void NotExceedMaxCommandsCount()
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(storage.Object);

            // Act
            for (int i = 0; i < manager.MaxCount + 1; i++)
            {
                manager.AddCommand("command_" + i);
            }

            // Assert
            Assert.Equal(manager.MaxCount, manager.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void NotAddEmptyCommand(string command)
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(storage.Object);

            // Act
            manager.AddCommand(command);

            // Assert
            Assert.Equal(0, manager.Count);
        }

        [Fact]
        public void NotAddSameLastCommand()
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(storage.Object);

            // Act
            manager.AddCommand("command");
            manager.AddCommand("command");

            // Assert
            Assert.Equal(1, manager.Count);
        }

        #endregion

        #region Clear tests

        [Fact]
        public void ClearElements()
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(storage.Object);

            // Act
            manager.TryGetPrevCommand(out string _); // Force to load from storage
            manager.Clear();

            // Assert
            Assert.Equal(0, manager.Count);
        }

        [Fact]
        public void ClearElementsWithoutLoadingFromStorage()
        {
            // Arrange
            var storage = new Mock<ICommandsHistoryStorage>();
            storage.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(storage.Object);

            // Act
            manager.Clear();

            // Assert
            Assert.Equal(0, manager.Count);
            storage.Verify(s => s.Load(), Times.Never());
        }

        #endregion
    }
}
