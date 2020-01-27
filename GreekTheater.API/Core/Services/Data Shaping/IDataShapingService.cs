using GreekTheater.API.Helpers.Paging;
using GreekTheater.API.Helpers.ResourceFilters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Services.Data_Shaping
{
    public interface IDataShapingService<T> where T : class
    {
        object GetShapedCollectionWithLinks(PagedList<T> collection,
            ResourceParameters resourceParameters);
        IDictionary<string, object> GetShapedEntity(MediaTypeHeaderValue parsedMediaType,
            T entity, string fields);
    }
}
