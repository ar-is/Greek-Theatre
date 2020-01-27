using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.Extension_Methods
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source,
             string fields)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrWhiteSpace(fields))
                return GetExpandoObject(source);

            return GetFilteredExpandoObject(source, fields);
        }

        private static ExpandoObject GetExpandoObject<TSource>(TSource source)
        {
            var dataShapedObject = new ExpandoObject();

            var propertyInfos = typeof(TSource)
                                    .GetProperties(BindingFlags.IgnoreCase |
                                    BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in propertyInfos)
                AddPropertyValuesToExpando(source, dataShapedObject, propertyInfo);

            return dataShapedObject;
        }

        private static ExpandoObject GetFilteredExpandoObject<TSource>(TSource source,
            string fields)
        {
            var dataShapedObject = new ExpandoObject();

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(TSource)
                    .GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    throw new Exception($"Property {propertyName} wasn't found " +
                        $"on {typeof(TSource)}");
                }

                AddPropertyValuesToExpando(source, dataShapedObject, propertyInfo);
            }

            return dataShapedObject;
        }

        private static void AddPropertyValuesToExpando<TSource>(TSource source,
            ExpandoObject dataShapedObject, PropertyInfo propertyInfo)
        {
            var propertyValue = propertyInfo.GetValue(source);

            ((IDictionary<string, object>)dataShapedObject)
                .Add(propertyInfo.Name, propertyValue);
        }
    }
}
