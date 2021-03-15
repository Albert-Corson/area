using System;

namespace Area.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ValidateIpAddressAttribute : Attribute
    { }
}