using Autofac;
using BankingApp.Application.Behaviours;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using System.Reflection;
using Module = Autofac.Module;

namespace BankingApp.Application.Autofac;

public class MediatorModule : Module
{
    private readonly Assembly[] _assemblies;

    public MediatorModule(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterMediatR(_assemblies);

        builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

        builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

        builder.RegisterGeneric(typeof(TransactionalBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

        base.Load(builder);
    }
}