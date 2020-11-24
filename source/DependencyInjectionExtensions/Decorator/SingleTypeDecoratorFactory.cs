﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Decorator
{
    /// <summary>
    /// Use this extension for decorating implementations of a given service type
    /// </summary>
    /// <typeparam name="TService">Service type</typeparam>
    /// <typeparam name="TDecorator">Decorator</typeparam>
    public class SingleTypeDecoratorFactory<TService, TDecorator> : IDecoratorFactory
        where TDecorator : notnull, TService
    {
        // ReSharper disable once StaticMemberInGenericType : Is wanted behavior here
        private static readonly ConstructorInfo ConstructorInfo;

        static SingleTypeDecoratorFactory()
        {
            var constructorInfos = typeof(TDecorator).GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            if (constructorInfos.Length != 1)
                throw new InvalidOperationException($"Decorators must have one public constructor. {typeof(TDecorator).Name} has {constructorInfos.Length}");

            ConstructorInfo = constructorInfos.Single();
        }

        public object CreateDecorated(object toDecorate, Type decoratingType, IServiceProvider serviceProvider)
        {
            object CreateParameter(ParameterInfo parameterInfo)
            {
                var parameterType = parameterInfo.ParameterType;
                return parameterType == typeof(TService) ? toDecorate : serviceProvider.GetRequiredService(parameterType);
            }

            var parameters = ConstructorInfo
                .GetParameters()
                .Select(CreateParameter)
                .ToArray();

            return Activator.CreateInstance(typeof(TDecorator), parameters);
        }

        public bool CanDecorate(Type decoratingType)
        {
            return typeof(TService) == decoratingType;
        }
    }
}