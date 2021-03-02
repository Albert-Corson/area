using System;
using System.Collections.Generic;
using System.Linq;
using Area.API.Models.Table;
using Area.API.Models.Table.ManyToMany;

namespace Area.API.Extensions
{
    public static class IEnumerableExtension
    {
        public static string? GetValue(this IEnumerable<ParamModel> dictionary, string key) =>
            dictionary.First(model => model.Name == key)?.Value;

        public static TEnum? GetEnumValue<TEnum>(this IEnumerable<ParamModel> dictionary, string key)
            where TEnum : struct, IConvertible =>
            Enum.TryParse<TEnum>(dictionary.First(model => model.Name == key).Value!, true, out var result)
                ? (TEnum?) result
                : null;

        public static TType GetValue<TType>(this IEnumerable<ParamModel> dictionary, string key) =>
            (TType) dictionary.First(model => model.Name == key).ConvertedValue!;
    }
}