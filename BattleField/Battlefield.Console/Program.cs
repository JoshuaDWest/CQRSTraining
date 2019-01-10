using System;
using System.Collections.Generic;
using CommandStack.Commands;
using Microsoft.Extensions.DependencyInjection;
using Models.Enums;
using ReadStack.Queries;
using ReadStack.ReadModel;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.EventStack;
using Xer.Cqrs.QueryStack;
using Xer.Delegator.Registration;

namespace Battlefield.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();
            var commandBus = container.GetService<CommandDelegator>();
            var queryBus = container.GetService<IQueryAsyncDispatcher>();
            System.Console.WriteLine("Game initialized...");

            var matchId = Guid.NewGuid();
            var newGame = new StartNewGameCommand(matchId);
            commandBus.SendAsync(newGame).Wait();
            
            System.Console.WriteLine("New game started...");
            var activeMatch = queryBus.DispatchAsync<QueryActiveMatch, MatchReadModel>(new QueryActiveMatch()).Result;
            System.Console.WriteLine($"The active match is: {activeMatch.Id}");

            var p1move = new SubmitMovesCommand()
            {
                Player = "player1",
                Match = matchId,
                Attacks = new List<Attack>()
                {
                    new Attack(){ Source = UnitType.PlaneBomber, Target = UnitType.TankHeavy },
                    new Attack(){ Source = UnitType.PlaneFighter, Target = UnitType.PlaneFighter },
                    new Attack(){ Source = UnitType.TankLight, Target = UnitType.Infantry },
                    new Attack(){ Source = UnitType.TankHeavy, Target = UnitType.TankHeavy },
                    new Attack(){ Source = UnitType.Infantry, Target = UnitType.TankHeavy }
                },
                Deployments = new List<Deploy>
                {
                    new Deploy(){ Type = UnitType.TankHeavy },
                    new Deploy(){ Type = UnitType.PlaneBomber }
                }
            };

            var p2move = new SubmitMovesCommand()
            {
                Player = "player2",
                Match = matchId,
                Attacks = new List<Attack>()
                {
                    new Attack(){ Source = UnitType.PlaneBomber, Target = UnitType.PlaneBomber },
                    new Attack(){ Source = UnitType.PlaneFighter, Target = UnitType.PlaneBomber },
                    new Attack(){ Source = UnitType.TankLight, Target = UnitType.TankLight },
                    new Attack(){ Source = UnitType.TankHeavy, Target = UnitType.TankLight },
                    new Attack(){ Source = UnitType.Infantry, Target = UnitType.TankHeavy }
                },
                Deployments = new List<Deploy>
                {
                    new Deploy(){ Type = UnitType.TankLight },
                    new Deploy(){ Type = UnitType.Infantry },
                    new Deploy(){ Type = UnitType.Infantry }
                }
            };

            commandBus.SendAsync(p1move).Wait();
            commandBus.SendAsync(p2move).Wait();

            activeMatch = queryBus.DispatchAsync<QueryActiveMatch, MatchReadModel>(new QueryActiveMatch()).Result;
            System.Console.WriteLine($"The active match is: {activeMatch.Id}");

            System.Console.WriteLine("Game Finished. Press any key to exit..");
            System.Console.Read();
        }


        private static IServiceProvider BuildContainer()
        {
            IServiceCollection services = new ServiceCollection();

            //setup command stack
            CommandStack.Setup.Begin(services);
            //setup query stack
            ReadStack.Setup.Begin(services);

            // Register event delegator.
            services.AddSingleton<EventDelegator>((serviceProvider) =>
            {
                // Register event handlers.
                var eventHandlerRegistration = new MultiMessageHandlerRegistration();
                CommandStack.Setup.RegisterEventHandlers(serviceProvider, eventHandlerRegistration);
                ReadStack.Setup.RegisterEventHandlers(serviceProvider, eventHandlerRegistration);
                return new EventDelegator(eventHandlerRegistration.BuildMessageHandlerResolver());
            });

            // Register container adapters to be used by resolvers.
            services.AddSingleton<SimpleInjectorContainerAdapter>(new SimpleInjectorContainerAdapter(new Lazy<IServiceProvider>(() => services.BuildServiceProvider())));
            return services.BuildServiceProvider();
        }


    }

    class SimpleInjectorContainerAdapter : Xer.Cqrs.CommandStack.Resolvers.IContainerAdapter,
        Xer.Cqrs.QueryStack.Resolvers.IContainerAdapter,
        Xer.Cqrs.EventStack.Resolvers.IContainerAdapter
    {
        private readonly Lazy<IServiceProvider> _container;

        public SimpleInjectorContainerAdapter(Lazy<IServiceProvider> container)
        {
            _container = container;
        }

        public T Resolve<T>() where T : class
        {
            return _container.Value.GetService<T>();
        }

        public IEnumerable<T> ResolveMultiple<T>() where T : class
        {
            return _container.Value.GetServices<T>();
        }
    }
}
