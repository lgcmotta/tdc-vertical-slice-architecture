using System.Reflection;

namespace BankingApp.Domain.Core;

public abstract class ValueObject<TKey, TValue> : IComparable
    where TKey : IComparable
    where TValue : IComparable
{
    protected ValueObject(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

    public TKey Key { get; }

    public TValue Value { get; }

    public static IEnumerable<TValueObject> Enumerate<TValueObject>()
        where TValueObject : ValueObject<TKey, TValue>
    {
        var enumerationType = typeof(TValueObject);

        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

        var fields = enumerationType.GetFields(bindingFlags);

        var properties = enumerationType.GetProperties(bindingFlags);

        return properties.Select(property => property.GetValue(null))
            .Union(fields.Select(field => field.GetValue(null)))
            .Cast<TValueObject>();
    }

    public static TValueObject ParseByValue<TValueObject>(TValue value)
        where TValueObject : ValueObject<TKey, TValue> => Convert<TValueObject, TValue>(value, nameof(value), item => item.Value.Equals(value));

    public static bool TryParseByValue<TValueObject>(TValue value, out TValueObject? valueObject)
        where TValueObject : ValueObject<TKey, TValue>
    {
        valueObject = TryConvert<TValueObject>(item => item.Value.Equals(value));
        return valueObject is not null;
    }

    public static TValueObject ParseByKey<TValueObject>(TKey id)
        where TValueObject : ValueObject<TKey, TValue> => Convert<TValueObject, TKey>(id, nameof(id), item => item.Key.Equals(id));

    public static bool TryParseByKey<TValueObject>(TKey key, out TValueObject? valueObject)
        where TValueObject : ValueObject<TKey, TValue>
    {
        valueObject = TryConvert<TValueObject>(item => item.Key.Equals(key));
        return valueObject is not null;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((ValueObject<TKey, TValue>) obj);
    }

    public int CompareTo(object? obj)
    {
        if (obj is ValueObject<TKey, TValue> enumeration)
            return Key.CompareTo(enumeration.Key);

        return -1;
    }

    public override int GetHashCode() => HashCode.Combine(Key, Value);

    public static bool operator ==(ValueObject<TKey, TValue>? left, ValueObject<TKey, TValue>? right) =>
        left is not null && right is not null && left.Key.Equals(right.Key) && left.Value.Equals(right.Value);

    public static bool operator !=(ValueObject<TKey, TValue>? left, ValueObject<TKey, TValue>? right) =>
        left is null || right is null || !left.Key.Equals(right.Key) || !left.Value.Equals(right.Value);

    private bool Equals(ValueObject<TKey, TValue> other) =>
        Key.Equals(other.Key) && Value.Equals(other.Value);

    private static TValueObject Convert<TValueObject, T>(T value, string name, Func<TValueObject, bool> function)
        where TValueObject : ValueObject<TKey, TValue>
    {
        var item = TryConvert(function);

        if (item is null)
            throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not a valid {name} for type {typeof(TValueObject)}");

        return item;
    }

    private static TValueObject? TryConvert<TValueObject>(Func<TValueObject, bool> function)
        where TValueObject : ValueObject<TKey, TValue>
    {
        return Enumerate<TValueObject>().FirstOrDefault(function);
    }


}