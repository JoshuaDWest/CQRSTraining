using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Models.Events;
using ReadStack.EventHandlers;
using ReadStack.QueryHandlers;
using ReadStack.Repositories;
using Xer.Cqrs.EventStack;
using Xer.Cqrs.QueryStack;
using Xer.Cqrs.QueryStack.Dispatchers;
using Xer.Cqrs.QueryStack.Registrations;
using Xer.Delegator.Registration;

namespace ReadStack
{
    public static class Setup
    {
        public static void Begin(IServiceCollection services)
        {
            // Read-side repository.
            services.AddSingleton<IMatchReadStackRepository, MatchReadStackRepository>();

            // Register query dispatcher.
            services.AddSingleton<IQueryAsyncDispatcher>(serviceProvider =>
            {
                // Register query handlers.
                var registration = new QueryHandlerRegistration();
                registration.Register(() => new QueryAllMatchesHandler(serviceProvider.GetRequiredService<IMatchReadStackRepository>()));
                registration.Register(() => new QueryActiveMatchHandler(serviceProvider.GetRequiredService<IMatchReadStackRepository>()));

                return new QueryDispatcher(registration);
            });
        }

        public static void RegisterEventHandlers(IServiceProvider serviceProvider, MultiMessageHandlerRegistration register)
        {
            register.RegisterEventHandler<MatchStartedEvent>(() => new MatchEventHandlers(serviceProvider.GetRequiredService<IMatchReadStackRepository>()));
            register.RegisterEventHandler<MatchEndedEvent>(() => new MatchEventHandlers(serviceProvider.GetRequiredService<IMatchReadStackRepository>()));
        }
    }
}
