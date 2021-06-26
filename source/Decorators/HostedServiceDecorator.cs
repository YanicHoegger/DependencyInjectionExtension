﻿using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace Nricher.Decorators
{
    public class HostedServiceDecorator : DecoratorBase<HostedServiceDecorator>
    {
        private bool _isStarted;

        public HostedServiceDecorator()
        {
            var disposeMethod = typeof(IDisposable).GetMethod(nameof(IDisposable.Dispose)) 
                                ?? throw new InvalidOperationException($"{nameof(IDisposable.Dispose)} method can not be found");
            IgnoringMethods.Add(disposeMethod);
        }

        protected override object? InvokeInternal(MethodInfo targetMethod, object?[]? args)
        {
            if (targetMethod == null) 
                throw new ArgumentNullException(nameof(targetMethod));

            switch (targetMethod.Name)
            {
                case nameof(IHostedService.StartAsync):
                    _isStarted = true;
                    break;
                case nameof(IHostedService.StopAsync):
                    _isStarted = false;
                    break;
                default:
                {
                    if (!_isStarted)
                        throw new InvalidOperationException($"{Decorated.GetType().Name} needs to be started");
                    break;
                }
            }

            return targetMethod.Invoke(Decorated, args);
        }

        public static object Create([NotNull] IHostedService hostedService)
        {
            return CreateDecorator<HostedServiceDecorator>(hostedService);
        }
    }
}
