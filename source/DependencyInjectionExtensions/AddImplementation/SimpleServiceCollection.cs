﻿using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.AddImplementation
{
    public class SimpleServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
}