﻿namespace DynamicTypeHelpers.Tests.TestClasses
{
#pragma warning disable CA1040 // Avoid empty interfaces : Is for a test
    public interface IObjectUnderTest
#pragma warning restore CA1040 // Avoid empty interfaces
    {
    }

    public class ObjectUnderTest : IObjectUnderTest
    {
    }
}
