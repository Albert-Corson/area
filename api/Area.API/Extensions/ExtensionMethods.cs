using System.Collections.Generic;
using System.Linq;
using Area.API.Models.Table;

namespace Area.API.Extensions
{
    public static class ExtensionMethods
    {
        public static string? GetValue(this IEnumerable<ParamModel> dictionary, string key)
        {
            return dictionary.FirstOrDefault(model => model.Name == key)?.Value;
        }
    }
}