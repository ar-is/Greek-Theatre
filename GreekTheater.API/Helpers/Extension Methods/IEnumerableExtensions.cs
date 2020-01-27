using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.Extension_Methods
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
            this IEnumerable<TSource> source,
            string fields)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var propertyInfos = GetPropertyInfos<TSource>(fields);

            return GetExpandoObjects(source, propertyInfos);
        }

        private static IEnumerable<ExpandoObject> GetExpandoObjects<TSource>(
            IEnumerable<TSource> source, List<PropertyInfo> propertyInfos)
        {
            var expandoObjects = new List<ExpandoObject>();

            foreach (TSource sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject)
                        .Add(propertyInfo.Name, propertyValue);
                }

                expandoObjects.Add(dataShapedObject);
            }

            return expandoObjects;
        }

        private static List<PropertyInfo> GetPropertyInfos<TSource>(string fields)
        {
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource)
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase |
                        BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on" +
                            $" {typeof(TSource)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }

            return propertyInfoList;
        }
    }
}
