using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SharedClasses
{

    public static class CloneClass
    {
        /// <summary>
        /// Clones a object via shallow copy
        /// </summary>
        /// <typeparam name="T">Object Type to Clone</typeparam>
        /// <param name="obj">Object to Clone</param>
        /// <returns>New Object reference</returns>
        /// 
        public static void copyProperties(object source, object destination)
        {
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] destinationProperties = destinationType.GetProperties();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo destinationProperty = Array.Find(destinationProperties,
                    prop => prop.Name == sourceProperty.Name && prop.PropertyType == sourceProperty.PropertyType);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    object value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }
        }
        public static T CloneObject<T>(this T obj) where T : class
        {
            if (obj == null) return null;
            System.Reflection.MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (inst != null)
                return (T)inst.Invoke(obj, null);
            else
                return null;
        }

        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public static class ObjectDataHelper
    {

        public static List<object> typedToObject<T>(List<T> list)
        {
            List<object> returnedData = new List<object>();
            foreach (T obj in list)
                returnedData.Add(obj);
            return returnedData;
        }

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
                decimal? value = numericValue(property.Value, property.Type);

                if (value != null)
                {
                    dict.Add(property.Name, (double)value);
                }
            }
            return dict;

        }

        private static Decimal? numericValue(object value, Type type)
        {
            if (type.IsEnum || (Nullable.GetUnderlyingType(type) != null && Nullable.GetUnderlyingType(type).IsEnum))
            {
                if (!Enum.IsDefined(type, value))
                    return Convert.ToDecimal(value);

                var display = type.GetField(value.ToString()).GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                    return Convert.ToDecimal(display.Name);

                return Convert.ToDecimal(value);
            }
            return null;
        }

        private static string FormatValue(object value, Type type, DisplayFormatAttribute format)
        {
            if (value == null)            
                return null;

            if (value is Enum)
                return value.ToString();

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

        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }
    }
}
