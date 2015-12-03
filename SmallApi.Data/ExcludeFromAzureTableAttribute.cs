using System;

namespace SmallApi.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromAzureTableAttribute : Attribute
    {
    }
}
