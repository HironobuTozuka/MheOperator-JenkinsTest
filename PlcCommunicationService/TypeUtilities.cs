using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace PlcCommunicationService
{
    public static class TypeUtilities
    {
        public static List<T> GetAllPublicConstantValues<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                .Select(x => (T)x.GetRawConstantValue())
                .ToList();
        }

        public static List<T> GetAllPublicConstantValues<T>(this Type type, string name)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => !fi.IsInitOnly && fi.FieldType == typeof(T) && fi.Name == name)
                .Select(x => (T)x.GetValue(null))
                .ToList();
        }
    }
}
