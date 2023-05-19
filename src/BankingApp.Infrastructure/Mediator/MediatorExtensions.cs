﻿using BankingApp.Domain.Entities;
using BankingApp.Infrastructure.EntityFramework.DbContexts;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Mediator;

public static class MediatorExtensions
{
    public static async Task<int> DispatchDomainEventsAsync(this IMediator mediator, AccountsDbContext dbContext)
    {
        var entities = dbContext.ChangeTracker.Entries<Entity>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = entities.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

        entities.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        var tasks = domainEvents.Select(async domainEvent => await mediator.Publish(domainEvent));

        await Task.WhenAll(tasks);

        return domainEvents.Count;
    }
}