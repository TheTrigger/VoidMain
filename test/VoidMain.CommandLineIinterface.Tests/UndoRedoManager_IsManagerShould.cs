using System;
using VoidMain.CommandLineIinterface.Tests.Tools;
using VoidMain.CommandLineIinterface.UndoRedo;
using Xunit;

namespace VoidMain.CommandLineIinterface.Tests
{
    public class UndoRedoManager_IsManagerShould
    {
        private static readonly IUndoRedoSnapshotEqualityComparer<string> Comparer
            = new UndoRedoSnapshotInvariantEqualityComparer();

        [Fact]
        public void RequireValidSnapshotsOnUndo()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string currentSnapshot = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => manager.TryUndo(currentSnapshot, out string _));
        }

        [Fact]
        public void RequireValidSnapshotsOnRedo()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string currentSnapshot = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => manager.TryUndo(currentSnapshot, out string _));
        }

        [Fact]
        public void RequireValidSnapshotsOnAdd()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string snapshot = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => manager.TryAddSnapshot(snapshot));
        }

        [Fact]
        public void NotUndoWithoutSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string currentSnapshot = "";

            for (int i = 0; i < 2; i++)
            {
                // Act
                bool done = manager.TryUndo(currentSnapshot, out string prevSnaoshot);

                // Assert
                Assert.Equal(false, done);
                Assert.Null(prevSnaoshot);
            }
        }

        [Fact]
        public void NotRedoWithoutSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string currentSnapshot = "";

            for (int i = 0; i < 2; i++)
            {
                // Act
                bool done = manager.TryRedo(currentSnapshot, out string nextSnaoshot);

                // Assert
                Assert.Equal(false, done);
                Assert.Null(nextSnaoshot);
            }
        }

        [Fact]
        public void NotUndoMoreThanItHaveSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string firstSnapshot = "snapshot_1";
            string secondSnapshot = "snapshot_2";
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            string currentSnapshot = secondSnapshot;
            string prevSnapshot = null;

            // Act
            bool done = manager.TryUndo(currentSnapshot, out prevSnapshot);
            currentSnapshot = prevSnapshot;
            done = manager.TryUndo(currentSnapshot, out prevSnapshot);

            // Assert
            Assert.Equal(false, done);
            Assert.Null(prevSnapshot);
        }

        [Fact]
        public void NotRedoMoreThanItHaveSnapshots()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string firstSnapshot = "snapshot_1";
            string secondSnapshot = "snapshot_2";
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            string currentSnapshot = secondSnapshot;
            string nextSnapshot = null;
            manager.TryUndo(currentSnapshot, out string prevSnaoshot);
            currentSnapshot = prevSnaoshot;

            // Act
            bool done = manager.TryRedo(currentSnapshot, out nextSnapshot);
            currentSnapshot = nextSnapshot;
            done = manager.TryRedo(currentSnapshot, out nextSnapshot);

            // Assert
            Assert.Equal(false, done);
            Assert.Null(nextSnapshot);
        }

        [Fact]
        public void AddNewSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string expected = "snapshot_1";
            string undoneSnapshot = "snapshot_2";
            manager.TryAddSnapshot(expected);

            // Act
            manager.TryAddSnapshot(undoneSnapshot);
            manager.TryUndo(undoneSnapshot, out string prevSnapshot);

            // Assert
            Assert.Equal(expected, prevSnapshot);
        }

        [Fact]
        public void NotExceedMaxSnapshotsCount()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);

            // Act
            for (int i = 0; i < manager.MaxCount + 1; i++)
            {
                manager.TryAddSnapshot("snapshot_" + i);
            }

            // Assert
            Assert.Equal(manager.MaxCount, manager.Count);
        }

        [Fact]
        public void DeleteAfterCurrent()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string firstSnapshot = "snapshot_1";
            string undoneSnapshot = "snapshot_2";
            string newSnapshot = "snapshot_3";
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(undoneSnapshot);
            manager.TryUndo(undoneSnapshot, out string _);

            // Act
            manager.TryAddSnapshot(newSnapshot, deleteAfter: true);
            manager.TryUndo(newSnapshot, out string prevSnapshot);

            // Assert
            Assert.Equal(2, manager.Count);
            Assert.Equal(firstSnapshot, prevSnapshot);
        }

        [Fact]
        public void NotDeleteAfterCurrentIfCurrentIsLast()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string firstSnapshot = "snapshot_1";
            string secondSnapshot = "snapshot_2";
            string newSnapshot = "snapshot_3";
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);

            // Act
            manager.TryAddSnapshot(newSnapshot, deleteAfter: true);
            manager.TryUndo(newSnapshot, out string prevSnapshot);

            // Assert
            Assert.Equal(3, manager.Count);
            Assert.Equal(secondSnapshot, prevSnapshot);
        }

        [Fact]
        public void NotAddSameLastSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string snapshot = "snapshot_1";
            manager.TryAddSnapshot(snapshot);

            // Act
            bool added = manager.TryAddSnapshot(snapshot);

            // Assert
            Assert.Equal(false, added);
            Assert.Equal(1, manager.Count);
        }

        [Fact]
        public void NotAddSameCurrentSnapshot()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string firstSnapshot = "snapshot_1";
            string secondSnapshot = "snapshot_2";
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out string _);

            // Act
            bool added = manager.TryAddSnapshot(firstSnapshot);

            // Assert
            Assert.Equal(false, added);
            Assert.Equal(1, manager.Count);
        }

        [Fact]
        public void AddNewSnapshotIfLastOneChangedOnUndo()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string firstSnapshot = "snapshot_1";
            string secondSnapshot = "snapshot_2";
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            string changedSnapshot = "snapshot_3";

            // Act
            manager.TryUndo(changedSnapshot, out string prevSnapshot);

            // Assert
            Assert.Equal(secondSnapshot, prevSnapshot);
            Assert.Equal(3, manager.Count);
        }

        [Fact]
        public void AddNewSnapshotIfCurrentOneChangedOnUndo()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string firstSnapshot = "snapshot_1";
            string secondSnapshot = "snapshot_2";
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out string _);
            string changedSnapshot = "snapshot_3";

            // Act
            manager.TryUndo(changedSnapshot, out string prevSnapshot);

            // Assert
            Assert.Equal(firstSnapshot, prevSnapshot);
            Assert.Equal(2, manager.Count);
        }

        [Fact]
        public void AddNewSnapshotAndNotRedoIfLastOneChangedOnRedo()
        {
            // Arrange
            var manager = new UndoRedoManager<string>(Comparer);
            string firstSnapshot = "snapshot_1";
            string secondSnapshot = "snapshot_2";
            manager.TryAddSnapshot(firstSnapshot);
            manager.TryAddSnapshot(secondSnapshot);
            manager.TryUndo(secondSnapshot, out string _);
            string changedSnapshot = "snapshot_3";

            // Act
            bool done = manager.TryRedo(changedSnapshot, out string prevSnapshot);

            // Assert
            Assert.Equal(false, done);
            Assert.Equal(2, manager.Count);
        }
    }
}
