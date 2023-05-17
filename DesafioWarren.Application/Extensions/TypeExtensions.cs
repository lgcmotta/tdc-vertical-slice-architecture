using System;
using System.Linq;

namespace DesafioWarren.Application.Extensions
{
    public static class TypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            if (!type.IsGenericType) return type.Name;

            var genericTypes = string.Join(","
                , type.GetGenericArguments().Select(genericArgument => genericArgument.Name));

            var typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";

            return typeName;
        }

        public static string GetGenericTypeName(this object @object) => @object.GetType().GetGenericTypeName();
    }
}