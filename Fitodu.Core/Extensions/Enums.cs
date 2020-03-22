using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Fitodu.Core.Extensions
{
    public static class Enums
    {
        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public static T GetEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T? TryGetEnum<T>(this string value) where T : struct
        {
            T enumValue;
            if (Enum.TryParse<T>(value, true, out enumValue))
                return (T?)enumValue;
            return null;
        }

        public static IEnumerable<T> GetEnumValues<T>() where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
                return null;
            return Enum.GetValues(type).Cast<T>();
        }

        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("Value must be of Enum type", nameof(enumerationValue));
            }

            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();
                return attrs?.Description;
            }
            return enumerationValue.ToString();
        }
    }
}