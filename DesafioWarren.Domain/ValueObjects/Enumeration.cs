using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DesafioWarren.Domain.ValueObjects
{
    public class Enumeration : IComparable
    {
        public int Id { get; }

        public string Value { get; }

        protected Enumeration(int id, string value)
        {
            Id = id;
            Value = value;
        }

        private static TEnumeration Convert<TEnumeration, TValue>(TValue value, string description, Func<TEnumeration, bool> function) where TEnumeration : Enumeration
        {
            var item = GetEnumerationItems<TEnumeration>().FirstOrDefault(function);

            if (item is null)
                throw new ArgumentOutOfRangeException(nameof(value)
                    , $"{value} is not a valid {description} for type {typeof(TEnumeration)}");

            return item;
        }


        public static IEnumerable<TEnumeration> GetEnumerationItems<TEnumeration>() where TEnumeration : Enumeration
        {
            var enumerationType = typeof(TEnumeration);

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

            var fields = enumerationType.GetFields(bindingFlags);

            var properties = enumerationType.GetProperties(bindingFlags);

            return properties.Select(property => property.GetValue(null))
                .Union(fields.Select(field => field.GetValue(null)))
                .Cast<TEnumeration>();
        }

        public static TEnumeration GetItemByValue<TEnumeration>(string value) where TEnumeration : Enumeration => 
            Convert<TEnumeration, string>(value, nameof(value), item => item.Value == value);

        public static TEnumeration GetItemById<TEnumeration>(int id) where TEnumeration : Enumeration => 
            Convert<TEnumeration, int>(id, nameof(id), item => item.Id == id);


        public int CompareTo(object? obj)
        {
            if (obj is Enumeration enumeration)
                return Id.CompareTo(enumeration.Id);

            return -1;
        }

        protected bool Equals(Enumeration other)
        {
            return Id == other.Id && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Enumeration) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Value);
        }

        public static bool operator ==(Enumeration left, Enumeration right) => 
            left is not null && right is not null && left.Id == right.Id && left.Value == right.Value;

        public static bool operator !=(Enumeration left, Enumeration right) =>
            left is null || right is null || left.Id != right.Id || left.Value != right.Value;
    }
}