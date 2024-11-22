using Microsoft.Extensions.Logging;
using Moq;
using ToDoTutorial.Core.Framework;
using ToDoTutorial.Core.Logic;

namespace ToDoTutorialTests
{
    [TestClass]
    public sealed class ToDoManagerTests
    {
        private static void DoTestSetup(int itemCount, out Mock<ILogger<ToDoManager>> logger, out ToDoManager manager)
        {
            logger = new Mock<ILogger<ToDoManager>>();
            manager = new ToDoManager(logger.Object, null);

            for (int i = 0; i < itemCount; i++)
            {
                manager.AddTaskAsync($"Task {i}").Wait();
            }
        }

        [TestMethod]
        public void GetCompletionString_NoTasks()
        {
            // Arrange
            DoTestSetup(0, out var _, out var manager);

            // Act
            var result = manager.GetCompletionString();

            // Assert
            Assert.AreEqual("(Currently No Tasks)", result);
        }

        [TestMethod]
        public void GetCompletionString_OneTask_NotComplete()
        {
            // Arrange
            DoTestSetup(1, out var _, out var manager);

            // Act
            var result = manager.GetCompletionString();

            // Assert
            Assert.AreEqual("(0 of 1) Task Complete", result);
        }

        [TestMethod]
        public async Task GetCompletionString_OneTask_Complete()
        {
            // Arrange
            DoTestSetup(1, out var _, out var manager);

            var items = await manager.GetItemsAsync();
            if(items is not null)
            {
                await manager.ToggleComplete(items[0].Id);
            }

            // Act
            var result = manager.GetCompletionString();

            // Assert
            Assert.AreEqual("(1 of 1) Task Complete", result);
        }

        [TestMethod]
        public void GetCompletionString_MultipleTasks_ZeroComplete()
        {
            // Arrange
            DoTestSetup(2, out var _, out var manager);

            // Act
            var result = manager.GetCompletionString();

            // Assert
            Assert.AreEqual("(0 of 2) Tasks Complete", result);
        }

        [TestMethod]
        public async Task GetCompletionString_MultipleTasks_OneComplete()
        {
            // Arrange
            DoTestSetup(2, out var _, out var manager);

            var items = await manager.GetItemsAsync();
            if (items is not null)
            {
                await manager.ToggleComplete(items[0].Id);
            }

            // Act
            var result = manager.GetCompletionString();

            // Assert
            Assert.AreEqual("(1 of 2) Tasks Complete", result);
        }

        [TestMethod]
        public async Task GetCompletionString_MultipleTasks_TwoComplete()
        {
            // Arrange
            DoTestSetup(2, out var _, out var manager);

            var items = await manager.GetItemsAsync();
            foreach(var item in items)
            {
                await manager.ToggleComplete(item.Id);
            }
            
            // Act
            var result = manager.GetCompletionString();

            // Assert
            Assert.AreEqual("(2 of 2) Tasks Complete", result);
        }

        [TestMethod]
        public async Task GetItemsAsync_GetsItems()
        {
            // Arrange
            DoTestSetup(5, out var _, out var manager);
            var items = await manager.GetItemsAsync();
            
            // Act
            var result = items.Count;

            // Assert
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public async Task AddTaskAsync_AddsOneItem()
        {
            // Arrange
            DoTestSetup(0, out var _, out var manager);
            await manager.AddTaskAsync("Task 1");
            var items = await manager.GetItemsAsync();

            // Act
            var result = items.Count;

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task ToggleComplete_TaskIsComplete()
        {
            // Arrange
            DoTestSetup(1, out var _, out var manager);
            var items = await manager.GetItemsAsync();
            if(items is not null)
            {
                await manager.ToggleComplete(items[0].Id);
            }
            // Act
            var result = items[0].Complete;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ToggleComplete_TaskIsNotComplete()
        {
            // Arrange
            DoTestSetup(1, out var _, out var manager);
            var items = await manager.GetItemsAsync();
            if (items is not null)
            {
                // Toggle True
                await manager.ToggleComplete(items[0].Id);
                // Toggle False
                await manager.ToggleComplete(items[0].Id);
            }
            // Act
            var result = items[0].Complete;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteTaskAsync_DeletesItem()
        {
            // Arrange
            DoTestSetup(1, out var _, out var manager);
            
            var items = await manager.GetItemsAsync();
            if (items is not null)
            {
                await manager.DeleteTaskAsync(items[0].Id);
                items = await manager.GetItemsAsync();
            }

            // Act
            var result = items.Count;

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}
