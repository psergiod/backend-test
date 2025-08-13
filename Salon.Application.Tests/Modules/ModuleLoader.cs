using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Salon.Application.Tests.Modules
{
    public class ModuleLoader
    {
        private ServiceCollection _serviceCollection;

        public ModuleLoader(ServiceCollection serviceCollection) 
        {
            _serviceCollection = serviceCollection;
        }
        public ModuleLoader UseModule<TModule>() where TModule : IModule, new()
        {
            _serviceCollection = new TModule().Register(_serviceCollection);

            return this;
        }

        public ServiceCollection Load()
        {
            return _serviceCollection;
        }
    }
}
