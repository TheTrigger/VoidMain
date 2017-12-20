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
            var traker = new ComponentsInvokeTracker();

            // Act
            builder.Use(traker.Component_A);
            builder.Use(traker.Component_B);
            builder.Use(traker.Component_Terminator);
            var app = builder.Build();
            app(null).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(new[] { "A", "B", "Terminator" }, traker.Invoked);
        }

        [Fact]
        public void ThrowWithoutTerminator()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var traker = new ComponentsInvokeTracker();

            // Act
            builder.Use(traker.Component_A);
            builder.Use(traker.Component_B);
            var app = builder.Build();

            // Assert
            Assert.Throws<InvalidOperationException>(() => app(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void StopAfterUseTerminator()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var traker = new ComponentsInvokeTracker();

            // Act
            builder.Use(traker.Component_A);
            builder.Use(traker.Component_B);
            builder.Use(traker.Component_Terminator);
            builder.Use(traker.Component_C);
            var app = builder.Build();
            app(null).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(new[] { "A", "B", "Terminator" }, traker.Invoked);
        }

        [Fact]
        public void StopAfterRunTerminator()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var traker = new ComponentsInvokeTracker();

            // Act
            builder.Use(traker.Component_A);
            builder.Use(traker.Component_B);
            builder.Run(traker.Component_Run);
            builder.Use(traker.Component_C);
            var app = builder.Build();
            app(null).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(new[] { "A", "B", "Run" }, traker.Invoked);
        }

        [Fact]
        public void InvokeInCorrectBranch()
        {
            // Arrange
            var builder = new ApplicationBuilder(Services.Empty);
            var traker = new ComponentsInvokeTracker();

            // Act
            builder.Use(traker.Component_A);
            builder.UseWhen(traker.Condition("Branch_1", true), branchOneApp =>
            {
                branchOneApp.Use(traker.Component_B);
                branchOneApp.UseWhen(traker.Condition("Branch_2", false), branchTwoApp =>
                {
                    branchTwoApp.Use(traker.Component_C);
                });
                branchOneApp.Use(traker.Component_Terminator);
            });
            builder.Run(traker.Component_Run);
            builder.Use(traker.Component_C);
            var app = builder.Build();
            app(null).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(new[] { "A", "Branch_1", "B", "Branch_2", "Terminator" }, traker.Invoked);
        }
    }
}
