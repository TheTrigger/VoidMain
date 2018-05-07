using System;
using VoidMain.CommandLineIinterface.IO.Views;
using Xunit;

namespace VoidMain.CommandLineIinterface.UndoRedo.Tests
{
    public class UndoRedoManager_Should
    {
        #region Ctor tests

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void HaveValidMaxCount(int maxCount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new UndoRedoManager(new UndoRedoOptions { MaxCount = maxCount }));
        }

        #endregion

        #region Null spanshot tessts

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

        #endregion

        #region Empty history tests

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

        #endregion

        #region History end tests

        [Fact]
        public void NotUndoMoreThanItHaveSnapshots()
        {
            // Arrange state
            var manager = new UndoRedoManager();
            var firstSnapshot = S("S1");
            var secondSnapshot = S("S2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            var currentSnapshot = secondSnapshot;
            // Current state: S1, [S2]

            // Act
            manager.TryUndo(currentSnapshot, out var prevSnapshot);
            currentSnapshot = prevSnapshot;
            // Current state: [S1], S2
            bool done = manager.TryUndo(currentSnapshot, out prevSnapshot);
            // Current state: [S1], S2

            // Assert
            Assert.False(done);
        }

        [Fact]
        public void NotRedoMoreThanItHaveSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("S1");
            var secondSnapshot = S("S2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            var currentSnapshot = secondSnapshot;
            manager.TryUndo(currentSnapshot, out var prevSnaoshot);
            currentSnapshot = prevSnaoshot;
            // Current state: [S1], S2

            // Act
            manager.TryRedo(currentSnapshot, out var nextSnapshot);
            currentSnapshot = nextSnapshot;
            // Current state: S1, [S2]
            bool done = manager.TryRedo(currentSnapshot, out nextSnapshot);
            // Current state: S1, [S2]

            // Assert
            Assert.False(done);
        }

        #endregion

        #region AddSnapshot tests

        [Fact]
        public void AddNewSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var expected = S("S1");
            var undoneSnapshot = S("S2");
            manager.TryAddSnapshot(expected);
            // Current state: [S1]

            // Act
            manager.TryAddSnapshot(undoneSnapshot);
            // Current state: S1, [S2]
            manager.TryUndo(undoneSnapshot, out var prevSnapshot);
            // Current state: [S1], S2

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
                manager.TryAddSnapshot(S("S" + i));
            }

            // Assert
            Assert.Equal(manager.MaxCount, manager.Count);
        }

        [Fact]
        public void NotAddSameLastSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var snapshot = S("S1");
            manager.TryAddSnapshot(snapshot);
            // Current state: [S1]

            // Act
            bool added = manager.TryAddSnapshot(snapshot);
            // Current state: [S1]

            // Assert
            Assert.False(added);
            Assert.Equal(1, manager.Count);
        }

        [Fact]
        public void NotAddSameCurrentSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("S1");
            var secondSnapshot = S("S2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out var _);
            // Current state: [S1], S2

            // Act
            bool added = manager.TryAddSnapshot(firstSnapshot);
            // Current state: [S1], S2

            // Assert
            Assert.False(added);
            Assert.Equal(2, manager.Count);
        }

        #endregion

        [Fact]
        public void DeleteAfterCurrent()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("S1");
            var undoneSnapshot = S("S2");
            var newSnapshot = S("S3");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(undoneSnapshot);
            manager.TryUndo(undoneSnapshot, out var _);
            // Current state: [S1], S2

            // Act
            manager.TryAddSnapshot(newSnapshot);
            // Current state: S1, [S3]
            manager.TryUndo(newSnapshot, out var prevSnapshot);
            // Current state: [S1], S3

            // Assert
            Assert.Equal(2, manager.Count);
            Assert.Equal(firstSnapshot, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void NotDeleteAfterCurrentIfCurrentIsLast()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("S1");
            var secondSnapshot = S("S2");
            var newSnapshot = S("S3");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            // Current state: S1, [S2]

            // Act
            manager.TryAddSnapshot(newSnapshot);
            // Current state: S1, S2, [S3]
            manager.TryUndo(newSnapshot, out var prevSnapshot);
            // Current state: S1, [S2], S3

            // Assert
            Assert.Equal(3, manager.Count);
            Assert.Equal(secondSnapshot, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void AddNewSnapshotIfLastOneChangedOnUndo()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("S1");
            var secondSnapshot = S("S2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            var changedSnapshot = S("S3");
            // Current state: S1, [S2]

            // Act
            manager.TryUndo(changedSnapshot, out var prevSnapshot);
            // Current state: S1, [S2], S3

            // Assert
            Assert.Equal(3, manager.Count);
            Assert.Equal(secondSnapshot, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void AddNewSnapshotIfCurrentOneChangedOnUndo()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("S1");
            var secondSnapshot = S("S2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out var _);
            var changedSnapshot = S("S3");
            // Current state: [S1], S2

            // Act
            manager.TryUndo(changedSnapshot, out var prevSnapshot);
            // Current state: [S1], S3

            // Assert
            Assert.Equal(2, manager.Count);
            Assert.Equal(firstSnapshot, prevSnapshot, CommandLineViewSnapshotComparer.IgnoreCursor);
        }

        [Fact]
        public void AddNewSnapshotAndNotRedoIfLastOneChangedOnRedo()
        {
            // Arrange
            var manager = new UndoRedoManager();
            var firstSnapshot = S("S1");
            var secondSnapshot = S("S2");
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out var _);
            var changedSnapshot = S("S3");
            // Current state: [S1], S2

            // Act
            bool done = manager.TryRedo(changedSnapshot, out var prevSnapshot);
            // Current state: S1, [S3]

            // Assert
            Assert.False(done);
            Assert.Equal(2, manager.Count);
        }

        #region Helpers

        private static CommandLineViewSnapshot S(string value)
        {
            return new CommandLineViewSnapshot(value, 0);
        }

        #endregion
    }
}
