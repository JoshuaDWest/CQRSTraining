using System;
using System.Collections.Generic;
using System.Text;
using CommandStack.CommandHandler;
using CommandStack.CommandHandler.Internal;
using CommandStack.Sagas;
using Microsoft.Extensions.DependencyInjection;
using Models.Events;
using Xer.Cqrs;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.EventStack;
using Xer.Delegator.Registration;
using Xer.DomainDriven;
using Xer.DomainDriven.Repositories;

namespace CommandStack
{
    public static class Setup
    {
        public static void Begin(IServiceCollection services)
        {
            // Write-side repository.
            services.AddSingleton<IAggregateRootRepository<Match>>((serviceProvider) =>
                new PublishingAggregateRootRepository<Match>(new InMemoryAggregateRootRepository<Match>(),
                                                               serviceProvider.GetRequiredService<IDomainEventPublisher>())
            );

            // Domain event publisher.
            services.AddSingleton<IDomainEventPublisher, DomainEventPublisher>();

            // Register command delegator.
            services.AddSingleton<CommandDelegator>(serviceProvider =>
            {
                // Register command handlers.
                var commandHandlerRegistration = new SingleMessageHandlerRegistration();
                commandHandlerRegistration.RegisterCommandHandler(() => new SubmitMovesCommandHandler(serviceProvider.GetRequiredService<IAggregateRootRepository<Match>>()));
                commandHandlerRegistration.RegisterCommandHandler(() => new StartNewGameCommandHandler(serviceProvider.GetRequiredService<IAggregateRootRepository<Match>>()));
                commandHandlerRegistration.RegisterCommandHandler(() => new EndGameCommandHandler(serviceProvider.GetRequiredService<IAggregateRootRepository<Match>>()));
                commandHandlerRegistration.RegisterCommandHandler(() => new ProcessTurnCommandHandler(serviceProvider.GetRequiredService<IAggregateRootRepository<Match>>()));

                return new CommandDelegator(commandHandlerRegistration.BuildMessageHandlerResolver());
            });
        }

        public static void RegisterEventHandlers(IServiceProvider serviceProvider, MultiMessageHandlerRegistration register)
        {
            //rather than lazy's this should probably go through a handler that goes to a singleton saga like the query side does with the repo, but this works too
            //game saga
            var gameSaga = new Lazy<GameSaga>(() => new GameSaga(serviceProvider.GetRequiredService<CommandDelegator>()));
            register.RegisterEventHandler<MatchStartedEvent>(() => gameSaga.Value);
            register.RegisterEventHandler<DeployedEvent>(() => gameSaga.Value);
            register.RegisterEventHandler<DestroyedEvent>(() => gameSaga.Value);

            //match turn saga
            var matchTurnSaga = new Lazy<MatchTurnSaga>(() => new MatchTurnSaga(serviceProvider.GetRequiredService<CommandDelegator>()));
            register.RegisterEventHandler<AllPlayerMovesSubmittedForTurnEvent>(() => matchTurnSaga.Value);
        }
    }
}
