using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.Extension_Methods
{
    public static class DateTimeExtensions
    {
        public static int? GetCurrentAge(this DateTimeOffset? dateTime, DateTimeOffset? dateOfDeath)
        {
            if (dateTime != null)
            {
                var lastDate = DateTimeOffset.Now;

                if (dateOfDeath != null)
                    lastDate = dateOfDeath.Value;

                var age = (lastDate.Year - dateTime.Value.Year);

                if (lastDate < dateTime.Value.AddYears(age))
                    age--;

                return age;
            }

            return null;
        }
    }
}
