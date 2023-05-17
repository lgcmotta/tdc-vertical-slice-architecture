using System.Reflection;
using Autofac;
using FluentValidation;
using Module = Autofac.Module;

namespace DesafioWarren.Application.Autofac
{
    public class ValidatorModule : Module
    {
        private readonly Assembly[] _assemblies;

        public ValidatorModule(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            AssemblyScanner.FindValidatorsInAssemblies(_assemblies)
                .ForEach(scannedAssembly => builder
                    .RegisterType(scannedAssembly.ValidatorType)
                    .As(scannedAssembly.InterfaceType)
                    .InstancePerLifetimeScope());
            
            base.Load(builder);
        }
    }
}