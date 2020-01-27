using GreekTheater.API.Core.Dtos.Performance;
using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Services.Sorting;
using GreekTheater.API.Helpers.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.Services.Sorting
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _performancePropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
               { "Id", new PropertyMappingValue(new List<string>() { "Guid" } ) },
               { "Title", new PropertyMappingValue(new List<string>() { "Title" } )},
               { "Year", new PropertyMappingValue(new List<string>() { "PremiereDate.Value.Year" } ) },
               { "Director", new PropertyMappingValue(new List<string>() { "Director.FirstName", "Director.LastName" }) }
          };

        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<PerformanceDto, Performance>(_performancePropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (!string.IsNullOrWhiteSpace(fields))
            {
                var fieldsAfterSplit = fields.Split(",");

                foreach (var field in fieldsAfterSplit)
                {
                    if (!propertyMapping.ContainsKey(GetPropertyName(field)))
                        return false;
                }
            }

            return true;
        }

        private string GetPropertyName(string field)
        {
            var trimmedField = field.Trim();
            var indexOfFirstSpace = trimmedField.IndexOf(" ");

            return indexOfFirstSpace == -1 ?
                trimmedField : trimmedField.Remove(indexOfFirstSpace);
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() != 1)
            {
                throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)},{typeof(TDestination)}");
            }

            return matchingMapping.First().MappingDictionary;
        }
    }
}
