using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.ActionConstraints
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequestHeaderMatchesMediaType : Attribute, IActionConstraint
    {
        private readonly MediaTypeCollection _mediaTypes = new MediaTypeCollection();
        private readonly string _requestHeaderToMatch;

        public int Order => 0;

        public RequestHeaderMatchesMediaType(string requestHeaderToMatch,
            string mediaType, params string[] otherMediaTypes)
        {
            _requestHeaderToMatch = requestHeaderToMatch
               ?? throw new ArgumentNullException(nameof(requestHeaderToMatch));
            AddMediaType(mediaType);
            otherMediaTypes.ToList().ForEach(mt => AddMediaType(mt));
        }

        private void AddMediaType(string mediaType)
        {
            if (MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
                _mediaTypes.Add(parsedMediaType);
            else
                throw new ArgumentException(nameof(mediaType));
        }

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;

            if (requestHeaders.ContainsKey(_requestHeaderToMatch))
            {
                var parsedRequestMediaType = new MediaType(requestHeaders[_requestHeaderToMatch]);

                foreach (var mediaType in _mediaTypes)
                {
                    var parsedMediaType = new MediaType(mediaType);
                    if (parsedRequestMediaType.Equals(parsedMediaType))
                        return true;
                }
            }

            return false;
        }
    }
}
