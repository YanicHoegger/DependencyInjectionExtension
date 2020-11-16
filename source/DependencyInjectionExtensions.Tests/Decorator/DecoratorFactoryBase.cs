﻿using System;

namespace DependencyInjectionExtensions.Tests.Decorator
{
    public abstract class DecoratorFactoryBase : IDecoratorFactory
    {
        public object CreateDecorated(object toDecorate, Type decoratingType, IServiceProvider serviceProvider)
        {
            return DecoratorType
                       .MakeGenericType(decoratingType)
                       .GetMethod(MethodName)
                       ?.Invoke(null, new[] { toDecorate })
                   ?? throw new InvalidOperationException($"Could not find method {MethodName} in {DecoratorType}");
        }

        public bool CanDecorate(Type decoratingType)
        {
            return decoratingType.IsInterface;
        }

        protected abstract Type DecoratorType { get; }
        protected abstract string MethodName { get; }
    }
}
