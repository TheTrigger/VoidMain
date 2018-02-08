using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace VoidMain.Application.Builder.Tests
{
    public class ApplicationBuilder_Should
    {
        [Fact]
        public void InvokeInOrder()
        {
            // Arrange
            var builder = NewApplicationBuilder();
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
            var builder = NewApplicationBuilder();
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
            var builder = NewApplicationBuilder();
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
            var builder = NewApplicationBuilder();
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
            var builder = NewApplicationBuilder();
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

        #region Helpers

        private static readonly Lazy<IServiceProvider> Services = new Lazy<IServiceProvider>(
            () => new ServiceCollection().BuildServiceProvider());

        private static ApplicationBuilder NewApplicationBuilder()
        {
            return new ApplicationBuilder(Services.Value);
        }

        public class ComponentsInvokeTracker
        {
            private enum ComponentType
            {
                Log,
                Throw,
                Terminator
            }

            public List<string> Invoked { get; } = new List<string>();

            private CommandDelegate Component_N(CommandDelegate next, string name, ComponentType type)
            {
                return async context =>
                {
                    Invoked.Add(name);
                    if (type == ComponentType.Throw)
                    {
                        throw new Exception();
                    }
                    if (type != ComponentType.Terminator)
                    {
                        await next(context);
                    }
                };
            }

            public CommandDelegate Component_A(CommandDelegate next)
            {
                return Component_N(next, "A", ComponentType.Log);
            }

            public CommandDelegate Component_B(CommandDelegate next)
            {
                return Component_N(next, "B", ComponentType.Log);
            }

            public CommandDelegate Component_C(CommandDelegate next)
            {
                return Component_N(next, "C", ComponentType.Log);
            }

            public CommandDelegate Component_Throw(CommandDelegate next)
            {
                return Component_N(next, "Throw", ComponentType.Throw);
            }

            public CommandDelegate Component_Terminator(CommandDelegate next)
            {
                return Component_N(next, "Terminator", ComponentType.Terminator);
            }

            public Task Component_Run(Dictionary<string, object> context)
            {
                Invoked.Add("Run");
                return Task.CompletedTask;
            }

            public Predicate<Dictionary<string, object>> Condition(string name, bool satisfy)
            {
                return context =>
                {
                    Invoked.Add(name);
                    return satisfy;
                };
            }
        }

        #endregion
    }
}
