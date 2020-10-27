using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace SharedClasses
{
    public static class ObjectDataHelper
    {
        public static ObjectFieldData recordKey(List<ObjectDataHelper.ObjectFieldData> _fieldsList, string _key)
        {
            foreach (ObjectDataHelper.ObjectFieldData field in _fieldsList)
            {
                if (field.fieldName == _key)
                    return field;
            }
            return null;
        }

        public static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(short),
            typeof(int),
            typeof(uint),
            typeof(double),
            typeof(decimal),
        };

        public static bool isNumericType(Type type)
        {
            return NumericTypes.Contains(type) ||
                   NumericTypes.Contains(Nullable.GetUnderlyingType(type));
        }
        private static int enumToShort(object _o, PropertyInfo _p)
        {
            object underlyingValue = Convert.ChangeType(_p.GetValue(_o), Enum.GetUnderlyingType(_p.GetValue(_o).GetType()));
            return (int)underlyingValue;
        }

        public class ObjectFieldData
        {
            public string fieldName, value;
            public Type type;
        }

        public static List<string> fieldNames(List<ObjectFieldData> _fieldsList)
        {
            List<string> collection = new List<string>();

            foreach (ObjectFieldData field in _fieldsList)
            {
                collection.Add(field.fieldName);
            }

            return collection;
        }

        public static List<ObjectFieldData> extractModelData(object o)
        {
            if (o == null)
                throw new ArgumentNullException();

            var type = o.GetType();
            var propertyInfos = type.GetProperties()
                .Select(p => new
                {
                    Name = p.GetCustomAttribute<DisplayAttribute>() != null ? p.GetCustomAttribute<DisplayAttribute>().Name : p.Name,
                    FormatAttribute = p.GetCustomAttribute<DisplayFormatAttribute>(),
                    Property = p,
                    Type = p.PropertyType.IsEnum ? typeof(Int32) : p.PropertyType,
                    Value = p.PropertyType.IsEnum ? enumToShort(o, p) : p.GetValue(o)
                });

            List<ObjectFieldData> list = new List<ObjectFieldData>();
            foreach (var property in propertyInfos)
                list.Add(new ObjectFieldData() { fieldName = property.Name, type = property.Type, value = FormatValue(property.Value, property.Type, property.FormatAttribute) });

            return list;
        }

        public static Dictionary<string, double> arguments(object o)
        {

            if (o == null)
                throw new ArgumentNullException();

            var type = o.GetType();
            var propertyInfos = type.GetProperties()
                .Select(p => new
                {
                    Name = p.GetCustomAttribute<DisplayAttribute>() != null ? p.GetCustomAttribute<DisplayAttribute>().Name : p.Name,
                    FormatAttribute = p.GetCustomAttribute<DisplayFormatAttribute>(),
                    Property = p,
                    Type = p.PropertyType.IsEnum ? typeof(Int32) : p.PropertyType,
                    Value = p.PropertyType.IsEnum ? enumToShort(o, p) : p.GetValue(o)
                });

            Dictionary<string, double> dict = new Dictionary<string, double>();

            foreach (var property in propertyInfos)
            {
                double? value = numericValue(property.Value, property.Type, property.FormatAttribute);

                if (value != null)
                {
                    dict.Add(property.Name, (double)value);
                }
            }
            return dict;

        }

        private static Double? numericValue(object value, Type type, DisplayFormatAttribute format)
        {
            if (type.IsEnum || (Nullable.GetUnderlyingType(type) != null && Nullable.GetUnderlyingType(type).IsEnum))
            {
                if (!Enum.IsDefined(type, value))
                    return Convert.ToDouble(value);

                var display = type.GetField(value.ToString()).GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                    return Convert.ToDouble(display.Name);

                return Convert.ToDouble(value);
            }
            return null;
        }

        private static string FormatValue(object value, Type type, DisplayFormatAttribute format)
        {
            if (value == null)
                return null;

            if (type.IsEnum || (Nullable.GetUnderlyingType(type) != null && Nullable.GetUnderlyingType(type).IsEnum))
            {
                if (!Enum.IsDefined(type, value))
                    return value.ToString();

                var display = type.GetField(value.ToString()).GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                    return display.Name;

                return value.ToString();
            }

            if (type == typeof(string))
                return (string)value;


            if (type == typeof(DateTime?) || type == typeof(DateTime))
                return ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (format != null)
                return string.Format(format.DataFormatString, value);

            return value.ToString();
        }
    }
}
