namespace BankingApp.Infrastructure.Core.Extensions;

public static class InfrastructureTypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        if (!type.IsGenericType) return type.Name;

        var genericTypes = string.Join(",", type.GetGenericArguments().Select(argument => argument.Name));

        var genericTypeName = $"{type.Name.Replace("`", string.Empty)}<{genericTypes}>";

        return genericTypeName;
    }
}