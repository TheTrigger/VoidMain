using Moq;
using System;
using VoidMain.CommandLineIinterface.History;
using Xunit;

namespace VoidMain.CommandLineIinterface.Tests
{
    public class CommandsHistoryManager_IsManagerShould
    {
        [Fact]
        public void RequireStorage()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandsHistoryManager(null));
        }

        [Theory]
        [InlineData("prev")]
        [InlineData("next")]
        [InlineData("add")]
        public void LoadCommandsBeforeUse(string command)
        {
            // Arrange
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(mock.Object);

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
            mock.Verify(s => s.Load(), Times.Once());
        }

        [Theory]
        [InlineData(1, "c")]
        [InlineData(2, "b")]
        public void GetPrevCommand(int times, string expected)
        {
            // Arrange
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(mock.Object);

            // Act
            string command = null;
            for (int i = 0; i < times; i++)
            {
                manager.TryGetPrevCommand(out command);
            }

            // Assert
            Assert.Equal(expected, command);
        }

        [Theory]
        [InlineData(1, "b")]
        [InlineData(2, "c")]
        public void GetNextCommand(int times, string expected)
        {
            // Arrange
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(mock.Object);
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
        public void StopAtFirstCommand()
        {
            // Arrange
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(mock.Object);

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

        [Fact]
        public void StopAfterLastCommand()
        {
            // Arrange
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(mock.Object);
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

        [Fact]
        public void AddNewCommand()
        {
            // Arrange
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(mock.Object);
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
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(mock.Object);

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
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(mock.Object);

            // Act
            manager.AddCommand(command);

            // Assert
            Assert.Equal(0, manager.Count);
        }

        [Fact]
        public void NotAddSameLastCommand()
        {
            // Arrange
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(mock.Object);

            // Act
            manager.AddCommand("command");
            manager.AddCommand("command");

            // Assert
            Assert.Equal(1, manager.Count);
        }

        [Fact]
        public void ClearElements()
        {
            // Arrange
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(new[] { "a", "b", "c" });
            var manager = new CommandsHistoryManager(mock.Object);

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
            var mock = new Mock<ICommandsHistoryStorage>();
            mock.Setup(s => s.Load()).Returns(Array.Empty<string>());
            var manager = new CommandsHistoryManager(mock.Object);

            // Act
            manager.Clear();

            // Assert
            Assert.Equal(0, manager.Count);
            mock.Verify(s => s.Load(), Times.Never());
        }
    }
}
