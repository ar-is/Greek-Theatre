using GreekTheater.API.Helpers.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.Extension_Methods
{
    public static class IQuerableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
               Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (mappingDictionary == null)
                throw new ArgumentNullException(nameof(mappingDictionary));

            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                string trimmedOrderByClause = orderByClause.Trim();
                bool isOrderDescending = trimmedOrderByClause.EndsWith(" desc");
                string propertyName = GetPropertyName(mappingDictionary, trimmedOrderByClause);
                var propertyMappingValue = GetPropertyMappingValue(mappingDictionary, propertyName);

                OrderSourcePerOrderByClause(ref source, ref isOrderDescending, propertyMappingValue);
            }

            return source;
        }

        private static void OrderSourcePerOrderByClause<T>(ref IQueryable<T> source,
            ref bool isOrderDescending,
            PropertyMappingValue propertyMappingValue)
        {
            foreach (var destinationProperty in
                                propertyMappingValue.DestinationProperties.Reverse())
            {
                if (propertyMappingValue.Revert)
                    isOrderDescending = !isOrderDescending;

                source = source.OrderBy(destinationProperty +
                    (isOrderDescending ? " descending" : " ascending"));
            }
        }

        private static PropertyMappingValue GetPropertyMappingValue(Dictionary<string, PropertyMappingValue> mappingDictionary, string propertyName)
        {
            var propertyMappingValue = mappingDictionary[propertyName];

            if (propertyMappingValue == null)
                throw new ArgumentNullException(nameof(propertyMappingValue));

            return propertyMappingValue;
        }

        private static string GetPropertyName(Dictionary<string, PropertyMappingValue> mappingDictionary, string trimmedOrderByClause)
        {
            var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
            var propertyName = indexOfFirstSpace == -1 ?
                trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

            if (!mappingDictionary.ContainsKey(propertyName))
                throw new ArgumentException($"Key mapping for {propertyName} is missing");

            return propertyName;
        }
    }
}
