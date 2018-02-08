using System;
using VoidMain.Application.Builder;
using VoidMain.Hosting.Tests.Tools;
using Xunit;

namespace VoidMain.Hosting.Tests
{
    public class ApplicationBuilder_IsComponentsShould
    {
        [Fact]
        public void InvokeInOrder()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var tracker = new ComponentsInvokeTracker();

            // Act
            builder.Use(tracker.Component_A);
            builder.Use(tracker.Component_B);
            builder.Use(tracker.Component_Terminator);
            var app = builder.Build();
            app(null).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(new[] { "A", "B", "Terminator" }, tracker.Invoked);
        }

        [Fact]
        public void ThrowWithoutTerminator()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var tracker = new ComponentsInvokeTracker();

            // Act
            builder.Use(tracker.Component_A);
            builder.Use(tracker.Component_B);
            var app = builder.Build();

            // Assert
            Assert.Throws<InvalidOperationException>(() => app(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void StopAfterUseTerminator()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var tracker = new ComponentsInvokeTracker();

            // Act
            builder.Use(tracker.Component_A);
            builder.Use(tracker.Component_B);
            builder.Use(tracker.Component_Terminator);
            builder.Use(tracker.Component_C);
            var app = builder.Build();
            app(null).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(new[] { "A", "B", "Terminator" }, tracker.Invoked);
        }

        [Fact]
        public void StopAfterRunTerminator()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var tracker = new ComponentsInvokeTracker();

            // Act
            builder.Use(tracker.Component_A);
            builder.Use(tracker.Component_B);
            builder.Run(tracker.Component_Run);
            builder.Use(tracker.Component_C);
            var app = builder.Build();
            app(null).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(new[] { "A", "B", "Run" }, tracker.Invoked);
        }

        [Fact]
        public void InvokeInCorrectBranch()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var tracker = new ComponentsInvokeTracker();

            // Act
            builder.Use(tracker.Component_A);
            builder.UseWhen(tracker.Condition("Branch_1", true), branchOneApp =>
            {
                branchOneApp.Use(tracker.Component_B);
                branchOneApp.UseWhen(tracker.Condition("Branch_2", false), branchTwoApp =>
                {
                    branchTwoApp.Use(tracker.Component_C);
                });
                branchOneApp.Use(tracker.Component_Terminator);
            });
            builder.Run(tracker.Component_Run);
            builder.Use(tracker.Component_C);
            var app = builder.Build();
            app(null).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(new[] { "A", "Branch_1", "B", "Branch_2", "Terminator" }, tracker.Invoked);
        }
    }
}
