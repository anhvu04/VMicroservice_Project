using System.Reflection;
using Mapster;

namespace Infrastructure.Extensions.MappingExtensions;

public static class MapsterExtensions
{
    public static TypeAdapterSetter<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(
        this TypeAdapterSetter<TSource, TDestination> setter)
    {
        var destinationType = typeof(TDestination);
        var sourceType = typeof(TSource);

        var destinationProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destProperty in destinationProperties)
        {
            var sourceProperty = sourceType.GetProperty(destProperty.Name, BindingFlags.Public | BindingFlags.Instance);

            if (sourceProperty == null)
            {
                setter.Ignore(destProperty.Name);
            }
        }

        return setter;
    }

    public static TypeAdapterSetter<TSource, TDestination> IgnoreNullProperties<TSource, TDestination>(
        this TypeAdapterSetter<TSource, TDestination> setter)
    {
        // Enable ignoring null values globally for this mapping
        setter.IgnoreNullValues(true);

        return setter;
    }
}