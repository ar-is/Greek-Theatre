using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.ResourceFilters
{
    public class PerformancesFilter
    {
        [FromQuery(Name = "title")]
        public string Title { get; set; }

        [FromQuery(Name = "year")]
        public int? PremiereYear { get; set; }

        [FromQuery(Name = "director")]
        public string Director { get; set; }

        public bool WithTitle()
        {
            if (!string.IsNullOrEmpty(Title) && string.IsNullOrWhiteSpace(Director) && PremiereYear == null)
            {
                Title = Title.Trim();
                return true;
            }

            return false;
        }

        public bool WithYear()
        {
            if (string.IsNullOrEmpty(Title) && string.IsNullOrWhiteSpace(Director) && PremiereYear != null)
                return true;

            return false;
        }

        public bool WithDirector()
        {
            if (!string.IsNullOrWhiteSpace(Director) && string.IsNullOrWhiteSpace(Title) && PremiereYear == null)
            {
                Director = Director.Trim();
                return true;
            }

            return false;
        }

        public bool WithTitleAndYear()
        {
            if (string.IsNullOrWhiteSpace(Director) && !string.IsNullOrWhiteSpace(Title) && PremiereYear != null)
            {
                Title = Title.Trim();
                return true;
            }

            return false;
        }

        public bool WithTitleAndDirector()
        {
            if (!string.IsNullOrWhiteSpace(Director) && !string.IsNullOrWhiteSpace(Title) && PremiereYear == null)
            {
                Title = Title.Trim();
                Director = Director.Trim();
                return true;
            }

            return false;
        }

        public bool WithTitleDirectorAndYear()
        {
            if (!string.IsNullOrWhiteSpace(Director) && !string.IsNullOrWhiteSpace(Title) && PremiereYear != null)
            {
                Title = Title.Trim();
                Director = Director.Trim();
                return true;
            }

            return false;
        }

        public bool WithYearAndDirector()
        {
            if (!string.IsNullOrWhiteSpace(Director) && string.IsNullOrWhiteSpace(Title) && PremiereYear != null)
            {
                Director = Director.Trim();
                return true;
            }

            return false;
        }
    }
}
