﻿namespace BankingApp.Transactions.Domain.ValueObjects;

public sealed class Money : IFormattable
{
    private readonly decimal _value;

    private Money() => _value = decimal.Zero;

    public Money(decimal value)
    {
        _value = value;
    }

    public decimal Value => TruncateValue(_value);

    public static Money Zero => new(decimal.Zero);

    public Money Negative()
    {
        return TruncateValue(_value * -1.0m);
    }

    public static Money ConvertToUSD(Money money, Currency currency)
    {
        return money / currency.DollarExchangeRate;
    }

    public static Money ConvertFromUSD(Money money, Currency currency)
    {
        return money * currency.DollarExchangeRate;
    }

    private Money TruncateValue(decimal value)
    {
        const int precision = 2;

        var roundValue = Math.Round(value, precision);

        return _value switch
        {
            > 0 when roundValue > value => roundValue - new decimal(1, 0, 0, false, precision),
            < 0 when roundValue < value => roundValue + new decimal(1, 0, 0, false, precision),
            _ => roundValue
        };
    }

    public string Format(Currency currency)
    {
        return _value < decimal.Zero
            ? $"-{currency.Symbol}{TruncateValue(_value * -1):F}"
            : $"{currency.Symbol}{TruncateValue(_value):F}";
    }

    public static implicit operator decimal(Money money) => money._value;
    public static implicit operator Money(decimal value) => new(value);

    public static Money operator +(Money left, Money right)
    {
        if (left == null) throw new ArgumentNullException(nameof(left));
        if (right == null) throw new ArgumentNullException(nameof(right));
        return left._value + right._value;
    }

    public static Money operator -(Money left, Money right)
    {
        if (left == null) throw new ArgumentNullException(nameof(left));
        if (right == null) throw new ArgumentNullException(nameof(right));
        return left._value - right._value;
    }

    public static Money operator *(Money left, Money right)
    {
        if (left == null) throw new ArgumentNullException(nameof(left));
        if (right == null) throw new ArgumentNullException(nameof(right));
        return left._value * right._value;
    }

    public static Money operator /(Money left, Money right)
    {
        if (left == null) throw new ArgumentNullException(nameof(left));
        if (right == null) throw new ArgumentNullException(nameof(right));
        return left._value / right._value;
    }

    public static bool operator ==(Money? left, Money? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        var compare = decimal.Compare(left._value, right._value);
        return compare == 0;
    }

    public static bool operator !=(Money left, Money right) => !(left == right);

    public static bool operator <(Money left, Money right)
    {
        return decimal.Compare(left._value, right._value) switch
        {
            < 0 => true,
            _ => false,
        };
    }

    public static bool operator >(Money left, Money right)
    {
        return decimal.Compare(left._value, right._value) switch
        {
            > 0 => true,
            _ => false,
        };
    }

    public static bool operator <=(Money left, Money right)
    {
        return decimal.Compare(left._value, right._value) switch
        {
            <= 0 => true,
            _ => false,
        };
    }

    public static bool operator >=(Money left, Money right)
    {
        return decimal.Compare(left._value, right._value) switch
        {
            >= 0 => true,
            _ => false,
        };
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value.ToString(format, formatProvider);
    }

    private bool Equals(Money other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Money other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}