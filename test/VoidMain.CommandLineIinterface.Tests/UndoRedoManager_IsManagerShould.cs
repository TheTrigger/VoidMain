using System;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.UndoRedo;
using Xunit;

namespace VoidMain.CommandLineIinterface.Tests
{
    public class UndoRedoManager_IsManagerShould
    {
        private static CommandLineViewSnapshot S(string value)
        {
            return new CommandLineViewSnapshot(value, 0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void HaveValidMaxCount(int maxCount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new UndoRedoManager(new UndoRedoOptions { MaxCount = maxCount}));
        }

        [Fact]
        public void RequireValidSnapshotsOnUndo()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var currentSnapshot = S(null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => manager.TryUndo(currentSnapshot, out var _));
        }

        [Fact]
        public void RequireValidSnapshotsOnRedo()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var currentSnapshot = S(null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => manager.TryUndo(currentSnapshot, out var _));
        }

        [Fact]
        public void RequireValidSnapshotsOnAdd()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var snapshot = S(null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => manager.TryAddSnapshot(snapshot));
        }

        [Fact]
        public void NotUndoWithoutSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var currentSnapshot = S("");

            for (int i = 0; i < 2; i++)
            {
                // Act
                bool done = manager.TryUndo(currentSnapshot, out var prevSnaoshot);

                // Assert
                Assert.False(done);
            }
        }

        [Fact]
        public void NotRedoWithoutSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var currentSnapshot = S("");

            for (int i = 0; i < 2; i++)
            {
                // Act
                bool done = manager.TryRedo(currentSnapshot, out var nextSnaoshot);

                // Assert
                Assert.False(done);
            }
        }

        [Fact]
        public void NotUndoMoreThanItHaveSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("snapshot_1");
            var secondSnapshot = S("snapshot_2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            var currentSnapshot = secondSnapshot;
            var prevSnapshot = S(null);

            // Act
            bool done = manager.TryUndo(currentSnapshot, out prevSnapshot);
            currentSnapshot = prevSnapshot;
            done = manager.TryUndo(currentSnapshot, out prevSnapshot);

            // Assert
            Assert.False(done);
        }

        [Fact]
        public void NotRedoMoreThanItHaveSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("snapshot_1");
            var secondSnapshot = S("snapshot_2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            var currentSnapshot = secondSnapshot;
            var nextSnapshot = S(null);
            manager.TryUndo(currentSnapshot, out var prevSnaoshot);
            currentSnapshot = prevSnaoshot;

            // Act
            bool done = manager.TryRedo(currentSnapshot, out nextSnapshot);
            currentSnapshot = nextSnapshot;
            done = manager.TryRedo(currentSnapshot, out nextSnapshot);

            // Assert
            Assert.False(done);
        }

        [Fact]
        public void AddNewSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var expected = S("snapshot_1");
            var undoneSnapshot = S("snapshot_2");
            manager.TryAddSnapshot(expected);

            // Act
            manager.TryAddSnapshot(undoneSnapshot);
            manager.TryUndo(undoneSnapshot, out var prevSnapshot);

            // Assert
            Assert.Equal(expected, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void NotExceedMaxSnapshotsCount()
        {
            // Arrange
            var manager = new UndoRedoManager();

            // Act
            for (int i = 0; i < manager.MaxCount + 1; i++)
            {
                manager.TryAddSnapshot(S("snapshot_" + i));
            }

            // Assert
            Assert.Equal(manager.MaxCount, manager.Count);
        }

        [Fact]
        public void DeleteAfterCurrent()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("snapshot_1");
            var undoneSnapshot = S("snapshot_2");
            var newSnapshot = S("snapshot_3");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(undoneSnapshot);
            manager.TryUndo(undoneSnapshot, out var _);

            // Act
            manager.TryAddSnapshot(newSnapshot, deleteAfter: true);
            manager.TryUndo(newSnapshot, out var prevSnapshot);

            // Assert
            Assert.Equal(2, manager.Count);
            Assert.Equal(firstSnapshot, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void NotDeleteAfterCurrentIfCurrentIsLast()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("snapshot_1");
            var secondSnapshot = S("snapshot_2");
            var newSnapshot = S("snapshot_3");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);

            // Act
            manager.TryAddSnapshot(newSnapshot, deleteAfter: true);
            manager.TryUndo(newSnapshot, out var prevSnapshot);

            // Assert
            Assert.Equal(3, manager.Count);
            Assert.Equal(secondSnapshot, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void NotAddSameLastSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var snapshot = S("snapshot_1");
            manager.TryAddSnapshot(snapshot);

            // Act
            bool added = manager.TryAddSnapshot(snapshot);

            // Assert
            Assert.False(added);
            Assert.Equal(1, manager.Count);
        }

        [Fact]
        public void NotAddSameCurrentSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("snapshot_1");
            var secondSnapshot = S("snapshot_2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out var _);

            // Act
            bool added = manager.TryAddSnapshot(firstSnapshot);

            // Assert
            Assert.False(added);
            Assert.Equal(1, manager.Count);
        }

        [Fact]
        public void AddNewSnapshotIfLastOneChangedOnUndo()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("snapshot_1");
            var secondSnapshot = S("snapshot_2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            var changedSnapshot = S("snapshot_3");

            // Act
            manager.TryUndo(changedSnapshot, out var prevSnapshot);

            // Assert
            Assert.Equal(3, manager.Count);
            Assert.Equal(secondSnapshot, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void AddNewSnapshotIfCurrentOneChangedOnUndo()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("snapshot_1");
            var secondSnapshot = S("snapshot_2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out var _);
            var changedSnapshot = S("snapshot_3");

            // Act
            manager.TryUndo(changedSnapshot, out var prevSnapshot);

            // Assert
            Assert.Equal(2, manager.Count);
            Assert.Equal(firstSnapshot, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void AddNewSnapshotAndNotRedoIfLastOneChangedOnRedo()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("snapshot_1");
            var secondSnapshot = S("snapshot_2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out var _);
            var changedSnapshot = S("snapshot_3");

            // Act
            bool done = manager.TryRedo(changedSnapshot, out var prevSnapshot);

            // Assert
            Assert.False(done);
            Assert.Equal(2, manager.Count);
        }
    }
}
